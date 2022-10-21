using Microsoft.EntityFrameworkCore;

namespace JbHifi.WeatherReport.DataLibrary.Models
{
    public partial class WeatherReportDbContext : DbContext
    {
        public WeatherReportDbContext()
        {
        }

        public WeatherReportDbContext(DbContextOptions<WeatherReportDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Audit> Audits { get; set; } = null!;
        public virtual DbSet<Openweatherserviceapikey> Openweatherserviceapikeys { get; set; } = null!;
        public virtual DbSet<Weatherreportapikey> Weatherreportapikeys { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Audit>(entity =>
            {
                entity.ToTable("audit");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Createdby)
                    .HasMaxLength(100)
                    .HasColumnName("createdby")
                    .HasDefaultValueSql("CURRENT_USER");

                entity.Property(e => e.Createddate)
                    .HasColumnName("createddate")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                entity.Property(e => e.Updatedby)
                    .HasMaxLength(100)
                    .HasColumnName("updatedby")
                    .HasDefaultValueSql("CURRENT_USER");

                entity.Property(e => e.Updateddate)
                    .HasColumnName("updateddate")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                entity.Property(e => e.Weatherreportapikeysid).HasColumnName("weatherreportapikeysid");

                entity.HasOne(d => d.Weatherreportapikeys)
                    .WithMany(p => p.Audits)
                    .HasForeignKey(d => d.Weatherreportapikeysid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_audit_weatherreportapikeys");
            });

            modelBuilder.Entity<Openweatherserviceapikey>(entity =>
            {
                entity.ToTable("openweatherserviceapikeys");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Apikey)
                    .HasMaxLength(50)
                    .HasColumnName("apikey");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(100)
                    .HasColumnName("createdby")
                    .HasDefaultValueSql("CURRENT_USER");

                entity.Property(e => e.Createddate)
                    .HasColumnName("createddate")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                entity.Property(e => e.Updatedby)
                    .HasMaxLength(100)
                    .HasColumnName("updatedby")
                    .HasDefaultValueSql("CURRENT_USER");

                entity.Property(e => e.Updateddate)
                    .HasColumnName("updateddate")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");
            });

            modelBuilder.Entity<Weatherreportapikey>(entity =>
            {
                entity.ToTable("weatherreportapikeys");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Createdby)
                    .HasMaxLength(100)
                    .HasColumnName("createdby")
                    .HasDefaultValueSql("CURRENT_USER");

                entity.Property(e => e.Createddate)
                    .HasColumnName("createddate")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Ratelimitperhour)
                    .HasColumnName("ratelimitperhour")
                    .HasDefaultValueSql("5");

                entity.Property(e => e.Uniqueid)
                    .HasColumnName("uniqueid")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.Updatedby)
                    .HasMaxLength(100)
                    .HasColumnName("updatedby")
                    .HasDefaultValueSql("CURRENT_USER");

                entity.Property(e => e.Updateddate)
                    .HasColumnName("updateddate")
                    .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
