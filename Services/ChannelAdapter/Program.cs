using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using ChannelAdapter;
using ChannelAdapter.Data;
using ChannelAdapter.Utilities;

var builder = Host.CreateDefaultBuilder(args);

// Add services to the container

builder.ConfigureServices((context, services) =>
{
    services.Configure<ChannelAdapterOptions>(
        context.Configuration.GetSection("ChannelAdapter"));
        
    services.AddDbContext<EventDbContext>(options =>
        options.UseSqlServer(
            context.Configuration.GetConnectionString("EventDb"),
            providerOptions => providerOptions.EnableRetryOnFailure()));
        
    services.AddAzureClients(acb => 
    {
        acb.ConfigureDefaults(context.Configuration.GetSection("AzureDefaults"));
        acb.UseCredential(new DefaultAzureCredential(true));
        acb.AddServiceBusClient(context.Configuration.GetSection("AzureServiceBus"));
    });

    services.AddHostedService<Worker>();
});

var app = builder.Build();
await app.RunAsync();
