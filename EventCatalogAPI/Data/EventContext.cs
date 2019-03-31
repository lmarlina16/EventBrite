using EventCatalogAPI.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventCatalogAPI.Data
{
    public class EventContext : DbContext
    {
        //dependency injection: pass options to EF Core base class to create the database
        public EventContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<EventCategory> EventCategories { get; set; }

        public DbSet<EventMetroArea> EventMetroAreas { get; set; }

        public DbSet<Event> Events { get; set; }

        //override it onCreating a table
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventCategory>(ConfigureEevntCategory);
            modelBuilder.Entity<EventMetroArea>(ConfigureEventMetroArea);
            modelBuilder.Entity<Event>(ConfigureEevnt);

        }

        private void ConfigureEevnt(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events");
            builder.Property(c => c.Id)
                .IsRequired(true)
                .ForSqlServerUseSequenceHiLo("event_hilo");

            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Date)
                .IsRequired();

            builder.Property(c => c.Address1).IsRequired();
            builder.Property(c => c.City).IsRequired();
            builder.Property(c => c.State).IsRequired();
            builder.Property(c => c.ZipCode).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(250);

            builder.HasOne(c => c.EventCategory)
                .WithMany() //1:M relationship
                .HasForeignKey(c => c.EventCategoryId);

            builder.HasOne(c => c.EventMetroArea)
                .WithMany() //1:M relationship
                .HasForeignKey(c => c.EventMetroAreaId);

        }

        private void ConfigureEventMetroArea(EntityTypeBuilder<EventMetroArea> builder)
        {
            builder.ToTable("EventMetroAreas");
            builder.Property(c => c.Id)
                .IsRequired(true)
                .ForSqlServerUseSequenceHiLo("event_metro_area_hilo");

            builder.Property(c => c.MetroArea)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.State)
                .IsRequired();
        }

        private void ConfigureEevntCategory(EntityTypeBuilder<EventCategory> builder)
        {
            builder.ToTable("EventCategories");
            builder.Property(c => c.Id)
                .IsRequired(true)
                .ForSqlServerUseSequenceHiLo("event_category_hilo");

            builder.Property(c => c.Category)
                .IsRequired()
                .HasMaxLength(100);

        }
    }
}
