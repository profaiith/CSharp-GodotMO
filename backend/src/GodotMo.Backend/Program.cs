using GodotMo.Backend.Abstractions;
using GodotMo.Backend.Features.Sessions;
using GodotMo.Backend.Features.World;
using GodotMo.Backend.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Centralized feature registration. Add new IMmoFeature implementations to extend the backend.
var features = new IMmoFeature[]
{
    new SessionFeature(),
    new WorldFeature()
};

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();

foreach (var feature in features)
{
    feature.AddServices(builder.Services);
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/health", () => Results.Ok(new { status = "ok", utc = DateTimeOffset.UtcNow }));

foreach (var feature in features)
{
    feature.MapEndpoints(app);
}

app.Run();
