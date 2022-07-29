using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using RnGo.Core.Repos;
using RnGo.Core.T1.Tests.TestSupport.Builders;

namespace RnGo.Core.T1.Tests.Services.LinkServiceTests;

[TestFixture]
public class ResolveAsyncTests
{
  private const string ShortCode = "AA3";
  private const long LinkId = 1233;
  private const string LinkUrl = "https://richardn.ca";

  [Test]
  public async Task ResolveAsync_GivenLinkNotResolved_ShouldReturnEmptyString()
  {
    // arrange
    var linkRepo = Substitute.For<ILinkRepo>();

    linkRepo
      .GetByShortCodeAsync(ShortCode)
      .ReturnsNull();

    // act
    var linkService = TestHelper.GetLinkService(linkRepo: linkRepo);
    var resolvedLink = await linkService.ResolveAsync(ShortCode);

    // assert
    Assert.That(resolvedLink, Is.EqualTo(string.Empty));
  }

  [Test]
  public async Task ResolveAsync_GivenLinkResolved_ShouldUpdateFollowCount()
  {
    // arrange
    var linkRepo = Substitute.For<ILinkRepo>();

    var linkEntity = new LinkEntityBuilder()
      .WithLinkId(LinkId)
      .WithShortCode(ShortCode)
      .Build();

    linkRepo
      .GetByShortCodeAsync(ShortCode)
      .Returns(linkEntity);

    // act
    var linkService = TestHelper.GetLinkService(linkRepo: linkRepo);
    await linkService.ResolveAsync(ShortCode);

    // assert
    await linkRepo.Received(1).UpdateFollowCountAsync(LinkId);
  }

  [Test]
  public async Task ResolveAsync_GivenLinkResolved_ShouldReturnLinkUrl()
  {
    // arrange
    var linkRepo = Substitute.For<ILinkRepo>();

    var linkEntity = new LinkEntityBuilder()
      .WithLinkId(LinkId)
      .WithShortCode(ShortCode)
      .WithUrl(LinkUrl)
      .Build();

    linkRepo
      .GetByShortCodeAsync(ShortCode)
      .Returns(linkEntity);

    // act
    var linkService = TestHelper.GetLinkService(linkRepo: linkRepo);
    var resolvedUrl = await linkService.ResolveAsync(ShortCode);

    // assert
    Assert.That(resolvedUrl, Is.EqualTo(LinkUrl));
  }
}
