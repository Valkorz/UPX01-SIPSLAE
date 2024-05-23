using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore.Design;
using DataView.Controllers;

namespace tools{

    public static class Statistics{
        
        public const int Treshold = 9;
        public static async Task<double> OverflowFrequency(DbSet<MonitorBacklog> database)
        {
            var records = await database.ToListAsync();
            int overflowCount = records.Count(record => record.WaterLevel > (double)Treshold);
            double frequency = (double)overflowCount / records.Count;
            return frequency;
        }
    }
}
