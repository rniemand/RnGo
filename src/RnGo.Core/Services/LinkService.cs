using Microsoft.Extensions.Logging;
using RnGo.Core.Models;
using RnGo.Core.Models.Responses;

namespace RnGo.Core.Services
{
  public interface ILinkService
  {
    Task<RnGoLink?> Resolve(string shortCode);
    Task<AddLinkResponse> AddLink(AddLinkRequest request);
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

    public async Task<AddLinkResponse> AddLink(AddLinkRequest request)
    {
      // TODO: [LinkService.StoreLink] (TESTS) Add tests
      var response = new AddLinkResponse();

      if (!IsValidLink(request.Url))
        return response.WithFailure("Invalid URL");

      // Check for an already existing link first
      var existingLink = await _linkStore.GetByUrl(request.Url);
      if (existingLink is not null)
        return response.WithSuccess(existingLink.ShortCode);

      // This is a new link, add it
      var addedLink = await _linkStore.StoreLink(request.Url);
      return string.IsNullOrWhiteSpace(addedLink) 
        ? response.WithFailure("Failed to add link") 
        : response.WithSuccess(addedLink);
    }

    public async Task<int> GetLinkCount()
    {
      // TODO: [LinkService.GetLinkCount] (TESTS) Add tests
      return await _linkStore.GetLinkCount();
    }

    // Internal
    private static bool IsValidLink(string link)
    {
      // TODO: [LinkService.IsValidLink] (TESTS) Add tests
      // TODO: [LinkService.IsValidLink] (EXPAND) Add better validation logic here
      return !string.IsNullOrWhiteSpace(link);
    }
  }
}
