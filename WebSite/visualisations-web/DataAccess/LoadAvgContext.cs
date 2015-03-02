using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using visualisations_web.Models;

namespace visualisations_web.DataAccess
{
    public class LoadAvgContext : DbContext
    {
        public LoadAvgContext(string connectionString) : base(connectionString) { }

        public DbSet<LoadAvg> Readings { get; set; }
    }
}