using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microservice.Data;
using Microservice.Models;
using Microservice.Utilities;

namespace Microservice.Controllers;

[ApiController]
[Route("[controller]")]
public class JournalController : ControllerBase
{
    public JournalController(ILogger<JournalController> logger, IOptions<MicroserviceOptions> options, JournalDbContext journalDbContext)
    {
        _logger = logger;
        _options = options.Value;
        _journalDbContext = journalDbContext;
    }  

    [HttpPost(Name = "CreateJournalEntry")]   
    public async Task Post(ServiceData? data)
    {
        ServiceData workingData = data != null ? data : new ServiceData();
        workingData.Elements.Add($"Microservice processed data at {DateTime.UtcNow}; Label={_options.Label};");

        var json = JsonSerializer.Serialize(workingData);
        var entry = new Journal() { JournalEntry = json };

        await _journalDbContext.AddAsync(entry);
        await _journalDbContext.SaveChangesAsync();
    }

    private readonly ILogger<JournalController> _logger;

    private readonly MicroserviceOptions _options;

    private readonly JournalDbContext _journalDbContext;
}