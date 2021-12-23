using Microsoft.Extensions.Logging;
using RnGo.Core.Models;

namespace RnGo.Core.Services
{
  public interface ILinkStatsService
  {
    Task RecordLinkFollow(RnGoLink link);
  }

  public class LinkStatsService : ILinkStatsService
  {
    private readonly ILogger<LinkStatsService> _logger;

    public LinkStatsService(
      ILogger<LinkStatsService> logger)
    {
      // TODO: [LinkStatsService] (TESTS) Add tests
      _logger = logger;
    }

    public async Task RecordLinkFollow(RnGoLink link)
    {
      // TODO: [LinkStatsService.RecordLinkFollow] (TESTS) Add tests
      await Task.CompletedTask;

      Console.WriteLine("");
    }
  }
}
