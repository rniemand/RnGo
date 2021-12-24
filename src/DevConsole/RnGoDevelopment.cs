using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Helpers;
using Rn.NetCore.Common.Logging;
using Rn.NetCore.DbCommon;
using Rn.NetCore.DbCommon.Helpers;
using Rn.NetCore.DbCommon.Interfaces;
using Rn.NetCore.Metrics;
using RnGo.Core.Entities;
using RnGo.Core.Helpers;
using RnGo.Core.Models;
using RnGo.Core.Providers;
using RnGo.Core.RepoQueries;
using RnGo.Core.Repositories;
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

    public RnGoDevelopment AddLink(string url, string? apiKey = null)
    {
      var linkService = _services.GetRequiredService<ILinkService>();

      var response = linkService
        .AddLink(new AddLinkRequest
        {
          Url = url,
          ApiKey = apiKey ?? "18A8B66F-B4F1-4814-8771-D1EABD9CFB43"
        })
        .GetAwaiter()
        .GetResult();

      Console.WriteLine($"Stored as '{response.ShortCode}'");

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

    public RnGoDevelopment AddDatabaseLink()
    {
      var repo = _services.GetRequiredService<ILinkRepo>();

      repo
        .AddLink(new LinkEntity
        {
          Url = "https://docs.google.com/spreadsheets",
          ShortCode = "2"
        })
        .GetAwaiter()
        .GetResult();


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

    public RnGoDevelopment StoreApiKey(string apiKey)
    {
      var apiKeyRepo = _services.GetRequiredService<IApiKeyRepo>();
      var apiKeyEntity = apiKeyRepo.GetByApiKey(apiKey).GetAwaiter().GetResult();
      if (apiKeyEntity is null)
      {
        apiKeyRepo.Add(apiKey).GetAwaiter().GetResult();
        apiKeyEntity = apiKeyRepo.GetByApiKey(apiKey).GetAwaiter().GetResult();
      }

      Console.WriteLine(apiKeyEntity.ApiKey);

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

        // Metrics
        .AddSingleton<IMetricServiceUtils, MetricServiceUtils>()
        .AddSingleton<IMetricService, MetricService>()

        // Providers
        .AddSingleton<IRnGoConfigProvider, RnGoConfigProvider>()

        // Database
        .AddSingleton<IConnectionResolver>(new ConnectionResolver(config, "RnGo"))
        .AddSingleton<IDbConnectionHelper, MySqlConnectionHelper>()
        .AddSingleton<ILinkRepo, LinkRepo>()
        .AddSingleton<IApiKeyRepo, ApiKeyRepo>()
        .AddSingleton<ILinkRepoQueries, LinkRepoQueries>()
        .AddSingleton<IApiKeyRepoQueries, ApiKeyRepoQueries>();

      return services.BuildServiceProvider();
    }
  }
}
