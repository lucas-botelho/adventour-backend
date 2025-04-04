using Adventour.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Adventour.Api.Data
{
    public class AdventourContext : DbContext
    {
        public AdventourContext(DbContextOptions<AdventourContext> options) : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<AttractionInfoType> AttractionInfoTypes { get; set; }
        public DbSet<AttractionInfo> AttractionInfos { get; set; }
        public DbSet<AttractionImages> AttractionImages { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Favorites> Favorites { get; set; }
        public DbSet<Itinerary> Itineraries { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Timeslot> Timeslots { get; set; }
        public DbSet<ReviewImages> ReviewImages { get; set; }
    }
}
