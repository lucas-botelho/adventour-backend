using Adventour.Api.Repositories;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Requests.Day;
using SendGrid.Helpers.Errors.Model;

namespace Adventour.Api.Services.Day
{
    public class DayService : IDayService
    {
        private readonly IDayRepository dayRepository;
        private readonly IItineraryRepository itineraryRepository;

        public DayService(IDayRepository dayRepository, IItineraryRepository itineraryRepository)
        {
            this.dayRepository = dayRepository;
            this.itineraryRepository = itineraryRepository;
        }

        public int AddDay(AddDayRequest request)
        {
            var itinerary = itineraryRepository.GetItineraryById(request.ItineraryId);

            if (itinerary == null)
            {
                throw new NotFoundException("The Itinerary doesn't exist");
            }

            return dayRepository.AddDay(request);
        }

        public void DeleteDay(int dayId)
        {
            var day = dayRepository.GetDayById(dayId);
            if (day == null)
            {
                throw new NotFoundException("Day not found");
            }

            bool success = dayRepository.DeleteDay(dayId);
            if (!success)
            {
                throw new ArgumentException("Failed to delete day");
            }
        }
    }
}


