using System;
using System.Collections.Generic;

namespace JbHifi.WeatherReport.DataLibrary.Models
{
    public partial class Weatherreportapikey
    {
        public int Id { get; set; }
        public int Ratelimitperhour { get; set; }
        public string Createdby { get; set; } = null!;
        public string Updatedby { get; set; } = null!;
        public DateTime Createddate { get; set; }
        public DateTime Updateddate { get; set; }
    }
}
