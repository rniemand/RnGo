﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Helpers;
using Rn.NetCore.Common.Logging;
using RnGo.Core.Helpers;
using RnGo.Core.Models;
using RnGo.Core.Providers;
using RnGo.Core.Services;

namespace DevConsole
{
  public class RnGoDevelopment
  {
    private readonly IServiceProvider _services;

    public RnGoDevelopment()
    {
      _services = BuildServiceContainer();
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
        .GetRequiredService<ILinkService>()
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

    public RnGoDevelopment StoreLink()
    {
      var linkService = _services.GetRequiredService<ILinkService>();
      var link = new RnGoLink
      {
        Url = "https://www.google.com/?q=3"
      };

      var result = linkService
        .StoreLink(link)
        .GetAwaiter()
        .GetResult();

      Console.WriteLine($"Stored as '{result}'");

      return this;
    }

    public RnGoDevelopment ResolveLink(string shortCode)
    {
      var jsonHelper = _services.GetRequiredService<IJsonHelper>();

      var resolvedLink = _services
        .GetRequiredService<ILinkService>()
        .Resolve(shortCode)
        .GetAwaiter()
        .GetResult();

      Console.WriteLine($"Resolved as: {jsonHelper.SerializeObject(resolvedLink)}");

      return this;
    }

    public RnGoDevelopment GetLinkCount()
    {
      var urlCount = _services
        .GetRequiredService<ILinkService>()
        .GetLinkCount()
        .GetAwaiter()
        .GetResult();

      Console.WriteLine("URL Count: {0}", urlCount);

      return this;
    }

    private static IServiceProvider BuildServiceContainer()
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
        .AddSingleton<ILinkService, LinkService>()
        .AddSingleton<ILinkStorageService, LinkStorageService>()
        .AddSingleton<ILinkStatsService, LinkStatsService>()

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
        .AddSingleton<IRnGoConfigProvider, RnGoConfigProvider>();

      return services.BuildServiceProvider();
    }
  }
}
