using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TFP.HolidaySearchRecommendation.Application.Exceptions;
using TFP.HolidaySearchRecommendation.Application.Middleware;
using TFP.HolidaySearchRecommendation.Application.Services;
using TFP.HolidaySearchRecommendation.Application.Triggers.HolidaySuggestions.HttpRequests;
using TFP.HolidaySearchRecommendation.Application.Triggers.HolidaySuggestions.HttpResponses;
using TFP.HolidaySearchRecommendation.Application.UseCases.HolidaySuggestions.Requests;
using TFP.HolidaySearchRecommendation.Application.UseCases.HolidaySuggestions.Responses;
using TFP.HolidaySearchRecommendation.Common.Constants;
using TFP.HolidaySearchRecommendation.Common.Exceptions;
using TFP.HolidaySearchRecommendation.Common.Middleware;
using TFP.HolidaySearchRecommendation.Common.Services;
using TFP.HolidaySearchRecommendation.Common.UseCases;
using TFP.HolidaySearchRecommendation.Domain.Messaging.Constants;
using TFP.HolidaySearchRecommendation.Domain.Models;

namespace TFP.HolidaySearchRecommendation.Application.Triggers.HolidaySuggestions
{
    public class HolidaySuggestionTrigger
    {
        private readonly IServiceBusMiddleware _serviceBusMiddleware;
        private readonly ITenantMiddleware _tenantMiddleware;
        private readonly ITenantContextAccessor _tenantContextAccessor;
        private readonly IUserContextAccessor _userContextAccessor;
        private readonly IAuthorizationMiddleware _authorizationMiddleware;
        private readonly IFeatureMiddleware _featureMiddleware;
        private readonly IOrchestrationService _orchestrationService;
        private readonly IValidator<GetHolidaySuggestionHttpRequest> _getHolidaySuggestionHttpRequestValidator;
        private readonly IUseCase<GetHolidaySuggestionRequest, GetHolidaySuggestionResponse> _getHolidaySuggestionUseCase;
        private readonly IValidator<CreateHolidaySuggestionHttpRequest> _createHolidaySuggestionHttpRequestValidator;
        private readonly IUseCase<CreateHolidaySuggestionRequest, CreateHolidaySuggestionResponse> _createHolidaySuggestionUseCase;
        private readonly IUseCase<EnqueueCreateHolidaySuggestionRequest, EnqueueCreateHolidaySuggestionResponse> _enqueueCreateHolidaySuggestionUseCase;

        public HolidaySuggestionTrigger(IServiceBusMiddleware serviceBusMiddleware,
            ITenantMiddleware tenantMiddleware,
            ITenantContextAccessor tenantContextAccessor,
            IUserContextAccessor userContextAccessor,
            IAuthorizationMiddleware authorizationMiddleware,
            IFeatureMiddleware featureMiddleware,
            IOrchestrationService orchestrationService,
            IValidator<GetHolidaySuggestionHttpRequest> getHolidaySuggestionHttpRequestValidator,
            IUseCase<GetHolidaySuggestionRequest, GetHolidaySuggestionResponse> getHolidaySuggestionUseCase,
            IValidator<CreateHolidaySuggestionHttpRequest> createHolidaySuggestionHttpRequestValidator,
            IUseCase<CreateHolidaySuggestionRequest, CreateHolidaySuggestionResponse> createHolidaySuggestionUseCase,
            IUseCase<EnqueueCreateHolidaySuggestionRequest, EnqueueCreateHolidaySuggestionResponse> enqueueCreateHolidaySuggestionUseCase
            )
        {
            _serviceBusMiddleware = serviceBusMiddleware;
            _tenantMiddleware = tenantMiddleware;
            _tenantContextAccessor = tenantContextAccessor;
            _userContextAccessor = userContextAccessor;
            _authorizationMiddleware = authorizationMiddleware;
            _featureMiddleware = featureMiddleware;
            _orchestrationService = orchestrationService;
            _getHolidaySuggestionHttpRequestValidator = getHolidaySuggestionHttpRequestValidator;
            _getHolidaySuggestionUseCase = getHolidaySuggestionUseCase;
            _createHolidaySuggestionHttpRequestValidator = createHolidaySuggestionHttpRequestValidator;
            _createHolidaySuggestionUseCase = createHolidaySuggestionUseCase;
            _enqueueCreateHolidaySuggestionUseCase = enqueueCreateHolidaySuggestionUseCase;
        }

