using System.Linq;
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

namespace RnGo.Core.T1.Tests.Services.LinkServiceTests;

[TestFixture]
public class AddLinkAsyncTests
{
  private const string LinkUrl = "https://richardn.ca";
  private const string ApiKey = "62E66F24-EE56-43C9-9BB1-74EE7E6697D7";
  private const string ShortCode = "JK";
  private const long LinkId = 11;

  [Test]
  public async Task AddLinkAsync_GivenInvalidLinkUrl_ShouldReturnFailure()
  {
    // arrange
    var addLinkRequest = new AddLinkRequestBuilder()
      .WithUrl("")
      .Build();

    // act
    var linkService = TestHelper.GetLinkService();
    var response = await linkService.AddLinkAsync(addLinkRequest);

    // assert
    Assert.That(response.Success, Is.False);
    Assert.That(response.Messages.First(), Is.EqualTo("Invalid URL"));
  }

  [Test]
  public async Task AddLinkAsync_GivenInvalidApiUrl_ShouldReturnFailure()
  {
    // arrange
    var apiKeyService = Substitute.For<IApiKeyService>();

    var addLinkRequest = new AddLinkRequestBuilder()
      .WithUrl(LinkUrl)
      .WithApiKey(ApiKey)
      .Build();

    apiKeyService
      .IsValidApiKeyAsync(ApiKey)
      .Returns(false);

    // act
    var linkService = TestHelper.GetLinkService(apiKeyService: apiKeyService);
    var response = await linkService.AddLinkAsync(addLinkRequest);

    // assert
    Assert.That(response.Success, Is.False);
    Assert.That(response.Messages.First(), Is.EqualTo("Invalid API key"));
  }

  [Test]
  public async Task AddLinkAsync_GivenLinkAlreadyExists_ShouldReturnExistingLink()
  {
    // arrange
    var apiKeyService = Substitute.For<IApiKeyService>();
    var linkRepo = Substitute.For<ILinkRepo>();

    var addLinkRequest = new AddLinkRequestBuilder()
      .WithUrl(LinkUrl)
      .WithApiKey(ApiKey)
      .Build();

    var linkEntity = new LinkEntityBuilder()
      .FromAddLinkRequest(addLinkRequest)
      .WithShortCode(ShortCode)
      .Build();

    linkRepo
      .GetByUrlAsync(LinkUrl)
      .Returns(linkEntity);

    apiKeyService
      .IsValidApiKeyAsync(ApiKey)
      .Returns(true);

    // act
    var linkService = TestHelper.GetLinkService(apiKeyService: apiKeyService,
      linkRepo: linkRepo);

    var response = await linkService.AddLinkAsync(addLinkRequest);

    // assert
    Assert.That(response.Success, Is.True);
    Assert.That(response.ShortCode, Is.EqualTo(ShortCode));
  }

  [Test]
  public async Task AddLinkAsync_GivenLinkIsNew_ShouldAddNewLink()
  {
    // arrange
    var apiKeyService = Substitute.For<IApiKeyService>();
    var linkRepo = Substitute.For<ILinkRepo>();
    var stringHelper = Substitute.For<IStringHelper>();

    var addLinkRequest = new AddLinkRequestBuilder()
      .WithUrl(LinkUrl)
      .WithApiKey(ApiKey)
      .Build();

    linkRepo.GetMaxLinkIdAsync().Returns(new GenericCountEntity(10));
    apiKeyService.IsValidApiKeyAsync(ApiKey).Returns(true);
    stringHelper.GenerateLinkString(11).Returns(ShortCode);

    // act
    var linkService = TestHelper.GetLinkService(apiKeyService: apiKeyService,
      linkRepo: linkRepo,
      stringHelper: stringHelper);

    await linkService.AddLinkAsync(addLinkRequest);

    // assert
    await linkRepo.Received(1).AddAsync(Arg.Is<LinkEntity>(l =>
      l.Url.Equals(LinkUrl) &&
      l.ShortCode.Equals(ShortCode)));
  }

  [Test]
  public async Task AddLinkAsync_GivenFailedToAddLink_ShouldLog()
  {
    // arrange
    var apiKeyService = Substitute.For<IApiKeyService>();
    var linkRepo = Substitute.For<ILinkRepo>();
    var stringHelper = Substitute.For<IStringHelper>();
    var logger = Substitute.For<ILoggerAdapter<LinkService>>();

    var addLinkRequest = new AddLinkRequestBuilder()
      .WithUrl(LinkUrl)
      .WithApiKey(ApiKey)
      .Build();

    linkRepo.GetMaxLinkIdAsync().Returns(new GenericCountEntity(10));
    apiKeyService.IsValidApiKeyAsync(ApiKey).Returns(true);
    stringHelper.GenerateLinkString(11).Returns(ShortCode);

    // act
    var linkService = TestHelper.GetLinkService(apiKeyService: apiKeyService,
      linkRepo: linkRepo,
      stringHelper: stringHelper,
      logger: logger);

    await linkService.AddLinkAsync(addLinkRequest);

    // assert
    logger.Received(1).LogError("Failed to store link: {url}", addLinkRequest.Url);
  }

