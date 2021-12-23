using Microsoft.Extensions.Logging;
using RnGo.Core.Models;

namespace RnGo.Core.Services
{
  public interface ILinkService
  {
    Task<RnGoLink?> Resolve(string shortCode);
    Task<string> StoreLink(RnGoLink link);
    Task<int> GetLinkCount();
  }

  public class LinkService : ILinkService
  {
    private readonly ILogger<LinkService> _logger;
    private readonly ILinkStorageService _linkStore;
    private readonly ILinkStatsService _statsService;

    public LinkService(
      ILogger<LinkService> logger,
      ILinkStorageService linkStore,
      ILinkStatsService statsService)
    {
      // TODO: [LinkService] (TESTS) Add tests
      _logger = logger;
      _linkStore = linkStore;
      _statsService = statsService;
    }

    public async Task<RnGoLink?> Resolve(string shortCode)
    {
      // TODO: [LinkService.Resolve] (TESTS) Add tests
      var link = await _linkStore.GetByShortCode(shortCode);

      if (link is not null)
        await _statsService.RecordLinkFollow(link);

      return link ?? null;
    }

    public async Task<string> StoreLink(RnGoLink link)
    {
      // TODO: [LinkService.StoreLink] (TESTS) Add tests
      if (!IsValidLink(link))
        return string.Empty;

      var resolvedLink = await _linkStore.GetByUrl(link.Url);
      if (resolvedLink is null)
        return await _linkStore.StoreLink(link);

      return resolvedLink.ShortCode;
    }

    public async Task<int> GetLinkCount()
    {
      // TODO: [LinkService.GetLinkCount] (TESTS) Add tests
      return await _linkStore.GetLinkCount();
    }

    // Internal
    private bool IsValidLink(RnGoLink? link)
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
