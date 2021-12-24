using NLog.Extensions.Logging;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Helpers;
using Rn.NetCore.Common.Logging;
using Rn.NetCore.DbCommon;
using Rn.NetCore.DbCommon.Helpers;
using Rn.NetCore.DbCommon.Interfaces;
using Rn.NetCore.Metrics;
using RnGo.Core.Helpers;
using RnGo.Core.Providers;
using RnGo.Core.RepoQueries;
using RnGo.Core.Repositories;
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
  .AddSingleton<ILinkStatsService, LinkStatsService>()
  .AddSingleton<IApiKeyService, ApiKeyService>()

  // Helpers
  .AddSingleton<IStringHelper, StringHelper>()
  .AddSingleton<IJsonHelper, JsonHelper>()

  // Abstractions
  .AddSingleton<IFileAbstraction, FileAbstraction>()
  .AddSingleton<IDirectoryAbstraction, DirectoryAbstraction>()
  .AddSingleton<IEnvironmentAbstraction, EnvironmentAbstraction>()
  .AddSingleton<IPathAbstraction, PathAbstraction>()
  .AddSingleton<IDateTimeAbstraction, DateTimeAbstraction>()

  // Providers
  .AddSingleton<IRnGoConfigProvider, RnGoConfigProvider>()
  .AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>))

  // Metrics
  .AddSingleton<IMetricServiceUtils, MetricServiceUtils>()
  .AddSingleton<IMetricService, MetricService>()

  // Database
  .AddSingleton<IConnectionResolver>(new ConnectionResolver(builder.Configuration, "RnGo"))
  .AddSingleton<IDbConnectionHelper, MySqlConnectionHelper>()
  .AddSingleton<ILinkRepo, LinkRepo>()
  .AddSingleton<ILinkRepoQueries, LinkRepoQueries>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.MapControllers();

app.Run();
