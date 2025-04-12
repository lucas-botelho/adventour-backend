using Adventour.Api.Models.Attractions;
using Adventour.Api.Models.Database;
using Adventour.Api.Models.Day;
using Adventour.Api.Requests.TimeSlot;
using Microsoft.AspNetCore.Mvc;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface IDayRepository
    {
        BasicDayDetails AddDay([FromBody] AddDayRequest request);
        int CalculateNextDayNumber(int itineraryId);
    }
}
