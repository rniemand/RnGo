using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using RnGo.Core.Entities;
using RnGo.Core.Helpers;
using RnGo.Core.Repos;
using RnGo.Core.Services;
using RnGo.Core.T1.Tests.TestSupport.Builders;

namespace RnGo.Core.T1.Tests.Services.LinkServiceTests;

[TestFixture]
public class ConcurrencyTests
{
  private const int RequestCount = 200;

  [Test]
  public async Task AddLinkAsync_GivenConcurrentRequests_ShouldAssignUniqueLinkIds()
  {
    // arrange
    var apiKeyService = Substitute.For<IApiKeyService>();
    var linkRepo = Substitute.For<ILinkRepo>();
    var stringHelper = Substitute.For<IStringHelper>();

    apiKeyService.IsValidApiKeyAsync(Arg.Any<string>()).Returns(true);
    linkRepo.GetMaxLinkIdAsync().Returns(new GenericCountEntity(100));

    var linkService = TestHelper.GetLinkService(apiKeyService: apiKeyService,
      linkRepo: linkRepo,
      stringHelper: stringHelper);

    // act
    await Task.WhenAll(Enumerable.Range(0, RequestCount).Select(i => Task.Run(() =>
      linkService.AddLinkAsync(new AddLinkRequestBuilder()
        .WithUrl($"https://richardn.ca/{i}")
        .WithApiKey("KEY")
        .Build()))));

    // assert
    var linkIds = stringHelper.ReceivedCalls()
      .Where(c => c.GetMethodInfo().Name == nameof(IStringHelper.GenerateLinkString))
      .Select(c => (long)c.GetArguments()[0]!)
      .OrderBy(x => x)
      .ToList();

    Assert.That(linkIds, Is.EqualTo(Enumerable.Range(101, RequestCount).Select(x => (long)x)));
  }
}
