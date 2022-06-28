using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Rn.NetCore.DbCommon;
using Rn.NetCore.Metrics.Extensions;
using RnGo.Core.Extensions;

namespace DevConsole;

public static class DIContainer
{
  public static IServiceProvider Services { get; }

  static DIContainer()
  {
    var services = new ServiceCollection();

    var config = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json", true, true)
      .Build();

    services
      .AddSingleton<IConfiguration>(config)
      .AddRnMetricsBase(config)
      .AddRnDbMySql(config)
      .AddRnGo(config)

      .AddLogging(loggingBuilder =>
      {
        // configure Logging with NLog
        loggingBuilder.ClearProviders();
        loggingBuilder.SetMinimumLevel(LogLevel.Trace);
        loggingBuilder.AddNLog(config);
      });

    Services = services.BuildServiceProvider();
  }
}
