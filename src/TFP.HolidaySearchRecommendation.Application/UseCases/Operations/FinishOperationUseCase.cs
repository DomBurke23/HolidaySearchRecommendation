using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TFP.HolidaySearchRecommendation.Application.UseCases.Operations.Requests;
using TFP.HolidaySearchRecommendation.Application.UseCases.Operations.Responses;
using TFP.HolidaySearchRecommendation.Common.UseCases;
using TFP.HolidaySearchRecommendation.Domain.Messaging.Constants;
using TFP.HolidaySearchRecommendation.Domain.Messaging.Models;
using TFP.HolidaySearchRecommendation.Domain.Services;
using TFP.HolidaySearchRecommendation.Infrastructure.Options;

namespace TFP.HolidaySearchRecommendation.Application.UseCases.Operations
{
    public class FinishOperationUseCase : IUseCase<FinishOperationRequest, FinishOperationResponse>
    {
        private readonly ILogger<IUseCase<FinishOperationRequest, FinishOperationResponse>> _logger;
        private readonly IMessageService _messageService;
        private readonly IOptions<ServiceBusOptions> _serviceBusOptions;

        public FinishOperationUseCase(
            ILogger<IUseCase<FinishOperationRequest, FinishOperationResponse>> logger,
            IMessageService messageService,
            IOptions<ServiceBusOptions> serviceBusOptions
            )
        {
            _logger = logger;
            _messageService = messageService;
            _serviceBusOptions = serviceBusOptions;
        }

        public async Task<FinishOperationResponse> HandleAsync(FinishOperationRequest request)
        {
            var message = new Message()
            {
                Id = Guid.NewGuid().ToString(),
                Subject = SubjectConstants.FinishOperation,
                Data = request
            };

            await _messageService.SendAsync(message, _serviceBusOptions.Value.OperationsTopic);

            return new FinishOperationResponse();
        }
    }
}
