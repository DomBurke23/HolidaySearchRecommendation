using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TFP.HolidaySearchRecommendation.Application.UseCases.HolidaySuggestions.Requests;
using TFP.HolidaySearchRecommendation.Application.UseCases.HolidaySuggestions.Responses;
using TFP.HolidaySearchRecommendation.Common.UseCases;
using TFP.HolidaySearchRecommendation.Domain.Data.Repositories;

namespace TFP.HolidaySearchRecommendation.Application.UseCases.HolidaySuggestions
{
    public class CreateHolidaySuggestionUseCase : IUseCase<CreateHolidaySuggestionRequest, CreateHolidaySuggestionResponse>
    {
        private readonly ILogger<IUseCase<CreateHolidaySuggestionRequest, CreateHolidaySuggestionResponse>> _logger;
        private readonly IHolidaySuggestionRepository _holidaySuggestionRepository;

        public CreateHolidaySuggestionUseCase(
            ILogger<IUseCase<CreateHolidaySuggestionRequest, CreateHolidaySuggestionResponse>> logger,
            IHolidaySuggestionRepository holidaySuggestionRepository)
        {
            _logger = logger;
            _holidaySuggestionRepository = holidaySuggestionRepository;
        }

        public async Task<CreateHolidaySuggestionResponse> HandleAsync(CreateHolidaySuggestionRequest request)
        {
            await _holidaySuggestionRepository.SaveAsync(request.HolidaySuggestion);

            return new CreateHolidaySuggestionResponse();
        }
    }
}
