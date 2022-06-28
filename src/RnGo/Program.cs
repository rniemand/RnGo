using NLog.Extensions.Logging;
using Rn.NetCore.DbCommon;
using Rn.NetCore.Metrics.Extensions;
using RnGo.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddNLog();

builder.Services
  .AddRnMetricsBase(builder.Configuration)
  .AddRnDbMySql(builder.Configuration)
  .AddRnGo(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.MapControllers();

app.Run();
