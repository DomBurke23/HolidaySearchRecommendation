using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TFP.HolidaySearchRecommendation.Application.UseCases.HolidaySuggestions.Requests;
using TFP.HolidaySearchRecommendation.Application.UseCases.HolidaySuggestions.Responses;
using TFP.HolidaySearchRecommendation.Common.UseCases;
using TFP.HolidaySearchRecommendation.Domain.Messaging.Constants;
using TFP.HolidaySearchRecommendation.Domain.Messaging.Content;
using TFP.HolidaySearchRecommendation.Domain.Messaging.Models;
using TFP.HolidaySearchRecommendation.Domain.Services;
using TFP.HolidaySearchRecommendation.Infrastructure.Options;

namespace TFP.HolidaySearchRecommendation.Application.UseCases.HolidaySuggestions
{
    public class EnqueueCreateHolidaySuggestionUseCase : IUseCase<EnqueueCreateHolidaySuggestionRequest, EnqueueCreateHolidaySuggestionResponse>
    {
        private readonly ILogger<IUseCase<EnqueueCreateHolidaySuggestionRequest, EnqueueCreateHolidaySuggestionResponse>> _logger;
        private readonly IMessageService _messageService;
        private readonly IOptions<ServiceBusOptions> _serviceBusOptions;

        public EnqueueCreateHolidaySuggestionUseCase(
            ILogger<IUseCase<EnqueueCreateHolidaySuggestionRequest, EnqueueCreateHolidaySuggestionResponse>> logger,
            IMessageService messageService,
            IOptions<ServiceBusOptions> serviceBusOptions)
        {
            _logger = logger;
            _messageService = messageService;
            _serviceBusOptions = serviceBusOptions;
        }

        public async Task<EnqueueCreateHolidaySuggestionResponse> HandleAsync(EnqueueCreateHolidaySuggestionRequest request)
        {
            var messageContent = new CreateHolidaySuggestionMessageContent()
            {
                OperationReference = request.OperationReference,
                OperationRequester = request.OperationRequester,
                HolidaySuggestion = request.HolidaySuggestion
            };

            var message = new Message()
            {
                Id = Guid.NewGuid().ToString(),
                Subject = SubjectConstants.CreateHolidaySuggestion,
                Data = messageContent
            };

            await _messageService.SendAsync(message, _serviceBusOptions.Value.ApplicationTopic);

            return new EnqueueCreateHolidaySuggestionResponse();
        }
    }
}
