using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetRoute.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetRoute.API.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Description> Descriptions { get; set; }
        public DbSet<DocumentType> documentTypes { get; set; }
        public DbSet<HistoryTrip> historyTrips { get; set; }
        public DbSet<Pet> pet { get; set; }
        public DbSet<PetType> petTypes { get; set; }
        public DbSet<PhotoPet> photoPets { get; set; }
        public DbSet<Race> races { get; set; }
        public DbSet<ScheduledTrip> scheduledTrips { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripInProgress> TripInProgresses { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<DocumentType>().HasIndex(x => x.Description).IsUnique();
            builder.Entity<PetType>().HasIndex(x => x.Description).IsUnique();
            builder.Entity<Race>().HasIndex(x => x.Description).IsUnique();

        }
    }
}
