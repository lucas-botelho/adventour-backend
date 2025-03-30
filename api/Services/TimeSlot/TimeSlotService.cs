using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Requests.TimeSlot;
using SendGrid.Helpers.Errors.Model;

namespace Adventour.Api.Services.TimeSlot
{
    public class TimeSlotService : ITimeSlotService
    {
        private readonly ITimeSlotRepository timeSlotRepository;
        private readonly IAttractionRepository attractionRepository;
        private readonly IDayRepository dayRepository;

        public TimeSlotService(
            ITimeSlotRepository timeSlotRepository,
            IAttractionRepository attractionRepository,
            IDayRepository dayRepository)
        {
            this.timeSlotRepository = timeSlotRepository;
            this.attractionRepository = attractionRepository;
            this.dayRepository = dayRepository;
        }

        public int AddTimeSlot(AddTimeSlotRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null");
            }

            var attraction = attractionRepository.GetAttractionById(request.AttractionId);
            if (attraction == null)
            {
                throw new NotFoundException("Invalid AttractionId");
            }

            var day = dayRepository.GetDayById(request.DayId);
            if (day == null)
            {
                throw new NotFoundException("Invalid DayId");
            }

            if (request.StartTime >= request.EndTime)
            {
                throw new ArgumentException("Start time cannot be greater than or equal to end time");
            }

            /*var overlappingTimeSlots = timeSlotRepository.GetTimeSlotsByDayId(request.DayId)
                .Where(ts => request.StartTime < ts.EndTime && ts.StartTime < request.EndTime)
                .ToList();

            if (overlappingTimeSlots.Any())
            {
                throw new ArgumentException("Time slot overlaps with existing time slots");
            }*/

            var newTimeSlotId = timeSlotRepository.AddTimeSlot(request);

            return newTimeSlotId;
        }

        public void DeleteTimeSlot(int timeSlotId)
        {
            if (timeSlotId <= 0)
            {
                throw new ArgumentException("Invalid TimeSlotId");
            }

            var timeSlot = timeSlotRepository.GetTimeSlotById(timeSlotId);
            if (timeSlot == null)
            {
                throw new NotFoundException("TimeSlot not found");
            }

            timeSlotRepository.DeleteTimeSlot(timeSlotId);
        }

        /*public List<Adventour.Api.Responses.TimeSlot.TimeSlot> GetTimeSlotsByDayId(int dayId)
        {
            if (dayId <= 0)
            {
                throw new ArgumentException("Invalid DayId");
            }

            var timeSlots = timeSlotRepository.GetTimeSlotsByDayId(dayId);
            if (timeSlots == null || !timeSlots.Any())
            {
                throw new NotFoundException("No time slots found for the given day");
            }

            return timeSlots;
        }*/
    }
}
