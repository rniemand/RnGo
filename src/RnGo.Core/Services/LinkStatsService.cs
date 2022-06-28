using Microsoft.Extensions.Logging;
using RnGo.Core.Entities;
using RnGo.Core.Repos;

namespace RnGo.Core.Services;

public interface ILinkStatsService
{
  Task RecordLinkFollow(LinkEntity link);
}

public class LinkStatsService : ILinkStatsService
{
  private readonly ILogger<LinkStatsService> _logger;
  private readonly ILinkRepo _linkRepo;

  public LinkStatsService(
    ILogger<LinkStatsService> logger,
    ILinkRepo linkRepo)
  {
    _logger = logger;
    _linkRepo = linkRepo;
  }


  // Interface methods
  public async Task RecordLinkFollow(LinkEntity link) =>
    await _linkRepo.UpdateFollowCountAsync(link.LinkId);
}
