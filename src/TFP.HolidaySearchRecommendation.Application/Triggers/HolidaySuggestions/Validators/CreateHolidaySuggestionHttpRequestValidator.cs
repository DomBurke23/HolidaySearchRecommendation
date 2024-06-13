using FluentValidation;
using TFP.HolidaySearchRecommendation.Application.Triggers.HolidaySuggestions.HttpRequests;

namespace TFP.HolidaySearchRecommendation.Application.Triggers.HolidaySuggestions.Validators
{
    public class CreateHolidaySuggestionHttpRequestValidator : AbstractValidator<CreateHolidaySuggestionHttpRequest>
    {
        public CreateHolidaySuggestionHttpRequestValidator()
        {
            RuleFor(x => x.Reference).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