  [Test]
  public async Task AddLinkAsync_GivenFailedToAddLink_ShouldReturnFailure()
  {
    // arrange
    var apiKeyService = Substitute.For<IApiKeyService>();
    var linkRepo = Substitute.For<ILinkRepo>();
    var stringHelper = Substitute.For<IStringHelper>();

    var addLinkRequest = new AddLinkRequestBuilder()
      .WithUrl(LinkUrl)
      .WithApiKey(ApiKey)
      .Build();

    linkRepo.GetMaxLinkIdAsync().Returns(new GenericCountEntity(10));
    apiKeyService.IsValidApiKeyAsync(ApiKey).Returns(true);
    stringHelper.GenerateLinkString(11).Returns(ShortCode);

    // act
    var linkService = TestHelper.GetLinkService(apiKeyService: apiKeyService,
      linkRepo: linkRepo,
      stringHelper: stringHelper);

    var addLinkResponse = await linkService.AddLinkAsync(addLinkRequest);

    // assert
    Assert.That(addLinkResponse.Success, Is.False);
    Assert.That(addLinkResponse.Messages.First(), Is.EqualTo("Failed to add link"));
  }

  [Test]
  public async Task AddLinkAsync_GivenLinkAdded_ShouldReturnLinkShortCode()
  {
    // arrange
    var apiKeyService = Substitute.For<IApiKeyService>();
    var linkRepo = Substitute.For<ILinkRepo>();
    var stringHelper = Substitute.For<IStringHelper>();

    var addLinkRequest = new AddLinkRequestBuilder()
      .WithUrl(LinkUrl)
      .WithApiKey(ApiKey)
      .Build();

    var linkEntity = new LinkEntityBuilder()
      .FromAddLinkRequest(addLinkRequest)
      .WithLinkId(LinkId)
      .WithShortCode(ShortCode)
      .Build();

    linkRepo.GetMaxLinkIdAsync().Returns(new GenericCountEntity(10));
    linkRepo.AddAsync(Arg.Any<LinkEntity>()).Returns(1);
    linkRepo.GetByIdAsync(LinkId).Returns(linkEntity);
    apiKeyService.IsValidApiKeyAsync(ApiKey).Returns(true);
    stringHelper.GenerateLinkString(11).Returns(ShortCode);

    // act
    var linkService = TestHelper.GetLinkService(apiKeyService: apiKeyService,
      linkRepo: linkRepo,
      stringHelper: stringHelper);

    var addLinkResponse = await linkService.AddLinkAsync(addLinkRequest);

    // assert
    Assert.That(addLinkResponse.Success, Is.True);
    Assert.That(addLinkResponse.ShortCode, Is.EqualTo(ShortCode));
  }

  [Test]
  public async Task AddLinkAsync_GivenAddedLinkNotReturnedFromDb_ShouldLog()
  {
    // arrange
    var apiKeyService = Substitute.For<IApiKeyService>();
    var linkRepo = Substitute.For<ILinkRepo>();
    var stringHelper = Substitute.For<IStringHelper>();
    var logger = Substitute.For<ILoggerAdapter<LinkService>>();

    var addLinkRequest = new AddLinkRequestBuilder()
      .WithUrl(LinkUrl)
      .WithApiKey(ApiKey)
      .Build();

    linkRepo.GetMaxLinkIdAsync().Returns(new GenericCountEntity(10));
    linkRepo.AddAsync(Arg.Any<LinkEntity>()).Returns(1);
    linkRepo.GetByIdAsync(LinkId).ReturnsNull();
    apiKeyService.IsValidApiKeyAsync(ApiKey).Returns(true);
    stringHelper.GenerateLinkString(11).Returns(ShortCode);

    // act
    var linkService = TestHelper.GetLinkService(apiKeyService: apiKeyService,
      linkRepo: linkRepo,
      stringHelper: stringHelper,
      logger: logger);

    await linkService.AddLinkAsync(addLinkRequest);

    // assert
    logger.Received(1).LogError("Failed to store link: {url}", addLinkRequest.Url);
  }

  [Test]
  public async Task AddLinkAsync_GivenAddedLinkNotReturnedFromDb_ShouldReturnFailure()
  {
    // arrange
    var apiKeyService = Substitute.For<IApiKeyService>();
    var linkRepo = Substitute.For<ILinkRepo>();
    var stringHelper = Substitute.For<IStringHelper>();

    var addLinkRequest = new AddLinkRequestBuilder()
      .WithUrl(LinkUrl)
      .WithApiKey(ApiKey)
      .Build();

    linkRepo.GetMaxLinkIdAsync().Returns(new GenericCountEntity(10));
    linkRepo.AddAsync(Arg.Any<LinkEntity>()).Returns(1);
    linkRepo.GetByIdAsync(LinkId).ReturnsNull();
    apiKeyService.IsValidApiKeyAsync(ApiKey).Returns(true);
    stringHelper.GenerateLinkString(11).Returns(ShortCode);

    // act
    var linkService = TestHelper.GetLinkService(apiKeyService: apiKeyService,
      linkRepo: linkRepo,
      stringHelper: stringHelper);

    var addLinkResponse = await linkService.AddLinkAsync(addLinkRequest);

    // assert
    Assert.That(addLinkResponse.Success, Is.False);
    Assert.That(addLinkResponse.Messages.First(), Is.EqualTo("Failed to add link"));
  }
}
