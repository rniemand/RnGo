using NSubstitute;
using Rn.NetCore.Common.Logging;
using RnGo.Core.Helpers;
using RnGo.Core.Repos;
using RnGo.Core.Services;

namespace RnGo.Core.T1.Tests.Services.LinkServiceTests;

public static class TestHelper
{
  public static LinkService GetLinkService(ILoggerAdapter<LinkService>? logger = null,
    IApiKeyService? apiKeyService = null,
    ILinkRepo? linkRepo = null,
    IStringHelper? stringHelper = null) =>
    new(logger ?? Substitute.For<ILoggerAdapter<LinkService>>(),
      apiKeyService ?? Substitute.For<IApiKeyService>(),
      linkRepo ?? Substitute.For<ILinkRepo>(),
      stringHelper ?? Substitute.For<IStringHelper>());
}
