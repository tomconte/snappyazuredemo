using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace visualisations_web.Models
{
    [Table("loadavg")]
    public class LoadAvg
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("time")]
        public DateTime Time { get; set; }

		[Column("device")]
		public string Device { get; set; }

        [Column("load1")]
        public double Load1 { get; set; }

		[Column("load5")]
		public double Load5 { get; set; }

		[Column("load15")]
		public double Load15 { get; set; }
	}
}
