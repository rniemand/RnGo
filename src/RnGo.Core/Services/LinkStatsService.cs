using Microsoft.Extensions.Logging;
using Rn.NetCore.Common.Abstractions;
using RnGo.Core.Configuration;
using RnGo.Core.Models;
using RnGo.Core.Providers;

namespace RnGo.Core.Services
{
  public interface ILinkStatsService
  {
    Task RecordLinkFollow(RnGoLink link);
  }

  public class LinkStatsService : ILinkStatsService
  {
    private readonly ILogger<LinkStatsService> _logger;
    private readonly IDirectoryAbstraction _directory;
    private readonly RnGoConfig _config;

    public LinkStatsService(
      ILogger<LinkStatsService> logger,
      IRnGoConfigProvider configProvider,
      IDirectoryAbstraction directory)
    {
      // TODO: [LinkStatsService] (TESTS) Add tests
      _logger = logger;
      _directory = directory;
      _config = configProvider.Provide();

      EnsureDirectoriesExist();
    }


    // Interface methods
    public async Task RecordLinkFollow(RnGoLink link)
    {
      // TODO: [LinkStatsService.RecordLinkFollow] (TESTS) Add tests
      await Task.CompletedTask;

      Console.WriteLine("");
    }


    // Internal methods
    private void EnsureDirectoriesExist()
    {
      // TODO: [LinkStatsService.EnsureDirectoriesExist] (TESTS) Add tests
      var statsDir = _config.StorageDirectory;

      if (!_directory.Exists(statsDir))
        _directory.CreateDirectory(statsDir);

      if (!_directory.Exists(statsDir))
        throw new Exception($"Unable to create directory: {statsDir}");

      _logger.LogInformation("Stats dir set to: {path}", statsDir);
    }
  }
}