        [FunctionName("GetHolidaySuggestion")]
        [OpenApiOperation(operationId: "HolidaySuggestion.Get", tags: new[] { "HolidaySuggestion" }, Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: HttpRequestHeadersConstants.XTenant, Description = "Tenant", Type = typeof(string), Required = true, In = ParameterLocation.Header)]
        [OpenApiParameter(name: "reference", Description = "HolidaySuggestion Reference", Type = typeof(string), Required = true, In = ParameterLocation.Query)]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(Domain.Models.HolidaySuggestion), Description = "Returns the HolidaySuggestion")]
        [OpenApiSecurity("Bearer", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT", Description = "JWT Authorization header using the Bearer scheme.")]
        public async Task<IActionResult> GetHolidaySuggestionAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "HolidaySuggestion")] HttpRequest req,
            ILogger log)
        {
            // Set Tenant Context
            try
            {
                await _tenantMiddleware.InvokeAsync(req.HttpContext);
            }
            catch (TenantNotFoundException tnfe)
            {
                log.LogError(tnfe, "Tenant not found");
                return new StatusCodeResult((int)HttpStatusCode.Forbidden);
            }

            // Validate User
            try
            {
                await _authorizationMiddleware.InvokeAsync(req);
            }
            catch (UnauthorizedAccessException uae)
            {
                return new UnauthorizedResult();
            }

            // Validate Request
            var getHolidaySuggestionHttpRequest = new GetHolidaySuggestionHttpRequest()
            {
                Reference = req.Query["reference"]
            };

            ValidationResult validationResult = _getHolidaySuggestionHttpRequestValidator.Validate(getHolidaySuggestionHttpRequest);
            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                }));
            }

            var getHolidaySuggestionRequest = new GetHolidaySuggestionRequest()
            {
                Reference = getHolidaySuggestionHttpRequest.Reference
            };
            GetHolidaySuggestionResponse getHolidaySuggestionResponse = await _getHolidaySuggestionUseCase.HandleAsync(getHolidaySuggestionRequest);
            HolidaySuggestion holidaySuggestion = getHolidaySuggestionResponse.HolidaySuggestion;

            if (holidaySuggestion == null)
            {
                return new NotFoundResult();
            }

            var getHolidaySuggestionHttpResponse = new GetHolidaySuggestionHttpResponse()
            {
                HolidaySuggestion = holidaySuggestion
            };

            return new OkObjectResult(getHolidaySuggestionHttpResponse);
        }

        [FunctionName("EnqueueCreateHolidaySuggestion")]
        [OpenApiOperation(operationId: "HolidaySuggestion.Create", tags: new[] { "HolidaySuggestion" }, Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: HttpRequestHeadersConstants.XTenant, Description = "Tenant", Type = typeof(string), Required = true, In = ParameterLocation.Header)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Domain.Models.HolidaySuggestion), Description = "Enqueue Create HolidaySuggestion", Required = true)]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(Domain.Models.HolidaySuggestion))]
        [OpenApiSecurity("Bearer", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT", Description = "JWT Authorization header using the Bearer scheme.")]
        public async Task<IActionResult> EnqueueCreateHolidaySuggestionAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "HolidaySuggestion")] HttpRequest req,
            ILogger log)
        {
            // Set Tenant Context
            try
            {
                await _tenantMiddleware.InvokeAsync(req.HttpContext);
            }
            catch (TenantNotFoundException tnfe)
            {
                log.LogError(tnfe, "Tenant not found");
                return new StatusCodeResult((int)HttpStatusCode.Forbidden);
            }

            // Validate User
            try
            {
                await _authorizationMiddleware.InvokeAsync(req);
            }
            catch (UnauthorizedAccessException uae)
            {
                return new UnauthorizedResult();
            }

            // Parse body
            var payload = await req.ReadAsStringAsync();
            CreateHolidaySuggestionHttpRequest createHolidaySuggestionHttpRequest = JsonConvert.DeserializeObject<CreateHolidaySuggestionHttpRequest>(payload);

            // Validate Request
            ValidationResult validationResult = _createHolidaySuggestionHttpRequestValidator.Validate(createHolidaySuggestionHttpRequest);
            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                }));
            }

            var holidaySuggestion = new HolidaySuggestion()
            {
                Reference = createHolidaySuggestionHttpRequest.Reference,
                Name = createHolidaySuggestionHttpRequest.Name
            };

            var operationReference = Guid.NewGuid().ToString();
            var operationRequester = _userContextAccessor.UserContext.User.UserReference;
            var enqueueCreateHolidaySuggestionRequest = new EnqueueCreateHolidaySuggestionRequest()
            {
                OperationReference = operationReference,
                OperationRequester = operationRequester,
                HolidaySuggestion = holidaySuggestion
            };
            EnqueueCreateHolidaySuggestionResponse enqueueCreateHolidaySuggestionResponse = await _enqueueCreateHolidaySuggestionUseCase.HandleAsync(enqueueCreateHolidaySuggestionRequest);

            var createHolidaySuggestionHttpResponse = new CreateHolidaySuggestionHttpResponse()
            {
                OperationReference = operationReference
            };

            return new ObjectResult(createHolidaySuggestionHttpResponse) { StatusCode = (int)HttpStatusCode.Accepted };
        }

        [FunctionName("CreateHolidaySuggestionOrchestrator_ServiceBusStart")]
        public async Task CreateHolidaySuggestionAsync(
             [ServiceBusTrigger("%AzureServiceBus:Topics:Application%", "%AzureServiceBus:Topics:Application:Subscriptions:CreateHolidaySuggestion%", Connection = "AzureWebJobsServiceBus")] JObject content, string messageId, string subject, IDictionary<string, object> applicationProperties,
             [DurableClient] IDurableOrchestrationClient orchestrationClient, ILogger log)
        {
            log.LogInformation($"Processing message {messageId}/{subject}");

            if (subject == SubjectConstants.CreateHolidaySuggestion)
            {
                try
                {
                    await _tenantMiddleware.InvokeAsync(applicationProperties);

                    var instanceId = (string)content["OperationReference"];
                    await _orchestrationService.StartNewAsync(orchestrationClient, "CreateHolidaySuggestionOrchestrator", instanceId, content);
                    log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
                }
                catch (BadMessageException bme)
                {
                    log.LogError(bme, $"Error ocurred parsing message {messageId}");
                }
            }
            else
            {
                log.LogError($"Unsupported subject received {subject} - expecting {SubjectConstants.CreateHolidaySuggestion}");
            }
        }
    }
}
