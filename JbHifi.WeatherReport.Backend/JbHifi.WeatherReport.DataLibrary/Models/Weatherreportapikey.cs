namespace JbHifi.WeatherReport.DataLibrary.Models
{
    public partial class Weatherreportapikey
    {
        public Weatherreportapikey()
        {
            Audits = new HashSet<Audit>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid Uniqueid { get; set; }
        public int Ratelimitperhour { get; set; }
        public string Createdby { get; set; } = null!;
        public string Updatedby { get; set; } = null!;
        public DateTime Createddate { get; set; }
        public DateTime Updateddate { get; set; }

        public virtual ICollection<Audit> Audits { get; set; }
    }
}
