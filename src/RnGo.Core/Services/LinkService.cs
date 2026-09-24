using Rn.NetCore.Common.Logging;
using RnGo.Core.Entities;
using RnGo.Core.Helpers;
using RnGo.Core.Models;
using RnGo.Core.Models.Responses;
using RnGo.Core.Repos;

namespace RnGo.Core.Services;

public interface ILinkService
{
  Task<string> ResolveAsync(string shortCode);
  Task<AddLinkResponse> AddLinkAsync(AddLinkRequest request);
  Task<long> GetLinkCount();
}

public class LinkService : ILinkService
{
  private readonly ILoggerAdapter<LinkService> _logger;
  private readonly IApiKeyService _apiKeyService;
  private readonly ILinkRepo _linkRepo;
  private readonly IStringHelper _stringHelper;
  // Singleton shared across requests: only ever advanced via Interlocked
  private long _lastLinkId;

  public LinkService(ILoggerAdapter<LinkService> logger,
    IApiKeyService apiKeyService,
    ILinkRepo linkRepo,
    IStringHelper stringHelper)
  {
    _logger = logger;
    _apiKeyService = apiKeyService;
    _linkRepo = linkRepo;
    _stringHelper = stringHelper;

    _lastLinkId = GetMaxLinkId();
  }

  public async Task<string> ResolveAsync(string shortCode)
  {
    var link = await _linkRepo.GetByShortCodeAsync(shortCode);

    if (link is null)
      return string.Empty;

    await _linkRepo.UpdateFollowCountAsync(link.LinkId);

    return link.Url;
  }

  public async Task<AddLinkResponse> AddLinkAsync(AddLinkRequest request)
  {
    var response = new AddLinkResponse();

    if (!IsValidLink(request.Url))
      return response.WithFailure("Invalid URL");

    if (!await _apiKeyService.IsValidApiKeyAsync(request.ApiKey))
      return response.WithFailure("Invalid API key");

    var existingLink = await _linkRepo.GetByUrlAsync(request.Url);
    if (existingLink is not null)
      return response.WithSuccess(existingLink.ShortCode);

    // This is a new link, add it
    var linkId = Interlocked.Increment(ref _lastLinkId);
    var shortCode = _stringHelper.GenerateLinkString(linkId);
    var linkEntity = new LinkEntity(request.Url, shortCode);

    var rowCount = await _linkRepo.AddAsync(linkEntity);
    if (rowCount <= 0)
    {
      _logger.LogError("Failed to store link: {url}", request.Url);
      return response.WithFailure("Failed to add link");
    }

    var dbLink = await _linkRepo.GetByIdAsync(linkId);
    if (dbLink is not null)
      return response.WithSuccess(dbLink.ShortCode);

    // Failed to store the DB link
    _logger.LogError("Failed to store link: {url}", request.Url);
    return response.WithFailure("Failed to add link");
  }

  public async Task<long> GetLinkCount()
  {
    var countEntity = await _linkRepo.GetMaxLinkIdAsync();
    return countEntity?.CountLong ?? 0;
  }


  // Internal methods
  private static bool IsValidLink(string link) =>
    !string.IsNullOrWhiteSpace(link);

  private long GetMaxLinkId()
  {
    // MAX(LinkId) over an empty table is NULL, which maps to 0 (or no row at all)
    var maxLinkId = _linkRepo.GetMaxLinkIdAsync().GetAwaiter().GetResult()?.CountLong ?? 0;
    if (maxLinkId < 0)
      throw new Exception("Unable to determine next link ID!");

    return maxLinkId;
  }
}
