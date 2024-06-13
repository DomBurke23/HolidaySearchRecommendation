using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using TFP.HolidaySearchRecommendation.Application.Triggers.Models;
using TFP.HolidaySearchRecommendation.Application.UseCases.HolidaySuggestions.Requests;
using TFP.HolidaySearchRecommendation.Application.UseCases.HolidaySuggestions.Responses;
using TFP.HolidaySearchRecommendation.Application.UseCases.Operations.Requests;
using TFP.HolidaySearchRecommendation.Application.UseCases.Operations.Responses;
using TFP.HolidaySearchRecommendation.Common.Middleware;
using TFP.HolidaySearchRecommendation.Common.UseCases;
using TFP.HolidaySearchRecommendation.Domain.Messaging.Constants;
using TFP.HolidaySearchRecommendation.Domain.Messaging.Content;

namespace TFP.HolidaySearchRecommendation.Application.Triggers.HolidaySuggestions.Orchestrations
{
    public class CreateHolidaySuggestionOrchestrator
    {
        private readonly ITenantMiddleware _tenantMiddleware;
        private readonly IUseCase<StartOperationRequest, StartOperationResponse> _startOperationUseCase;
        private readonly IUseCase<CreateHolidaySuggestionRequest, CreateHolidaySuggestionResponse> _createHolidaySuggestionUseCase;
        private readonly IUseCase<FinishOperationRequest, FinishOperationResponse> _finishOperationUseCase;

        public CreateHolidaySuggestionOrchestrator(ITenantMiddleware tenantMiddleware,
            IUseCase<StartOperationRequest, StartOperationResponse> startOperationUseCase,
            IUseCase<CreateHolidaySuggestionRequest, CreateHolidaySuggestionResponse> createHolidaySuggestionUseCase,
            IUseCase<FinishOperationRequest, FinishOperationResponse> finishOperationUseCase)
        {
            _tenantMiddleware = tenantMiddleware;
            _startOperationUseCase = startOperationUseCase;
            _createHolidaySuggestionUseCase = createHolidaySuggestionUseCase;
            _finishOperationUseCase = finishOperationUseCase;
        }

        [FunctionName("CreateHolidaySuggestionOrchestrator")]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            OrchestrationInput input = context.GetInput<OrchestrationInput>();
            CreateHolidaySuggestionMessageContent createHolidaySuggestionMessageContent = input.Data.ToObject<CreateHolidaySuggestionMessageContent>();
            DateTime startedAt = context.CurrentUtcDateTime;

            var operationTasks = new Task[2];

            var startOperationActivityInput = new ActivityInput<StartOperationRequest>()
            {
                Tenant = input.Tenant,
                Request = new StartOperationRequest()
                {
                    Reference = context.InstanceId,
                    Name = SubjectConstants.CreateHolidaySuggestion,
                    Requester = createHolidaySuggestionMessageContent.OperationRequester,
                    StartedAt = startedAt
                }
            };
            operationTasks[0] = context.CallActivityAsync<string>("CreateHolidaySuggestionOrchestrator_StartOperation", startOperationActivityInput);

            var createHolidaySuggestionActivityInput = new ActivityInput<CreateHolidaySuggestionRequest>()
            {
                Tenant = input.Tenant,
                Request = new CreateHolidaySuggestionRequest()
                {
                    HolidaySuggestion = createHolidaySuggestionMessageContent.HolidaySuggestion
                }
            };
            ActivityOutput<CreateHolidaySuggestionResponse> createHolidaySuggestionActivityOutput = await context.CallActivityAsync<ActivityOutput<CreateHolidaySuggestionResponse>>("CreateHolidaySuggestionOrchestrator_CreateHolidaySuggestion", createHolidaySuggestionActivityInput);
            DateTime finishedAt = context.CurrentUtcDateTime;
            var finishOperationActivityInput = new ActivityInput<FinishOperationRequest>()
            {
                Tenant = input.Tenant,
                Request = new FinishOperationRequest()
                {
                    Reference = context.InstanceId,
                    Name = SubjectConstants.CreateHolidaySuggestion,
                    Requester = createHolidaySuggestionMessageContent.OperationRequester,
                    ResultCode = createHolidaySuggestionActivityOutput.ResultCode,
                    ResultMessage = createHolidaySuggestionActivityOutput.ResultMessage,
                    StartedAt = startedAt,
                    FinishedAt = finishedAt
                }
            };

            operationTasks[1] = context.CallActivityAsync<string>("CreateHolidaySuggestionOrchestrator_FinishOperation", finishOperationActivityInput);

            await Task.WhenAll(operationTasks);
        }

        [FunctionName("CreateHolidaySuggestionOrchestrator_StartOperation")]
        public async Task StartOperation([ActivityTrigger] ActivityInput<StartOperationRequest> input, ILogger log)
        {
            await _tenantMiddleware.InvokeAsync(input.Tenant);
            await _startOperationUseCase.HandleAsync(input.Request);
        }

        [FunctionName("CreateHolidaySuggestionOrchestrator_CreateHolidaySuggestion")]
        public async Task<ActivityOutput<CreateHolidaySuggestionResponse>> CreateHolidaySuggestion([ActivityTrigger] ActivityInput<CreateHolidaySuggestionRequest> input, ILogger log)
        {
            await _tenantMiddleware.InvokeAsync(input.Tenant);

            var activityOutput = new ActivityOutput<CreateHolidaySuggestionResponse>();

            try
            {
                CreateHolidaySuggestionResponse createHolidaySuggestionResponse = await _createHolidaySuggestionUseCase.HandleAsync(input.Request);
                activityOutput.ResultCode = 0;
                activityOutput.ResultMessage = "Success";
                activityOutput.Response = createHolidaySuggestionResponse;
            }
            catch (Exception e)
            {
                activityOutput.ResultCode = 1;
                activityOutput.ResultMessage = $"Failed - {e.GetType().Name} :: {e.Message}";
            }

            return activityOutput;
        }

        [FunctionName("CreateHolidaySuggestionOrchestrator_FinishOperation")]
        public async Task FinishOperation([ActivityTrigger] ActivityInput<FinishOperationRequest> input, ILogger log)
        {
            await _tenantMiddleware.InvokeAsync(input.Tenant);
            await _finishOperationUseCase.HandleAsync(input.Request);
        }
    }
}

