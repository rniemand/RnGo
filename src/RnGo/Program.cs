using NLog.Extensions.Logging;
using RnGo.Core.Helpers;
using RnGo.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddNLog();

builder.Services
  // Services
  .AddSingleton<ILinkService, LinkService>()
  .AddSingleton<ILinkStorageService, LinkStorageService>()

  // Helpers
  .AddSingleton<IStringHelper, StringHelper>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.MapControllers();

app.Run();
