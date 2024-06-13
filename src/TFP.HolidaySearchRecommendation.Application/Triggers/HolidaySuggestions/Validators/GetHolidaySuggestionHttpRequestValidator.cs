using FluentValidation;
using TFP.HolidaySearchRecommendation.Application.Triggers.HolidaySuggestions.HttpRequests;

namespace TFP.HolidaySearchRecommendation.Application.Triggers.HolidaySuggestions.Validators
{
    public class GetHolidaySuggestionHttpRequestValidator : AbstractValidator<GetHolidaySuggestionHttpRequest>
    {
        public GetHolidaySuggestionHttpRequestValidator()
        {
            RuleFor(x => x.Reference).NotEmpty();
        }
    }
}
