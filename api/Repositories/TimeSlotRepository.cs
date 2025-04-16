using Adventour.Api.Data;
using Adventour.Api.Responses.Attractions;
using Adventour.Api.Models.Database;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Requests.TimeSlot;
using Microsoft.EntityFrameworkCore;
using Adventour.Api.Exceptions;
using Adventour.Api.Responses.TimeSlots;

namespace Adventour.Api.Repositories
{
    public class TimeSlotRepository : ITimeSlotRepository
    {
        private readonly AdventourContext db;
        private readonly ILogger<TimeSlotRepository> logger;
        private const string logHeader = "## TimeSlotRepository ##: ";

        public TimeSlotRepository(ILogger<TimeSlotRepository> logger, AdventourContext context)
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
                    logger.LogError($"{logHeader} Day with ID {request.DayId} not found!");
                    throw new NotFoundException($"Day with ID {request.DayId} not found!");
                }

                var attraction = db.Attraction.FirstOrDefault(a => a.Id == request.AttractionId);
                if (attraction is null)
                {
                    logger.LogError($"{logHeader} Attraction with ID {request.AttractionId} not found!");
                    throw new NotFoundException($"Attraction with ID {request.AttractionId} not found!");
                }

                bool overlapExists = db.Timeslot.Any(ts => 
                    ts.DayId == request.DayId &&
                    ts.StartTime < request.EndTime &&
                    ts.EndTime > request.StartTime
                );
                if (overlapExists)
                {
                    logger.LogError($"{logHeader} There is already a TimeSlot that overlaps on the same day and time.");
                    throw new ConflictException("There is already a TimeSlot that overlaps on the same day and time.");
                }

                var newTimeSlot = new Timeslot
                {
                    DayId = request.DayId,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    AttractionId = request.AttractionId
                };

                db.Timeslot.Add(newTimeSlot);
                db.SaveChanges();

                var createdTimeSlot = db.Timeslot
                    .Include(ts => ts.Attraction)
                        .ThenInclude(a => a.AttractionImages)
                    .FirstOrDefault(ts => ts.Id == newTimeSlot.Id);

                if (createdTimeSlot == null)
                {
                    logger.LogError($"{logHeader} Error reloading TimeSlot after creation.");
                    throw new NotFoundException("Error reloading TimeSlot after creation.");
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
                throw;
            }
        }

        public bool RemoveTimeSlot(int idTimeSlot)
        {
            try
            {
                var timeslot = db.Timeslot.FirstOrDefault(ts => ts.Id == idTimeSlot);
                if (timeslot is null)
                {
                    logger.LogError($"{logHeader} Timeslot with ID {idTimeSlot} not found.");
                    throw new NotFoundException($"Timeslot with ID {idTimeSlot} not found!");
                }

                db.Timeslot.Remove(timeslot);
                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                throw;
            }
        }

        public BasicTimeSlotDetails UpdateTimeSlot(UpdateTimeSlotRequest request)
        {
            var timeSlot = db.Timeslot
                .Include(ts => ts.Attraction)
                    .ThenInclude(a => a.AttractionImages)
                .FirstOrDefault(ts => ts.Id == request.TimeSlotId);
            if (timeSlot == null)
            {
                logger.LogError($"{logHeader} TimeSlot with ID {request.TimeSlotId} not found.");
                throw new NotFoundException($"TimeSlot with ID {request.TimeSlotId} not found.");
            }

            var day = db.Day.FirstOrDefault(d => d.Id == request.DayId);
            if (day is null)
            {
                logger.LogError($"{logHeader} Day with ID {request.DayId} not found!");
                throw new NotFoundException($"Day with ID {request.DayId} not found!");
            }

            var attraction = db.Attraction.FirstOrDefault(a => a.Id == request.AttractionId);
            if (attraction is null)
            {
                logger.LogError($"{logHeader} Attraction with ID {request.AttractionId} not found!");
                throw new NotFoundException($"Attraction with ID {request.AttractionId} not found!");
            }

            bool overlapExists = db.Timeslot.Any(ts =>
                ts.Id != request.TimeSlotId &&
                ts.DayId == request.DayId &&
                ts.StartTime < request.EndTime &&
                ts.EndTime > request.StartTime
            );
            if (overlapExists)
            {
                logger.LogError($"{logHeader} There is already a TimeSlot that overlaps on the same day and time.");
                throw new ConflictException("There is already a TimeSlot that overlaps on the same day and time.");
            }

            timeSlot.DayId = request.DayId;
            timeSlot.StartTime = request.StartTime;
            timeSlot.EndTime = request.EndTime;
            timeSlot.AttractionId = request.AttractionId;

            db.SaveChanges();

            return new BasicTimeSlotDetails
            {
                Id = timeSlot.Id,
                DayId = timeSlot.DayId,
                StartTime = timeSlot.StartTime,
                EndTime = timeSlot.EndTime,
                Attraction = timeSlot.Attraction != null
                    ? new BasicAttractionDetails
                    {
                        Id = timeSlot.Attraction.Id,
                        Name = timeSlot.Attraction.Name,
                        Description = timeSlot.Attraction.Description,
                        IsFavorited = db.Favorites.Any(f =>
                            f.AttractionId == timeSlot.Attraction.Id),
                        AttractionImages = timeSlot.Attraction.AttractionImages
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


    }
}
