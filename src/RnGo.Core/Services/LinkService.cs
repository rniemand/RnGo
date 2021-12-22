using Microsoft.Extensions.Logging;
using RnGo.Core.Helpers;
using RnGo.Core.Models;

namespace RnGo.Core.Services
{
  public interface ILinkService
  {
    Task<ResolvedLink> Resolve(string shortCode);
    Task<string> StoreLink(ResolvedLink link);
  }

  public class LinkService : ILinkService
  {
    private readonly ILogger<LinkService> _logger;
    private readonly IStringHelper _stringHelper;
    private readonly ILinkStorageService _linkStore;

    public LinkService(
      ILogger<LinkService> logger,
      IStringHelper stringHelper,
      ILinkStorageService linkStore)
    {
      // TODO: [LinkService] (TESTS) Add tests
      _logger = logger;
      _stringHelper = stringHelper;
      _linkStore = linkStore;
    }

    public async Task<ResolvedLink> Resolve(string shortCode)
    {
      var encoded = _stringHelper.GuidString();

      return new ResolvedLink();
    }

    public async Task<string> StoreLink(ResolvedLink link)
    {
      // TODO: [LinkService.StoreLink] (TESTS) Add tests
      if (!IsValidLink(link))
        return string.Empty;

      var resolvedLink = await _linkStore.GetByUrl(link.Url);
      if (resolvedLink is null)
        return await _linkStore.StoreLink(link);

      return resolvedLink.ShortCode;
    }

    // Internal
    private bool IsValidLink(ResolvedLink? link)
    {
      // TODO: [LinkService.IsValidLink] (TESTS) Add tests
      if (link is null)
        return false;

      // ReSharper disable once ConvertIfStatementToReturnStatement
      if (string.IsNullOrWhiteSpace(link.Url))
        return false;

      return true;
    }
  }
}
