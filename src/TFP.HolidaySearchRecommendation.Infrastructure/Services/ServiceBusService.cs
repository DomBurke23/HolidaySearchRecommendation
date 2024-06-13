using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TFP.HolidaySearchRecommendation.Common.Constants;
using TFP.HolidaySearchRecommendation.Common.Services;
using TFP.HolidaySearchRecommendation.Domain.Messaging.Models;
using TFP.HolidaySearchRecommendation.Domain.Services;
using TFP.HolidaySearchRecommendation.Infrastructure.Options;

namespace TFP.HolidaySearchRecommendation.Infrastructure.Services
{
    public class ServiceBusService : IMessageService
    {
        private readonly ITenantContextAccessor _tenantContextAccessor;
        private readonly ServiceBusOptions _serviceBusOptions;

        public ServiceBusService(IOptions<ServiceBusOptions> serviceBusOptions, ITenantContextAccessor tenantContextAccessor)
        {
            _tenantContextAccessor = tenantContextAccessor;
            _serviceBusOptions = serviceBusOptions.Value;
        }

        public async Task SendAsync(Message message, string topic)
        {
            var data = JObject.FromObject(message.Data);
            await using var serviceBusClient = new ServiceBusClient(_serviceBusOptions.ConnectionString);
            ServiceBusSender sender = serviceBusClient.CreateSender(topic);
            var messageContent = JsonConvert.SerializeObject(data);
            var serviceBusMessage = new ServiceBusMessage(messageContent)
            {
                MessageId = message.Id,
                Subject = message.Subject
            };
            serviceBusMessage.ApplicationProperties.Add(CustomHeaderConstants.Tenant, _tenantContextAccessor.TenantContext.TenantOptions.Name);
            await sender.SendMessageAsync(serviceBusMessage);
        }

        public async Task SendScheduledAsync(Message message, string topic, DateTime scheduledDateTime)
        {
            var data = JObject.FromObject(message.Data);
            await using var serviceBusClient = new ServiceBusClient(_serviceBusOptions.ConnectionString);
            ServiceBusSender sender = serviceBusClient.CreateSender(topic);
            var messageContent = JsonConvert.SerializeObject(data);
            var serviceBusMessage = new ServiceBusMessage(messageContent)
            {
                MessageId = message.Id,
                Subject = message.Subject
            };
            serviceBusMessage.ApplicationProperties.Add(CustomHeaderConstants.Tenant, _tenantContextAccessor.TenantContext.TenantOptions.Name);
            await sender.ScheduleMessageAsync(serviceBusMessage, scheduledDateTime);
        }
    }
}
