using Adventour.Api.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Adventour.Api.Data
{
    public class AdventourContext : DbContext
    {
        public AdventourContext(DbContextOptions<AdventourContext> options) : base(options)
        {
        }

        public DbSet<Country> Country { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Attraction> Attraction { get; set; }
        public DbSet<AttractionInfoType> AttractionInfoType { get; set; }
        public DbSet<AttractionInfo> AttractionInfo { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<Favorites> Favorites { get; set; }
        //public DbSet<City> City { get; set; }
        public DbSet<AttractionImages> AttractionImages { get; set; }
        public DbSet<Itinerary> Itinerarie { get; set; }
        public DbSet<Day> Day { get; set; }
        public DbSet<Timeslot> Timeslot { get; set; }
        public DbSet<ReviewImages> ReviewImages { get; set; }
    }
}
