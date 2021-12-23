using Microsoft.Extensions.Logging;
using RnGo.Core.Entities;
using RnGo.Core.Repositories;

namespace RnGo.Core.Services
{
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
      // TODO: [LinkStatsService] (TESTS) Add tests
      _logger = logger;
      _linkRepo = linkRepo;
    }


    // Interface methods
    public async Task RecordLinkFollow(LinkEntity link)
    {
      // TODO: [LinkStatsService.RecordLinkFollow] (TESTS) Add tests
      await _linkRepo.UpdateFollowCount(link.LinkId);
    }
  }
}
