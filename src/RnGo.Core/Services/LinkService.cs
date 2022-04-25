using Microsoft.Extensions.Logging;
using RnGo.Core.Models;
using RnGo.Core.Models.Responses;

namespace RnGo.Core.Services;

public interface ILinkService
{
  Task<string> Resolve(string shortCode);
  Task<AddLinkResponse> AddLink(AddLinkRequest request);
  Task<long> GetLinkCount();
}

public class LinkService : ILinkService
{
  private readonly ILogger<LinkService> _logger;
  private readonly ILinkStorageService _linkStore;
  private readonly ILinkStatsService _statsService;
  private readonly IApiKeyService _apiKeyService;

  public LinkService(
    ILogger<LinkService> logger,
    ILinkStorageService linkStore,
    ILinkStatsService statsService,
    IApiKeyService apiKeyService)
  {
    // TODO: [LinkService] (TESTS) Add tests
    _logger = logger;
    _linkStore = linkStore;
    _statsService = statsService;
    _apiKeyService = apiKeyService;
  }

  public async Task<string> Resolve(string shortCode)
  {
    // TODO: [LinkService.Resolve] (TESTS) Add tests
    var link = await _linkStore.GetByShortCode(shortCode);

    if (link is null)
      return string.Empty;

    await _statsService.RecordLinkFollow(link);

    return link.Url;
  }

  public async Task<AddLinkResponse> AddLink(AddLinkRequest request)
  {
    // TODO: [LinkService.StoreLink] (TESTS) Add tests
    var response = new AddLinkResponse();

    // Ensure that this is a valid URL
    if (!IsValidLink(request.Url))
      return response.WithFailure("Invalid URL");

    // Ensure that this is a valid API key
    if (!await _apiKeyService.IsValidApiKey(request.ApiKey))
      return response.WithFailure("Invalid API key");

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

  public async Task<long> GetLinkCount()
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