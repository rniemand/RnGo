using System;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Rn.NetCore.Common.Logging;
using RnGo.Core.Entities;
using RnGo.Core.Helpers;
using RnGo.Core.Repos;
using RnGo.Core.Services;
using RnGo.Core.T1.Tests.TestSupport.Builders;

namespace RnGo.Core.T1.Tests.Services;

[TestFixture]
public class LinkServiceTests
{
  private const string SampleLinkUrl = "https://richardn.ca";
  private const string ApiKey = "0E25C180-0BB7-44A6-B0AE-3BF03D4E9D29";

  [Test]
  public async Task Constructor_GivenCalled_ShouldGenerateNextLinkId()
  {
    // arrange
    var linkRepo = Substitute.For<ILinkRepo>();
    var apiKeyService = Substitute.For<IApiKeyService>();
    var stringHelper = Substitute.For<IStringHelper>();

    var addLinkRequest = new AddLinkRequestBuilder()
      .WithUrl(SampleLinkUrl)
      .WithApiKey(ApiKey)
      .Build();

    apiKeyService
      .IsValidApiKeyAsync(ApiKey)
      .Returns(true);

    linkRepo
      .GetByUrlAsync(SampleLinkUrl)
      .ReturnsNull();

    // act
    var linkService = GetLinkService(linkRepo: linkRepo,
      apiKeyService: apiKeyService,
      stringHelper: stringHelper);

    await linkService.AddLinkAsync(addLinkRequest);

    // assert
    await linkRepo.Received(1).GetMaxLinkIdAsync();
    stringHelper.Received(1).GenerateLinkString(1);
  }

  [Test]
  public async Task Constructor_GivenHasMaxLinkId_ShouldUseReturnedMaxLinkId()
  {
    // arrange
    var linkRepo = Substitute.For<ILinkRepo>();
    var apiKeyService = Substitute.For<IApiKeyService>();
    var stringHelper = Substitute.For<IStringHelper>();

    var addLinkRequest = new AddLinkRequestBuilder()
      .WithUrl(SampleLinkUrl)
      .WithApiKey(ApiKey)
      .Build();

    apiKeyService
      .IsValidApiKeyAsync(ApiKey)
      .Returns(true);

    linkRepo
      .GetByUrlAsync(SampleLinkUrl)
      .ReturnsNull();

    linkRepo
      .GetMaxLinkIdAsync()
      .Returns(new GenericCountEntity { CountLong = 10 });

    // act
    var linkService = GetLinkService(linkRepo: linkRepo,
      apiKeyService: apiKeyService,
      stringHelper: stringHelper);

    await linkService.AddLinkAsync(addLinkRequest);

    // assert
    await linkRepo.Received(1).GetMaxLinkIdAsync();
    stringHelper.Received(1).GenerateLinkString(11);
  }

  [Test]
  public void Constructor_GivenInvalidMaxLinkId_ShouldThrow()
  {
    // arrange
    var linkRepo = Substitute.For<ILinkRepo>();
    
    linkRepo
      .GetMaxLinkIdAsync()
      .Returns(new GenericCountEntity { CountLong = -10 });

    // act
    var ex = Assert.Throws<Exception>(() => GetLinkService(linkRepo: linkRepo));

    // assert
    Assert.That(ex!.Message, Is.EqualTo("Unable to determine next link ID!"));
  }


  private static LinkService GetLinkService(ILoggerAdapter<LinkService>? logger = null,
    IApiKeyService? apiKeyService = null,
    ILinkRepo? linkRepo = null,
    IStringHelper? stringHelper = null) =>
    new(logger ?? Substitute.For<ILoggerAdapter<LinkService>>(),
      apiKeyService ?? Substitute.For<IApiKeyService>(),
      linkRepo ?? Substitute.For<ILinkRepo>(),
      stringHelper ?? Substitute.For<IStringHelper>());
}
