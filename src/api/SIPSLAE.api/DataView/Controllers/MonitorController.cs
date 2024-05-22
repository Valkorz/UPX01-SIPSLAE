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

namespace DataView.Controllers
{
    //Database table information
    public class MonitorBacklog
    {
        public int Id {get; set;}
        public float WaterLevel {get; set;}
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

        [HttpGet("frequency")]
        public async Task<double> GetFrequency(){
            double frequency = await Statistics.OverflowFrequency(_context.logs);
            return frequency;
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
        public async Task<ActionResult<MonitorBacklog>> Post(MonitorBacklog item){
            item.TimeOfRecord = DateTime.Now;
            item.Id = _context.logs.Count() + 1;
            _context.logs.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Added: ", item);
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
            _context.logs.RemoveRange(_context.logs);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
