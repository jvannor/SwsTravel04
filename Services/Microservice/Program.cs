using Microsoft.EntityFrameworkCore;
using Microservice.Data;
using Microservice.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<JournalDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("JournalDb"),
        providerOptions => providerOptions.EnableRetryOnFailure()));

builder.Services.Configure<MicroserviceOptions>(
    builder.Configuration.GetSection("Microservice"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
