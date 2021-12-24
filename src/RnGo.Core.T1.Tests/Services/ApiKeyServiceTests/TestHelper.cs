using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RnGo.Core.Entities;
using RnGo.Core.Providers;
using RnGo.Core.Repos;
using RnGo.Core.Services;
using RnGo.Core.T1.Tests.TestSupport.Builders;

namespace RnGo.Core.T1.Tests.Services.ApiKeyServiceTests
{
  public class TestHelper
  {
    public static ApiKeyService GetApiKeyService(
      ILogger<ApiKeyService>? logger = null,
      IRnGoConfigProvider? configProvider = null,
      IApiKeyRepo? apiKeyRepo = null)
    {
      return new ApiKeyService(
        logger ?? Substitute.For<ILogger<ApiKeyService>>(),
        EnsureValidConfigProvider(configProvider),
        EnsureValidApiKeyRepo(apiKeyRepo)
      );
    }

    private static IRnGoConfigProvider EnsureValidConfigProvider(IRnGoConfigProvider? provider = null)
    {
      if (provider is not null)
        return provider;

      provider = Substitute.For<IRnGoConfigProvider>();
      provider.Provide().Returns(RnGoConfigBuilder.ValidDefault);
      return provider;
    }

    private static IApiKeyRepo EnsureValidApiKeyRepo(IApiKeyRepo? repo = null)
    {
      if (repo is not null)
        return repo;

      repo = Substitute.For<IApiKeyRepo>();
      repo.GetEnabledApiKeys().Returns(new List<ApiKeyEntity>());
      return repo;
    }
  }
}
