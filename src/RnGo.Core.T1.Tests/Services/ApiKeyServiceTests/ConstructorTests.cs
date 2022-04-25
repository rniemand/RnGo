using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using RnGo.Core.Entities;
using RnGo.Core.Providers;
using RnGo.Core.Repos;
using RnGo.Core.T1.Tests.TestSupport.Builders;

namespace RnGo.Core.T1.Tests.Services.ApiKeyServiceTests;

[TestFixture]
public class ConstructorTests
{
  [Test]
  public void Constructor_GivenCalled_ShouldCallRnGoConfigProvider()
  {
    // Arrange
    var configProvider = Substitute.For<IRnGoConfigProvider>();
    configProvider.Provide().Returns(RnGoConfigBuilder.ValidDefault);

    // Act
    TestHelper.GetApiKeyService(
      configProvider: configProvider
    );

    // Assert
    configProvider.Received(1).Provide();
  }

  [Test]
  public void Constructor_GivenCalled_ShouldLoadConfigApiKeys()
  {
    // Arrange
    var configProvider = Substitute.For<IRnGoConfigProvider>();
    configProvider.Provide().Returns(RnGoConfigBuilder.ValidDefault);

    // Act
    var service = TestHelper.GetApiKeyService(
      configProvider: configProvider
    );

    // Assert
    Assert.That(service.ApiKeys.Count, Is.EqualTo(1));
    Assert.That(service.ApiKeys[0], Is.EqualTo(RnGoConfigBuilder.ValidApiKey));
  }

  [Test]
  public void Constructor_GivenCalled_ShouldLoadDatabaseApiKeys()
  {
    // Arrange
    var configProvider = Substitute.For<IRnGoConfigProvider>();
    var apiKeyRepo = Substitute.For<IApiKeyRepo>();
    configProvider.Provide().Returns(RnGoConfigBuilder.ValidDefault);
      
    apiKeyRepo
      .GetEnabledApiKeys()
      .Returns(new List<ApiKeyEntity>
      {
        ApiKeyEntityBuilder.ValidEntity
      });

    // Act
    var service = TestHelper.GetApiKeyService(
      configProvider: configProvider,
      apiKeyRepo: apiKeyRepo
    );

    // Assert
    Assert.That(service.ApiKeys.Count, Is.EqualTo(2));
    Assert.That(service.ApiKeys[1], Is.EqualTo(ApiKeyEntityBuilder.DefaultApiKey));
  }
}