using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using RnGo.Controllers;
using RnGo.Core.Services;

namespace RnGo.T1.Tests.Controllers;

[TestFixture]
public class FollowControllerTests
{
  private const string ShortCode = "B";

  [Test]
  public async Task Get_GivenKnownShortCode_ShouldRedirectToUrl()
  {
    // arrange
    var linkService = Substitute.For<ILinkService>();
    linkService.ResolveAsync(ShortCode).Returns("https://richardn.ca");

    // act
    var result = await new FollowController(linkService).Get(ShortCode);

    // assert
    Assert.That(result, Is.InstanceOf<RedirectResult>());
    Assert.That(((RedirectResult)result).Url, Is.EqualTo("https://richardn.ca"));
  }

  [Test]
  public async Task Get_GivenUnknownShortCode_ShouldReturnBadRequest()
  {
    // arrange
    var linkService = Substitute.For<ILinkService>();
    linkService.ResolveAsync(ShortCode).Returns(string.Empty);

    // act
    var result = await new FollowController(linkService).Get(ShortCode);

    // assert
    Assert.That(result, Is.InstanceOf<BadRequestResult>());
  }
}
