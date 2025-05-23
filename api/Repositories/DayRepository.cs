﻿using Adventour.Api.Data;
using Adventour.Api.Models.Database;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Adventour.Api.Requests.TimeSlot;
using Azure.Core;
using Adventour.Api.Responses.Day;

namespace Adventour.Api.Repositories
{
    public class DayRepository : IDayRepository
    {
        private readonly AdventourContext db;
        private readonly ILogger<DayRepository> logger;
        private const string logHeader = "## DayRepository ##: ";

        public DayRepository(ILogger<DayRepository> logger, AdventourContext context)
        {
            this.logger = logger;
            db = context;
        }

        public DayDetails AddDay([FromBody] AddDayRequest request)
        {
            try
            {
                var itinerary = db.Itinerary.FirstOrDefault(i => i.Id == request.itineraryId);
                if (itinerary is null)
                {
                    logger.LogError($"{logHeader} Itinerary with ID {request.itineraryId} not found!");
                    throw new NotFoundException($"Itinerary with ID {request.itineraryId} not found!");
                }

                var newDay = new Day
                {
                    ItineraryId = request.itineraryId,
                    DayNumber = CalculateNextDayNumber(request.itineraryId)
                };

                db.Day.Add(newDay);
                db.SaveChanges();

                var createdDay = db.Day
                    .Include(d => d.Itinerary)
                    .FirstOrDefault(d => d.Id == newDay.Id);

                if (createdDay == null)
                {
                    logger.LogError($"{logHeader} Error reloading Day after creation.");
                    throw new NotFoundException("Error reloading Day after creation.");
                }


                return new DayDetails
                {
                    Id = newDay.Id,
                    ItineraryId = newDay.ItineraryId,
                    DayNumber = newDay.DayNumber
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                throw;
            }
        }

        public bool RemoveDay(int dayId)
        {
            try
            {
                var day = db.Day.FirstOrDefault(d => d.Id == dayId);
                if (day is null)
                {
                    logger.LogError($"{logHeader} Day with ID {dayId} not found!");
                    throw new NotFoundException($"Day with ID {dayId} not found!");
                }

                db.Day.Remove(day);
                db.SaveChanges();

                RenumberDays(day.ItineraryId);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                throw;
            }
        }

        public int CalculateNextDayNumber(int itineraryId)
        {
           try
            {
                var nextDayNumber = 1;

                var existingDays = db.Day
                    .Where(d => d.ItineraryId == itineraryId);

                if (existingDays.Any())
                {
                    nextDayNumber = existingDays.Max(d => d.DayNumber) + 1;
                }

                return nextDayNumber;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                throw;
            }
        }

        private void RenumberDays(int itineraryId)
        {
            var days = db.Day
                .Where(d => d.ItineraryId == itineraryId)
                .OrderBy(d => d.DayNumber)
                .ToList();

            int counter = 1;

            foreach (var day in days)
            {
                day.DayNumber = counter++;
            }

            db.SaveChanges();
        }

    }
}
