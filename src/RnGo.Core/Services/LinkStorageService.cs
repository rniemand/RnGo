using Microsoft.Extensions.Logging;
using RnGo.Core.Entities;
using RnGo.Core.Helpers;
using RnGo.Core.Models.Dto;
using RnGo.Core.Repos;

namespace RnGo.Core.Services;

public interface ILinkStorageService
{
  Task<RnGoLinkDto?> GetByUrl(string url);
  Task<string> StoreLink(string link);
  Task<LinkEntity?> GetByShortCode(string shortCode);
  Task<long> GetLinkCount();
}

public class LinkStorageService : ILinkStorageService
{
  private readonly ILogger<LinkStorageService> _logger;
  private readonly IStringHelper _stringHelper;
  private readonly ILinkRepo _linkRepo;
  private long _nextLinkId;

  public LinkStorageService(
    ILogger<LinkStorageService> logger,
    IStringHelper stringHelper,
    ILinkRepo linkRepo)
  {
    // TODO: [LinkStorageService] (TESTS) Add tests
    _logger = logger;
    _stringHelper = stringHelper;
    _linkRepo = linkRepo;
    _nextLinkId = GetNextLinkId();

    logger.LogInformation("Setting next linkId to {id}", _nextLinkId);
  }


  // Interface methods
  public async Task<RnGoLinkDto?> GetByUrl(string url)
  {
    // TODO: [LinkStorageService.GetByUrl] (TESTS) Add tests
    if (string.IsNullOrWhiteSpace(url))
      return null;

    return RnGoLinkDto.FromEntity(
      await _linkRepo.GetByUrl(url)
    );
  }

  public async Task<string> StoreLink(string url)
  {
    // TODO: [LinkStorageService.StoreLink] (TESTS) Add tests
    var linkId = _nextLinkId++;
    var shortCode = _stringHelper.GenerateLinkString(linkId);
    var linkEntity = new LinkEntity(url, shortCode);
      
    // Add the link to the DB
    var rowCount = await _linkRepo.AddLink(linkEntity);
    if (rowCount <= 0)
    {
      _logger.LogError("Failed to store link: {url}", url);
      return string.Empty;
    }

    // Fetch the generated link from the DB
    var dbLink = await _linkRepo.GetById(linkId);
    if (dbLink is not null)
      return dbLink.ShortCode;

    // Failed to store the DB link
    _logger.LogError("Failed to store link: {url}", url);
    return string.Empty;
  }

  public async Task<LinkEntity?> GetByShortCode(string shortCode)
  {
    // TODO: [LinkStorageService.GetByShortCode] (TESTS) Add tests
    return await _linkRepo.GetByShortCode(shortCode);
  }

  public async Task<long> GetLinkCount()
  {
    // TODO: [LinkStorageService.GetLinkCount] (TESTS) Add tests
    var countEntity = await _linkRepo.GetMaxLinkId();

    return countEntity?.CountLong ?? 0;
  }


  // Internal methods
  private long GetNextLinkId()
  {
    // TODO: [LinkStorageService.GetNextLinkId] (TESTS) Add tests
    var countEntity = _linkRepo.GetMaxLinkId().GetAwaiter().GetResult();
    if (countEntity is null)
      return 1;

    var nextLinkId = countEntity.CountLong;
    if (nextLinkId <= 0)
      return 1;

    return nextLinkId + 1;
  }
}