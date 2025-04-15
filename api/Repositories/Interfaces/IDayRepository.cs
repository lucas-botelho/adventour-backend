using Adventour.Api.Responses.Attractions;
using Adventour.Api.Models.Database;
using Adventour.Api.Requests.TimeSlot;
using Adventour.Api.Responses.Day;
using Microsoft.AspNetCore.Mvc;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface IDayRepository
    {
        BasicDayDetails AddDay([FromBody] AddDayRequest request);
        bool RemoveDay(int dayId);
        int CalculateNextDayNumber(int itineraryId);
    }
}
