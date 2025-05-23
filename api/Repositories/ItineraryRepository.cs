using Adventour.Api.Requests.Itinerary;
using Adventour.Api.Exceptions;
using Adventour.Api.Data;
using Adventour.Api.Models.Database;
using Adventour.Api.Responses.Attractions;
using Adventour.Api.Responses.TimeSlots;
using Microsoft.EntityFrameworkCore;
using Adventour.Api.Responses.Day;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Responses.Itinerary;

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

        public Itinerary AddItinerary(ItineraryRequest request, Person user)
        {
            if (request == null || user == null)
            {
                return null;
            }

            try
            {

                var itinerary = new Itinerary
                {
                    Title = string.IsNullOrEmpty(request.Name) ? "New Itinerary" : request.Name,
                    UserId = user.Id,
                    CreatedAt = DateTime.Now,
                    Days = request.Days?.Select(dayRequest => new Day
                    {
                        DayNumber = dayRequest.DayNumber,
                        Timeslots = dayRequest.Timeslots?.Select(timeslotRequest => new Timeslot
                        {
                            StartTime = DateTime.Parse(timeslotRequest.StartTime),
                            EndTime = DateTime.Parse(timeslotRequest.EndTime),
                            AttractionId = timeslotRequest.AttractionId
                        }).ToList() ?? new List<Timeslot>()
                    }).ToList() ?? new List<Day>()
                };


                db.Itinerary.Add(itinerary);
                db.SaveChanges();


                return db.Itinerary
                    .Include(i => i.Days)
                        .ThenInclude(d => d.Timeslots)
                            .ThenInclude(t => t.Attraction)
                    .Where(i => i.Id == itinerary.Id)
                    .Select(i => new Itinerary
                    {
                        Id = i.Id,
                        Title = i.Title,
                        CreatedAt = i.CreatedAt,
                        UserId = i.UserId,
                        Days = i.Days.Select(d => new Day
                        {
                            Id = d.Id,
                            DayNumber = d.DayNumber,
                            Timeslots = d.Timeslots.Select(ts => new Timeslot
                            {
                                Id = ts.Id,
                                StartTime = ts.StartTime,
                                EndTime = ts.EndTime,
                                AttractionId = ts.AttractionId,
                                Attraction = new Attraction
                                {
                                    Id = ts.Attraction.Id,
                                    Name = ts.Attraction.Name
                                }
                            }).ToList()
                        }).ToList()
                    })
                    .FirstOrDefault()! ;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
            }

            return null;
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
                    .Select(d => new DayDetails
                    {
                        Id = d.Id,
                        ItineraryId = d.ItineraryId,
                        DayNumber = d.DayNumber,
                        Timeslots = d.Timeslots
                            .OrderBy(ts => ts.StartTime)
                            .Select(ts => new TimeSlotDetails
                            {
                                Id = ts.Id,
                                DayId = ts.DayId,
                                StartTime = ts.StartTime,
                                EndTime = ts.EndTime,
                                Attraction = ts.Attraction != null
                                    ? new AttractionDetails
                                    {
                                        Id = ts.Attraction.Id,
                                        Name = ts.Attraction.Name,
                                        ShortDescription = ts.Attraction.ShortDescription,
                                        LongDescription = ts.Attraction.LongDescription,
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

        public IEnumerable<FullItineraryDetails> GetUserItineraries(Person user, Country country)
        {

            var itineraries = db.Itinerary
                                .Include(i => i.Days)
                                    .ThenInclude(d => d.Timeslots)
                                        .ThenInclude(ts => ts.Attraction)
                                            .ThenInclude(a => a.AttractionImages)
                                .Where(i => i.UserId == user.Id &&
                                            i.Days.Any(d =>
                                                d.Timeslots.Any(ts =>
                                                    ts.Attraction != null && ts.Attraction.CountryId == country.Id)))
                                .ToList();



            var result = new List<FullItineraryDetails>();

            foreach (var itinerary in itineraries)
            {
                var fullItineraryDetails = MapToFullItineraryDetails(itinerary, user);
                result.Add(fullItineraryDetails);
            }


            return result;
        }
    }
}
