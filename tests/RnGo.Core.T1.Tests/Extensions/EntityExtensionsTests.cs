using NUnit.Framework;
using RnGo.Core.Extensions;
using RnGo.Core.Models.Dto;

namespace RnGo.Core.T1.Tests.Extensions;

[TestFixture]
public class EntityExtensionsTests
{
  [Test]
  public void ToEntity_GivenDto_ShouldMapAllFields()
  {
    // arrange
    var dto = new RnGoLinkDto
    {
      LinkId = 11,
      Url = "https://richardn.ca",
      ShortCode = "B"
    };

    // act
    var entity = dto.ToEntity();

    // assert
    Assert.That(entity.LinkId, Is.EqualTo(11));
    Assert.That(entity.Url, Is.EqualTo("https://richardn.ca"));
    Assert.That(entity.ShortCode, Is.EqualTo("B"));
  }
}
