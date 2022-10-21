namespace JbHifi.WeatherReport.DataLibrary.Models
{
    public partial class Audit
    {
        public int Id { get; set; }
        public int Weatherreportapikeysid { get; set; }
        public string Createdby { get; set; } = null!;
        public string Updatedby { get; set; } = null!;
        public DateTime Createddate { get; set; }
        public DateTime Updateddate { get; set; }

        public virtual Weatherreportapikey Weatherreportapikeys { get; set; } = null!;
    }
}
