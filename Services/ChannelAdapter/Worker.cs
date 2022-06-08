using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using System.Text.Json;
using ChannelAdapter.Data;
using ChannelAdapter.Models;
using ChannelAdapter.Utilities;

namespace ChannelAdapter;

public class Worker : BackgroundService
{
    public Worker(ILogger<Worker> logger, IOptions<ChannelAdapterOptions> options, IServiceScopeFactory serviceScopeFactory, ServiceBusClient serviceBusClient)
    {
        _logger = logger;
        _options = options.Value;
        _serviceScopeFactory = serviceScopeFactory;
        _serviceBusSender = serviceBusClient.CreateSender("PubSubChannel");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<EventDbContext>();
                var messageBatch = await _serviceBusSender.CreateMessageBatchAsync(stoppingToken);
                var events = context.Events.Where(e => e.Completed == null);

                foreach(var e in events)
                {
                    _logger.LogInformation($"Worker processing event {e.EventId} with code {e.EventCode}");

                    var xform = new { EventId = e.EventId, EventCode = e.EventCode, Timestamp = e.Created };
                    var message = new ServiceBusMessage(JsonSerializer.Serialize(xform));
                    message.ApplicationProperties.Add("Source", "Source");
                    message.ApplicationProperties.Add("Destination", "Destination");
                    message.ApplicationProperties.Add("Topic", e.EventCode);
                    message.ScheduledEnqueueTime = new DateTime(e.Scheduled.Ticks, DateTimeKind.Utc);

                    if (!messageBatch.TryAddMessage(message))
                    {
                        throw new ApplicationException("failed to process batch; too many messages");
                    }

                    e.Completed = DateTime.UtcNow;
                }    

                if (messageBatch.Count > 0)
                {                
                    await context.SaveChangesAsync(stoppingToken);
                    await _serviceBusSender.SendMessagesAsync(messageBatch, stoppingToken);  
                }              
            }

            await Task.Delay(_options.Interval, stoppingToken);
        }
    }
    private readonly ILogger<Worker> _logger;
    private readonly ChannelAdapterOptions _options;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ServiceBusSender _serviceBusSender;
}
