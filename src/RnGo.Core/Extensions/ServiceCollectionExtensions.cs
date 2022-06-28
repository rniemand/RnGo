using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Helpers;
using Rn.NetCore.Common.Logging;
using RnGo.Core.Helpers;
using RnGo.Core.RepoQueries;
using RnGo.Core.Repos;
using RnGo.Core.Services;

namespace RnGo.Core.Extensions;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddRnGo(this IServiceCollection services, IConfiguration configuration)
  {
    // Abstractions
    services.TryAddSingleton<IFileAbstraction, FileAbstraction>();
    services.TryAddSingleton<IDirectoryAbstraction, DirectoryAbstraction>();
    services.TryAddSingleton<IEnvironmentAbstraction, EnvironmentAbstraction>();
    services.TryAddSingleton<IPathAbstraction, PathAbstraction>();
    services.TryAddSingleton<IDateTimeAbstraction, DateTimeAbstraction>();

    // Helpers
    services.TryAddSingleton<IJsonHelper, JsonHelper>();

    // Logging
    services.TryAddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));

    return services
      // Services
      .AddSingleton<ILinkService, LinkService>()
      .AddSingleton<ILinkStorageService, LinkStorageService>()
      .AddSingleton<ILinkStatsService, LinkStatsService>()
      .AddSingleton<IApiKeyService, ApiKeyService>()

      // Helpers
      .AddSingleton<IStringHelper, StringHelper>()

      // DB: Repositories
      .AddSingleton<ILinkRepo, LinkRepo>()
      .AddSingleton<IApiKeyRepo, ApiKeyRepo>()

      // DB: Queries
      .AddSingleton<ILinkRepoQueries, LinkRepoQueries>()
      .AddSingleton<IApiKeyRepoQueries, ApiKeyRepoQueries>();
  }
}
