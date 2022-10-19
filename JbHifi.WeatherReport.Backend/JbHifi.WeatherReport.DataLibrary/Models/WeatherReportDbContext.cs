using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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

        public virtual DbSet<Openweatherserviceapikey?> Openweatherserviceapikeys { get; set; } = null!;
        public virtual DbSet<Weatherreportapikey> Weatherreportapikeys { get; set; } = null!;
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

                entity.Property(e => e.Ratelimitperhour)
                    .HasColumnName("ratelimitperhour")
                    .HasDefaultValueSql("5");

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
