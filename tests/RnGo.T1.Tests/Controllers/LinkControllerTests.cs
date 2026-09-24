using NSubstitute;
using NUnit.Framework;
using RnGo.Controllers;
using RnGo.Core.Models;
using RnGo.Core.Models.Responses;
using RnGo.Core.Services;

namespace RnGo.T1.Tests.Controllers;

[TestFixture]
public class LinkControllerTests
{
  [Test]
  public async Task StoreLink_GivenRequest_ShouldReturnServiceResponse()
  {
    // arrange
    var linkService = Substitute.For<ILinkService>();
    var request = new AddLinkRequest { Url = "https://richardn.ca", ApiKey = "KEY" };
    var expected = new AddLinkResponse().WithSuccess("B");
    linkService.AddLinkAsync(request).Returns(expected);

    // act
    var response = await new LinkController(linkService).StoreLink(request);

    // assert
    Assert.That(response, Is.SameAs(expected));
    await linkService.Received(1).AddLinkAsync(request);
  }

  [Test]
  public async Task GetLinkCount_GivenCalled_ShouldReturnServiceCount()
  {
    // arrange
    var linkService = Substitute.For<ILinkService>();
    linkService.GetLinkCount().Returns(42L);

    // act
    var count = await new LinkController(linkService).GetLinkCount();

    // assert
    Assert.That(count, Is.EqualTo(42));
  }
}
