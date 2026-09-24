using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Rn.NetCore.Common.Abstractions;
using RnGo.Core.Entities;
using RnGo.Core.Repos;
using RnGo.Core.Services;
using RnGo.Core.T1.Tests.TestSupport.Builders;

namespace RnGo.Core.T1.Tests.Services;

[TestFixture]
public class ApiKeyServiceTests
{
  private const string ApiKey = "3BC034D6-B059-401A-8C5F-1BB1B5CA214B";

  private IApiKeyRepo _apiKeyRepo = null!;
  private IDateTimeAbstraction _dateTime = null!;
  private DateTime _now;

  [SetUp]
  public void SetUp()
  {
    _apiKeyRepo = Substitute.For<IApiKeyRepo>();
    _dateTime = Substitute.For<IDateTimeAbstraction>();
    _now = new DateTime(2026, 1, 1, 12, 0, 0);

    _dateTime.Now.Returns(_ => _now);

    _apiKeyRepo
      .GetEnabledAsync()
      .Returns(new List<ApiKeyEntity> { new ApiKeyEntityBuilder().WithApiKey(ApiKey).Build() });
  }

  [Test]
  public async Task IsValidApiKeyAsync_GivenNoKeysInDb_ShouldReturnFalse()
  {
    // arrange
    _apiKeyRepo.GetEnabledAsync().Returns(new List<ApiKeyEntity>());

    // act
    var isValid = await GetService().IsValidApiKeyAsync(ApiKey);

    // assert
    Assert.That(isValid, Is.False);
  }

  [Test]
  public async Task IsValidApiKeyAsync_GivenMatchingKey_ShouldReturnTrue()
  {
    // act
    var isValid = await GetService().IsValidApiKeyAsync(ApiKey);

    // assert
    Assert.That(isValid, Is.True);
  }

  [Test]
  public async Task IsValidApiKeyAsync_GivenLowercaseKey_ShouldMatchCaseInsensitively()
  {
    // act
    var isValid = await GetService().IsValidApiKeyAsync(ApiKey.ToLower());

    // assert
    Assert.That(isValid, Is.True);
  }

  [Test]
  public async Task IsValidApiKeyAsync_GivenUnknownKey_ShouldReturnFalse()
  {
    // act
    var isValid = await GetService().IsValidApiKeyAsync("NOT-A-REAL-KEY");

    // assert
    Assert.That(isValid, Is.False);
  }

  [Test]
  public async Task IsValidApiKeyAsync_GivenCalledWithinRefreshWindow_ShouldOnlyLoadKeysOnce()
  {
    // arrange
    var service = GetService();

    // act
    await service.IsValidApiKeyAsync(ApiKey);
    _now = _now.AddMinutes(9);
    await service.IsValidApiKeyAsync(ApiKey);

    // assert
    await _apiKeyRepo.Received(1).GetEnabledAsync();
  }

  [Test]
  public async Task IsValidApiKeyAsync_GivenRefreshWindowElapsed_ShouldReloadKeys()
  {
    // arrange
    var service = GetService();

    // act
    await service.IsValidApiKeyAsync(ApiKey);
    _now = _now.AddMinutes(10);
    await service.IsValidApiKeyAsync(ApiKey);

    // assert
    await _apiKeyRepo.Received(2).GetEnabledAsync();
  }

  [Test]
  public async Task RefreshApiKeys_GivenReload_ShouldReplaceExistingKeys()
  {
    // arrange
    const string newKey = "EA0EA5E3-F494-4458-B93D-AA6E06F58EBD";
    var service = GetService();
    await service.RefreshApiKeys();

    _apiKeyRepo
      .GetEnabledAsync()
      .Returns(new List<ApiKeyEntity> { new ApiKeyEntityBuilder().WithApiKey(newKey).Build() });

    // act
    _now = _now.AddMinutes(10);
    await service.RefreshApiKeys();

    // assert
    Assert.That(service.ApiKeys, Is.EqualTo(new[] { newKey }));
  }

  private ApiKeyService GetService() =>
    new(Substitute.For<ILogger<ApiKeyService>>(), _apiKeyRepo, _dateTime);
}
