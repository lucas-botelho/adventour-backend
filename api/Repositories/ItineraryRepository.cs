﻿using Adventour.Api.Requests.Itinerary;
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
                                .Where(i => i.UserId == user.Id)
                                .ToList();

            var result = new List<FullItineraryDetails>();

            foreach (var itinerary in itineraries)
            {
                var fullItineraryDetails = MapToFullItineraryDetails(itinerary, user);
                result.Add(fullItineraryDetails);
            }


            return result;
        }

        public bool DeleteItinerary(int itineraryId, Person user)
        {
            try
            {
                var itinerary = db.Itinerary
                    .Include(i => i.Days)
                        .ThenInclude(d => d.Timeslots)
                    .FirstOrDefault(i => i.Id == itineraryId && i.UserId == user.Id);

                if (itinerary == null)
                {
                    logger.LogWarning($"{logHeader} Tried to delete non-existent or unauthorized itinerary with ID: {itineraryId}");
                    return false;
                }

                db.Itinerary.Remove(itinerary);
                db.SaveChanges();
                logger.LogInformation($"{logHeader} Itinerary {itineraryId} deleted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} Error deleting itinerary {itineraryId}: {ex.Message}");
                return false;
            }
        }

        public bool UpdateItinerary(int itineraryId, ItineraryRequest request, Person user)
        {
            try
            {
                var itinerary = db.Itinerary
                    .Include(i => i.Days)
                        .ThenInclude(d => d.Timeslots)
                    .FirstOrDefault(i => i.Id == itineraryId && i.UserId == user.Id);

                if (itinerary == null || request == null)
                    return false;

                itinerary.Title = string.IsNullOrEmpty(request.Name) ? itinerary.Title : request.Name;

                // Atualizar dias existentes, adicionar novos e remover os que foram apagados
                var updatedDays = new List<Day>();

                foreach (var dayReq in request.Days ?? new())
                {
                    var existingDay = itinerary.Days.FirstOrDefault(d => d.Id == dayReq.Id);

                    if (existingDay != null)
                    {
                        // Atualiza dia
                        existingDay.DayNumber = dayReq.DayNumber;

                        var updatedTimeSlots = new List<Timeslot>();

                        foreach (var tsReq in dayReq.Timeslots ?? new())
                        {
                            var existingTs = existingDay.Timeslots.FirstOrDefault(t => t.Id == tsReq.Id);

                            if (existingTs != null)
                            {
                                existingTs.StartTime = DateTime.Parse(tsReq.StartTime);
                                existingTs.EndTime = DateTime.Parse(tsReq.EndTime);
                                existingTs.AttractionId = tsReq.AttractionId;
                                updatedTimeSlots.Add(existingTs);
                            }
                            else
                            {
                                updatedTimeSlots.Add(new Timeslot
                                {
                                    StartTime = DateTime.Parse(tsReq.StartTime),
                                    EndTime = DateTime.Parse(tsReq.EndTime),
                                    AttractionId = tsReq.AttractionId
                                });
                            }
                        }

                        existingDay.Timeslots = updatedTimeSlots;
                        updatedDays.Add(existingDay);
                    }
                    else
                    {
                        updatedDays.Add(new Day
                        {
                            DayNumber = dayReq.DayNumber,
                            Timeslots = dayReq.Timeslots?.Select(ts => new Timeslot
                            {
                                StartTime = DateTime.Parse(ts.StartTime),
                                EndTime = DateTime.Parse(ts.EndTime),
                                AttractionId = ts.AttractionId
                            }).ToList() ?? new()
                        });
                    }
                }

                itinerary.Days = updatedDays;

                db.SaveChanges();

                return true;

            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                return false;
            }
        }

    }
}
