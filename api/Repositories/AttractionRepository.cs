using Adventour.Api.Data;
using Adventour.Api.Models;
using Adventour.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Adventour.Api.Repositories
{
    public class AttractionRepository : IAttractionRepository
    {
        private readonly AdventourContext db;
        private readonly ILogger<CountryRepository> logger;
        private const string logHeader = "## AttractionRepository ##: ";
        public AttractionRepository(ILogger<CountryRepository> logger, AdventourContext context)
        {
            this.logger = logger;
            db = context;
        }

        public IEnumerable<Attraction> GetBaseAttractionData(string countryCode)
        {
            try
            {
                var country = db.Country
                    .Where(c => c.Code.Equals(countryCode.ToUpper()))
                    .Include(c => c.Attractions)
                    .ThenInclude(a => a.AttractionImages)
                    .FirstOrDefault();

                if (country == null)
                {
                    logger.LogError($"{logHeader} Country code not found.");
                    return Enumerable.Empty<Attraction>();
                }

                var attractions = country.Attractions;

                List<Attraction> baseAttractions = attractions.Where(a => a.AttractionImages.Count > 0).Select(
                    a => new Attraction
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Description = a.Description,
                        AttractionImages = a.AttractionImages
                            .Where(i => i.IsMain)
                            .Select(
                                img => new AttractionImages
                                {
                                    PictureRef = img.PictureRef,
                                }
                            ).ToList()
                    }).ToList();

                return baseAttractions;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                return Enumerable.Empty<Attraction>();
            }
        }
    }
}
