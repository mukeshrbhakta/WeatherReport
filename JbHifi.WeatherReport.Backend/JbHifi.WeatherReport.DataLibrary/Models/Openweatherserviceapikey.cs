using System;
using System.Collections.Generic;

namespace JbHifi.WeatherReport.DataLibrary.Models
{
    public partial class Openweatherserviceapikey
    {
        public int Id { get; set; }
        public string Apikey { get; set; } = null!;
        public string Createdby { get; set; } = null!;
        public string Updatedby { get; set; } = null!;
        public DateTime Createddate { get; set; }
        public DateTime Updateddate { get; set; }
    }
}
