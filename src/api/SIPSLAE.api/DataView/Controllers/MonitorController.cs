using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore.Design;
using tools;
using CsvHelper;
using System.IO;
using System.Globalization;

namespace DataView.Controllers
{
    //Database table information
    public class MonitorBacklog
    {
        public int Id {get; set;}
        public float WaterLevel {get; set;}
        public float Variation {get; set;}
        public int Month {get; set;}
        public bool IsRaining {get; set;}
        public double MinuteInterval {get; set;}
        public DateTime? TimeOfRecord {get; set;}      
    }

    //Database context
    public class MonitorContext : DbContext
    {
        //Pass context options to base constructor
        public MonitorContext(DbContextOptions<MonitorContext> opts) : base(opts) {}

        public DbSet<MonitorBacklog> logs {get; set;}
    }


    [ApiController]
    [Route("[controller]")]
    public class MonitorController : ControllerBase
    {
        private readonly MonitorContext _context; //Reference the database context the controller is reffering to

        public MonitorController(MonitorContext context){
            _context = context;
        }

        //Get all tasks in the database and return on get request
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<MonitorBacklog>>> Get(){
            return await _context.logs.ToListAsync();
        }

        //Convert database to csv file
        [HttpGet("csv")]
        public IActionResult DownloadCsv(){
            var logs = _context.logs.ToList();

            using(var memoryStream = new MemoryStream())
            using(var writer = new StreamWriter(memoryStream))
            using(var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)){
                csv.WriteRecords(logs);
                writer.Flush();
                return File(memoryStream.GetBuffer(), "text/csv", "logs.csv");
            }
        }

        //Order list by time descending, take last n values and return as a list.

        [HttpGet("getLevels")]
        public async Task<ActionResult<IEnumerable<MonitorBacklog>>> GetWaterLevels([FromQuery] int n)
        {
            return await _context.logs.OrderByDescending(log => log.TimeOfRecord)
                                    .Take(n)
                                    .ToListAsync();
        }

        [HttpGet("treshold")]
        public async Task<int> GetTreshold(){
            return await Task.Run( () => Statistics.Treshold );
        }

        [HttpGet("frequency")]
        public async Task<double> GetFrequency(){
            double frequency = await Statistics.OverflowFrequency(_context.logs);
            return frequency;
        }

        [HttpGet("variation")]
        public async Task<double> GetVariationAverage([FromQuery] int fromLast){
            var lastItems = await _context.logs.ToListAsync();
            float averageVariation = 0.0f;
            if(lastItems.Count() > 0){
                var sample = lastItems.Skip(Math.Max(0, lastItems.Count() - fromLast));
                averageVariation = sample.Sum(item => item.Variation) / fromLast;
            }
            return averageVariation;
        }

        [HttpGet("average")]
        public async Task<double> GetAverage([FromQuery] int fromLast){
            var items = await _context.logs.ToListAsync();
            double avg = 0;
            for(int i = items.Count() - fromLast; i < items.Count(); i++){
                avg += (double)items[i].WaterLevel >= Statistics.Treshold? 0 : (double)items[i].WaterLevel;
            }

            avg /= (double)fromLast;
            return avg;
        }

        [HttpPost("post")]
        public async Task<ActionResult> Post(MonitorBacklog item){
            //Time is measured in UTC so that the program doesn't have to be modified too much when changing localities.
            DateTime time = DateTime.UtcNow;
            var lastItems = await _context.logs.ToListAsync();

            item.TimeOfRecord = time;
            item.Id = _context.logs.Count() + 1;
            item.Month = time.Month;

            //Check if it's raining
            if(lastItems.Count() > 0){
                const int interval = 10;
                var lastItem = lastItems.ElementAt(lastItems.Count() - 1);

                //Get minutes interval between measures
                item.Variation = item.WaterLevel - lastItem.WaterLevel;
                TimeSpan? timeDifference = item.TimeOfRecord - lastItem.TimeOfRecord;
                if(timeDifference.HasValue){
                    item.MinuteInterval = timeDifference.Value.TotalMinutes;
                } else item.MinuteInterval = 0.0;

                var sample = lastItems.Skip(Math.Max(0, lastItems.Count() - interval));
                float averageVariation = sample.Sum(item => item.Variation) / interval;
                if(averageVariation >= 4) item.IsRaining = true;
                else item.IsRaining = false;

            } else {
                item.Variation = 0;
                item.IsRaining = false;
                item.MinuteInterval = 0.0;
            }

            _context.logs.Add(item);
            await _context.SaveChangesAsync();

            return Content($"Added: {item.ToString()}");
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<MonitorBacklog>> Delete(MonitorBacklog item){
            _context.logs.Remove(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Removed: ", item);
        }

        [HttpDelete("clear")]
        public async Task<ActionResult> ClearDatabase()
        {
            //Remove all items one by one because dbSet doesn't have a clear() method, bruh.
            var items  = _context.logs;
            foreach(var item in items){
                _context.logs.Remove(item);
            }
            await _context.SaveChangesAsync();
            return Content($"Cleared all data.");
        }
    }
}
