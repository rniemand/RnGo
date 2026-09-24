using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using RnGo.Core.Entities;
using RnGo.Core.Repos;

namespace RnGo.Core.T1.Tests.Services.LinkServiceTests;

[TestFixture]
public class GetLinkCountTests
{
  [Test]
  public async Task GetLinkCount_GivenMaxLinkId_ShouldReturnIt()
  {
    // arrange
    var linkRepo = Substitute.For<ILinkRepo>();

    linkRepo
      .GetMaxLinkIdAsync()
      .Returns(new GenericCountEntity(25));

    // act
    var linkService = TestHelper.GetLinkService(linkRepo: linkRepo);
    var count = await linkService.GetLinkCount();

    // assert
    Assert.That(count, Is.EqualTo(25));
  }

  [Test]
  public async Task GetLinkCount_GivenNoLinks_ShouldReturnZero()
  {
    // arrange
    var linkRepo = Substitute.For<ILinkRepo>();

    linkRepo
      .GetMaxLinkIdAsync()
      .ReturnsNull();

    // act
    var linkService = TestHelper.GetLinkService(linkRepo: linkRepo);
    var count = await linkService.GetLinkCount();

    // assert
    Assert.That(count, Is.EqualTo(0));
  }
}
