using Microsoft.Extensions.Logging;
using RnGo.Core.Helpers;
using RnGo.Core.Models;

namespace RnGo.Core.Services
{
  public interface ILinkService
  {
    ResolvedLink Resolve(string shortCode);
    string StoreLink(ResolvedLink link);
  }

  public class LinkService : ILinkService
  {
    private readonly ILogger<LinkService> _logger;
    private readonly IStringHelper _stringHelper;

    public LinkService(
      ILogger<LinkService> logger,
      IStringHelper stringHelper)
    {
      // TODO: [LinkService] (TESTS) Add tests
      _logger = logger;
      _stringHelper = stringHelper;
    }

    public ResolvedLink Resolve(string shortCode)
    {
      var encoded = _stringHelper.GuidString();

      return new ResolvedLink();
    }

    public string StoreLink(ResolvedLink link)
    {




      return "";
    }
  }
}
