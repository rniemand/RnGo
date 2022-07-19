using NSubstitute;
using NUnit.Framework;
using Rn.NetCore.Common.Logging;
using RnGo.Core.Helpers;
using RnGo.Core.Repos;
using RnGo.Core.Services;

namespace RnGo.Core.T1.Tests.Services;

[TestFixture]
public class LinkServiceTests
{


  private static LinkService GetLinkService(ILoggerAdapter<LinkService>? logger = null,
    IApiKeyService? apiKeyService = null,
    ILinkRepo? linkRepo = null,
    IStringHelper? stringHelper = null) =>
    new LinkService(logger ?? Substitute.For<ILoggerAdapter<LinkService>>(),
      apiKeyService ?? Substitute.For<IApiKeyService>(),
      linkRepo ?? Substitute.For<ILinkRepo>(),
      stringHelper ?? Substitute.For<IStringHelper>());
}
