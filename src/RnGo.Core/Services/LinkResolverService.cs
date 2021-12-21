using Microsoft.Extensions.Logging;
using RnGo.Core.Helpers;
using RnGo.Core.Models;

namespace RnGo.Core.Services
{
  public interface ILinkResolverService
  {
    ResolvedLink Resolve(string shortCode);
  }

  public class LinkResolverService : ILinkResolverService
  {
    private readonly ILogger<LinkResolverService> _logger;
    private readonly IStringHelper _stringHelper;

    public LinkResolverService(
      ILogger<LinkResolverService> logger,
      IStringHelper stringHelper)
    {
      // TODO: [LinkResolverService] (TESTS) Add tests
      _logger = logger;
      _stringHelper = stringHelper;
    }

    public ResolvedLink Resolve(string shortCode)
    {


      return new ResolvedLink();
    }
  }
}
