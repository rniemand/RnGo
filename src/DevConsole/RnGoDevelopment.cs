using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Rn.NetCore.Common.Logging;
using RnGo.Core.Helpers;
using RnGo.Core.Services;

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

    public RnGoDevelopment ResolveLink()
    {
      _services
        .GetRequiredService<ILinkResolverService>()
        .Resolve("a");

      return this;
    }

    public RnGoDevelopment Base64Encode()
    {
      var helper = _services.GetRequiredService<IStringHelper>();
      var encoded = helper.Base64Encode("hello");
      Console.WriteLine("Encoded: " + encoded);
      var decoded = helper.Base64Decode(encoded);
      Console.WriteLine("Decoded: " + decoded);
      return this;
    }

    public RnGoDevelopment GenerateLinkString(long input)
    {
      var linkString = _services
        .GetRequiredService<IStringHelper>()
        .GenerateLinkString(input);

      Console.WriteLine($"Generated '{linkString}' from '{input}'");
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
        })
        
        // Services
        .AddSingleton<ILinkResolverService, LinkResolverService>()
        
        // Helpers
        .AddSingleton<IStringHelper, StringHelper>();

      _services = services.BuildServiceProvider();
    }
  }
}
