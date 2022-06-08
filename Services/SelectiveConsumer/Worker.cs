using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using SelectiveConsumer.Clients;
using SelectiveConsumer.Utilities;

namespace SelectiveConsumer;

public class Worker : BackgroundService
{
    public Worker(ILogger<Worker> logger, IOptions<SelectiveConsumerOptions> options, IServiceProvider serviceProvider, ServiceBusClient serviceBusClient)
    {
        _logger = logger;
        _options = options.Value;
        _serviceProvider = serviceProvider;
        _serviceBusClient = serviceBusClient;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) 
    { 
        return Task.CompletedTask;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        if (_serviceBusProcessor == null)
        {
            _serviceBusProcessor = _serviceBusClient.CreateProcessor(_options.Topic, _options.Subscription);
            _serviceBusProcessor.ProcessMessageAsync += MessageHandler;
            _serviceBusProcessor.ProcessErrorAsync += ErrorHandler;
        }

        await _serviceBusProcessor.StartProcessingAsync();
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_serviceBusProcessor != null)
        {
            await _serviceBusProcessor.StopProcessingAsync();
        }

        await base.StopAsync(cancellationToken);
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {      
        if (args.Message.Body != null)
        {
            var data = args.Message.Body.ToString();
            if (!string.IsNullOrEmpty(data))
            {
                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    var client = scope.ServiceProvider.GetRequiredService<IMicroserviceClient>();
                    await client.ProcessEvent(data);
                }
            }
        }

        await args.CompleteMessageAsync(args.Message);
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logger.LogInformation($"{args.Exception.ToString()}");
        return Task.CompletedTask;
    }    

    private readonly ILogger<Worker> _logger;
    private readonly SelectiveConsumerOptions _options;
    private readonly IServiceProvider _serviceProvider;
    private readonly ServiceBusClient _serviceBusClient;
    private ServiceBusProcessor? _serviceBusProcessor;
}
