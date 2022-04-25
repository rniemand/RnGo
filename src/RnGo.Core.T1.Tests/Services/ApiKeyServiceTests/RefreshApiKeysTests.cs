using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using RnGo.Core.Entities;
using RnGo.Core.Providers;
using RnGo.Core.Repos;
using RnGo.Core.T1.Tests.TestSupport.Builders;

namespace RnGo.Core.T1.Tests.Services.ApiKeyServiceTests;

[TestFixture]
public class RefreshApiKeysTests
{
  [Test]
  public void RefreshApiKeys_GivenCalled_ShouldClearApiKeys()
  {
    // Arrange
    var configProvider = Substitute.For<IRnGoConfigProvider>();
    var apiKeyRepo = Substitute.For<IApiKeyRepo>();

    configProvider.Provide().Returns(RnGoConfigBuilder.ValidDefault);
    apiKeyRepo.GetEnabledApiKeys().Returns(
      new List<ApiKeyEntity> { new ApiKeyEntityBuilder().BuildWithValidDefaults() },
      new List<ApiKeyEntity>()
    );

    var service = TestHelper.GetApiKeyService(
      configProvider: configProvider,
      apiKeyRepo: apiKeyRepo
    );

    // Act
    Assert.That(service.ApiKeys.Count, Is.EqualTo(2));
    service.RefreshApiKeys();

    // Assert
    Assert.That(service.ApiKeys.Count, Is.EqualTo(1));
  }
}