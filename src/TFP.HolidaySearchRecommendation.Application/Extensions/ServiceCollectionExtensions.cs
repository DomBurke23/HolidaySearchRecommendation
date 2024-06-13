using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TFP.HolidaySearchRecommendation.Application.Middleware;
using TFP.HolidaySearchRecommendation.Application.Services;
using TFP.HolidaySearchRecommendation.Application.Triggers.HolidaySuggestions.HttpRequests;
using TFP.HolidaySearchRecommendation.Application.Triggers.HolidaySuggestions.Validators;
using TFP.HolidaySearchRecommendation.Application.UseCases.HolidaySuggestions;
using TFP.HolidaySearchRecommendation.Application.UseCases.HolidaySuggestions.Requests;
using TFP.HolidaySearchRecommendation.Application.UseCases.HolidaySuggestions.Responses;
using TFP.HolidaySearchRecommendation.Application.UseCases.Operations;
using TFP.HolidaySearchRecommendation.Application.UseCases.Operations.Requests;
using TFP.HolidaySearchRecommendation.Application.UseCases.Operations.Responses;
using TFP.HolidaySearchRecommendation.Common.UseCases;

namespace TFP.HolidaySearchRecommendation.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IValidator<GetHolidaySuggestionHttpRequest>, GetHolidaySuggestionHttpRequestValidator>();
            services.AddTransient<IValidator<CreateHolidaySuggestionHttpRequest>, CreateHolidaySuggestionHttpRequestValidator>();
            services.AddTransient<IUseCase<GetHolidaySuggestionRequest, GetHolidaySuggestionResponse>, GetHolidaySuggestionUseCase>();
            services.AddTransient<IUseCase<CreateHolidaySuggestionRequest, CreateHolidaySuggestionResponse>, CreateHolidaySuggestionUseCase>();
            services.AddTransient<IUseCase<EnqueueCreateHolidaySuggestionRequest, EnqueueCreateHolidaySuggestionResponse>, EnqueueCreateHolidaySuggestionUseCase>();
            services.AddTransient<IOrchestrationService, OrchestrationService>();
            services.AddTransient<IUseCase<StartOperationRequest, StartOperationResponse>, StartOperationUseCase>();
            services.AddTransient<IUseCase<FinishOperationRequest, FinishOperationResponse>, FinishOperationUseCase>();

            services.AddTransient<IServiceBusMiddleware, ServiceBusMiddleware>();
        }
    }
}
