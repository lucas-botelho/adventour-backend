using Adventour.Api.Requests.Itinerary;
using Adventour.Api.Models.Itinerary;
using Adventour.Api.Exceptions;
using Adventour.Api.Data;
using Adventour.Api.Models.Database;
using Adventour.Api.Models.Attractions;
using Adventour.Api.Models.TimeSlots;
using Microsoft.EntityFrameworkCore;
using Adventour.Api.Models.Day;
using Adventour.Api.Repositories.Interfaces;

namespace Adventour.Api.Repositories
{
    public class ItineraryRepository : IItineraryRepository
    {
        private readonly AdventourContext db;
        private readonly ILogger<ItineraryRepository> logger;
        private const string logHeader = "## ItineraryRepository ##: ";

        public ItineraryRepository(ILogger<ItineraryRepository> logger, AdventourContext context)
        {
            this.logger = logger;
            db = context;
        }

        public FullItineraryDetails AddItinerary(AddItineraryRequest request)
        {
            try
            {
                Person? user = db.Person.FirstOrDefault(person => person.OauthId != null && person.OauthId.Equals(request.UserId));
                if (user == null)
                {
                    logger.LogError($"{logHeader} User not found with ID: {request.UserId}.");
                    throw new NotFoundException($"User not found with ID: {request.UserId}.");
                }

                Itinerary itinerary = new Itinerary
                {
                    Title = request.Title,
                    UserId = user.Id,
                    CreatedAt = DateTime.Now
                };

                db.Itinerary.Add(itinerary);
                db.SaveChanges();

                return GetItineraryById(itinerary.Id, request.UserId);
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                throw;
            }
        }

        public FullItineraryDetails GetItineraryById(int itineraryId, string userId)
        {
            var user = db.Person.FirstOrDefault(p => p.OauthId == userId);
            if (user == null)
            {
                logger.LogError($"{logHeader} User not found with ID: {userId}.");
                throw new NotFoundException($"User not found with ID: {userId}.");
            }

            var itinerary = db.Itinerary
                .Include(i => i.Days)
                    .ThenInclude(d => d.Timeslots)
                        .ThenInclude(ts => ts.Attraction)
                            .ThenInclude(a => a.AttractionImages)
                .FirstOrDefault(i => i.Id == itineraryId && i.UserId == user.Id);

            if (itinerary == null)
            {
                logger.LogError($"{logHeader} Itinerary not found with ID: {itineraryId}.");
                throw new NotFoundException($"Itinerary not found with ID: {itineraryId}.");
            }

            return MapToFullItineraryDetails(itinerary, user);
        }

        private FullItineraryDetails MapToFullItineraryDetails(Itinerary itinerary, Person user)
        {
            return new FullItineraryDetails
            {
                Id = itinerary.Id,
                Title = itinerary.Title,
                CreatedAt = itinerary.CreatedAt,
                Days = itinerary.Days
                    .OrderBy(d => d.DayNumber)
                    .Select(d => new BasicDayDetails
                    {
                        Id = d.Id,
                        ItineraryId = d.ItineraryId,
                        DayNumber = d.DayNumber,
                        Timeslots = d.Timeslots
                            .OrderBy(ts => ts.StartTime)
                            .Select(ts => new BasicTimeSlotDetails
                            {
                                Id = ts.Id,
                                DayId = ts.DayId,
                                StartTime = ts.StartTime,
                                EndTime = ts.EndTime,
                                Attraction = ts.Attraction != null
                                    ? new BasicAttractionDetails
                                    {
                                        Id = ts.Attraction.Id,
                                        Name = ts.Attraction.Name,
                                        Description = ts.Attraction.Description,
                                        IsFavorited = db.Favorites.Any(f =>
                                            f.AttractionId == ts.Attraction.Id &&
                                            f.UserId == user.Id
                                        ),
                                        AttractionImages = ts.Attraction.AttractionImages
                                            .Where(img => img.IsMain)
                                            .Select(img => new AttractionImages
                                            {
                                                PictureRef = img.PictureRef
                                            })
                                            .ToList()
                                    }
                                    : null
                            }).ToList()
                    }).ToList()
            };
        }

    }
}
