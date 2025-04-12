using Adventour.Api.Data;
using Adventour.Api.Models.Attractions;
using Adventour.Api.Models.Database;
using Adventour.Api.Models.TimeSlots;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Requests.TimeSlot;
using Microsoft.EntityFrameworkCore;
using System;

namespace Adventour.Api.Repositories
{
    public class TimeSlotRepository : ITimeSlotRepository
    {
        private readonly AdventourContext db;
        private readonly ILogger<CountryRepository> logger;
        private const string logHeader = "## TimeSlotRepository ##: ";

        public TimeSlotRepository(ILogger<CountryRepository> logger, AdventourContext context)
        {
            this.logger = logger;
            db = context;
        }

        public BasicTimeSlotDetails? AddTimeSlot(AddTimeSlotRequest request)
        {
            try
            {
                var day = db.Day.FirstOrDefault(d => d.Id == request.DayId);
                if (day is null)
                {
                    logger.LogError($"{logHeader} Dia com ID {request.DayId} não encontrado.");
                    return null;
                }

                bool overlapExists = db.Timeslot.Any(ts =>
                    ts.DayId == request.DayId &&
                    ts.StartTime < request.EndTime &&
                    ts.EndTime > request.StartTime
                );

                if (overlapExists)
                {
                    logger.LogError($"{logHeader} Já existe um TimeSlot que se sobrepõe no mesmo dia e horário.");
                    throw new Exception("Já existe um TimeSlot que se sobrepõe no mesmo dia e horário.");
                }

                var newTimeSlot = new Timeslot
                {
                    DayId = request.DayId,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    AttractionId = 1
                };

                db.Timeslot.Add(newTimeSlot);
                db.SaveChanges();

                var createdTimeSlot = db.Timeslot
                    .Include(ts => ts.Attraction)
                        .ThenInclude(a => a.AttractionImages)
                    .FirstOrDefault(ts => ts.Id == newTimeSlot.Id);

                if (createdTimeSlot == null)
                {
                    logger.LogError($"{logHeader} Erro ao recarregar o TimeSlot após criação.");
                    return null;
                }

                return new BasicTimeSlotDetails
                {
                    Id = createdTimeSlot.Id,
                    DayId = createdTimeSlot.DayId,
                    StartTime = createdTimeSlot.StartTime,
                    EndTime = createdTimeSlot.EndTime,
                    Attraction = createdTimeSlot.Attraction != null
                        ? new BasicAttractionDetails
                        {
                            Id = createdTimeSlot.Attraction.Id,
                            Name = createdTimeSlot.Attraction.Name,
                            Description = createdTimeSlot.Attraction.Description,
                            IsFavorited = db.Favorites.Any(f =>
                                f.AttractionId == createdTimeSlot.Attraction.Id
                                ),
                            AttractionImages = createdTimeSlot.Attraction.AttractionImages
                                .Where(img => img.IsMain)
                                .Select(img => new AttractionImages
                                {
                                    PictureRef = img.PictureRef
                                })
                                .ToList()
                        }
                        : null
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                throw new Exception(ex.Message);
            }
        }


        // ...

        public bool RemoveTimeSlot(int idTimeSlot)
        {
            try
            {
                var timeslot = db.Timeslot.FirstOrDefault(ts => ts.Id == idTimeSlot);
                if (timeslot is null)
                {
                    var errorMessage = $"{logHeader} Timeslot com ID {idTimeSlot} não encontrado.";
                    logger.LogError(errorMessage);
                }

                db.Timeslot.Remove(timeslot);
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
