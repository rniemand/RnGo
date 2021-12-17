using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Rn.NetCore.Common.Logging;

namespace DevConsole
{
  public class RnGoDevelopment
  {
    private static IServiceProvider _services;

    public RnGoDevelopment()
    {
      ConfigureDI();
    }

    public RnGoDevelopment DoNothing()
    {
      return this;
    }

    public RnGoDevelopment HelloWorld()
    {
      _services
        .GetRequiredService<ILoggerAdapter<RnGoDevelopment>>()
        .Info("Hello World");

      return this;
    }

    private static void ConfigureDI()
    {
      var services = new ServiceCollection();

      var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true)
        .Build();

      services
        // Configuration
        .AddSingleton<IConfiguration>(config)

        // Logging
        .AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>))
        .AddLogging(loggingBuilder =>
        {
          // configure Logging with NLog
          loggingBuilder.ClearProviders();
          loggingBuilder.SetMinimumLevel(LogLevel.Trace);
          loggingBuilder.AddNLog(config);
        });

      _services = services.BuildServiceProvider();
    }
  }
}
