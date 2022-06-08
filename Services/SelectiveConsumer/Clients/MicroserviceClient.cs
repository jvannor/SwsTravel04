using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SelectiveConsumer.Models;
using SelectiveConsumer.Utilities;

namespace SelectiveConsumer.Clients;

public class MicroserviceClient : IMicroserviceClient
{
    public MicroserviceClient(ILogger<MicroserviceClient> logger, IOptions<SelectiveConsumerOptions> options, HttpClient client)
    {
        _logger = logger;
        _options = options.Value;
        _client = client;
    }

    public async Task ProcessEvent(string data)
    {   
        var serviceData = new ServiceData();
        serviceData.Elements.Add($"Selective Consumer processed data at {DateTime.UtcNow}; Label={_options.Label};");
        serviceData.Elements.Add(data);

        var json = JsonSerializer.Serialize(serviceData);

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(_options.MicroserviceUri, content);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"Unexpected HTTP Status Code, {response.StatusCode}; request uri={_options.MicroserviceUri};");
            throw new ApplicationException($"Unexpected HTTP Status Code, {response.StatusCode}; request uri={_options.MicroserviceUri};");
        }
    }

    private readonly ILogger<MicroserviceClient> _logger;
    private readonly SelectiveConsumerOptions _options;
    private readonly HttpClient _client;
}