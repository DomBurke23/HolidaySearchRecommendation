using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TFP.HolidaySearchRecommendation.Application.UseCases.HolidaySuggestions.Requests;
using TFP.HolidaySearchRecommendation.Application.UseCases.HolidaySuggestions.Responses;
using TFP.HolidaySearchRecommendation.Common.UseCases;
using TFP.HolidaySearchRecommendation.Domain.Data.Repositories;
using TFP.HolidaySearchRecommendation.Domain.Models;

namespace TFP.HolidaySearchRecommendation.Application.UseCases.HolidaySuggestions
{
    public class GetHolidaySuggestionUseCase : IUseCase<GetHolidaySuggestionRequest, GetHolidaySuggestionResponse>
    {
        private readonly ILogger<IUseCase<GetHolidaySuggestionRequest, GetHolidaySuggestionResponse>> _logger;
        private readonly IHolidaySuggestionRepository _holidaySuggestionRepository;

        public GetHolidaySuggestionUseCase(
            ILogger<IUseCase<GetHolidaySuggestionRequest, GetHolidaySuggestionResponse>> logger,
            IHolidaySuggestionRepository holidaySuggestionRepository)
        {
            _logger = logger;
            _holidaySuggestionRepository = holidaySuggestionRepository;
        }

        public async Task<GetHolidaySuggestionResponse> HandleAsync(GetHolidaySuggestionRequest request)
        {
            HolidaySuggestion holidaySuggestion = await _holidaySuggestionRepository.FindAsync(request.Reference);

            return new GetHolidaySuggestionResponse()
            {
                HolidaySuggestion = holidaySuggestion
            };
        }
    }
}
