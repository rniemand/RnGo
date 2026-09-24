using NUnit.Framework;
using RnGo.Core.Models.Responses;

namespace RnGo.Core.T1.Tests.Models;

[TestFixture]
public class AddLinkResponseTests
{
  [Test]
  public void WithFailure_GivenMessage_ShouldSetFailureState()
  {
    // act
    var response = new AddLinkResponse().WithFailure("Nope");

    // assert
    Assert.That(response.Success, Is.False);
    Assert.That(response.Messages, Is.EqualTo(new[] { "Nope" }));
    Assert.That(response.ShortCode, Is.Empty);
  }

  [Test]
  public void WithSuccess_GivenShortCode_ShouldSetSuccessState()
  {
    // act
    var response = new AddLinkResponse().WithSuccess("SD");

    // assert
    Assert.That(response.Success, Is.True);
    Assert.That(response.ShortCode, Is.EqualTo("SD"));
    Assert.That(response.Messages, Is.Empty);
  }
}
