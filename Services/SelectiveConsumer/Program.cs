using Azure.Identity;
using Microsoft.Extensions.Azure;
using SelectiveConsumer;
using SelectiveConsumer.Clients;
using SelectiveConsumer.Utilities;

var builder = Host.CreateDefaultBuilder(args);

// Add and configure container services

builder.ConfigureServices((context, services) => 
{
    services.Configure<SelectiveConsumerOptions>(
        context.Configuration.GetSection("SelectiveConsumer"));
            
    services.AddAzureClients(acb =>
    {
        acb.ConfigureDefaults(context.Configuration.GetSection("AzureDefaults"));
        acb.UseCredential(new DefaultAzureCredential(true));
        acb.AddServiceBusClient(context.Configuration.GetSection("AzureServiceBus"));
    });

    services.AddHttpClient<IMicroserviceClient, MicroserviceClient>()
        .AddPolicyHandler(Support.GetRetryPolicy());
    
    services.AddHostedService<Worker>();
});

// Build and run the application

var app = builder.Build();
await app.RunAsync();