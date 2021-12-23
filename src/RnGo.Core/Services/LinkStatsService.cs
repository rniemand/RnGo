using Microsoft.Extensions.Logging;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Helpers;
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
    private readonly IPathAbstraction _path;
    private readonly IFileAbstraction _file;
    private readonly IDateTimeAbstraction _dateTime;
    private readonly IJsonHelper _jsonHelper;
    private readonly RnGoConfig _config;
    private readonly Dictionary<string, LinkStats> _stats;

    public LinkStatsService(
      ILogger<LinkStatsService> logger,
      IRnGoConfigProvider configProvider,
      IDirectoryAbstraction directory,
      IPathAbstraction path,
      IFileAbstraction file,
      IDateTimeAbstraction dateTime,
      IJsonHelper jsonHelper)
    {
      // TODO: [LinkStatsService] (TESTS) Add tests
      _logger = logger;
      _directory = directory;
      _path = path;
      _file = file;
      _dateTime = dateTime;
      _jsonHelper = jsonHelper;

      _config = configProvider.Provide();
      _stats = new Dictionary<string, LinkStats>();

      EnsureDirectoriesExist();
    }


    // Interface methods
    public async Task RecordLinkFollow(RnGoLink link)
    {
      // TODO: [LinkStatsService.RecordLinkFollow] (TESTS) Add tests
      await Task.CompletedTask;

      var filePath = GenerateStatsFilePath(link);
      var statsEntry = GetStatsEntry(link, filePath);

      statsEntry.LastCallUtc = _dateTime.UtcNow;
      statsEntry.HitCount += 1;

      SaveStatsEntry(statsEntry, filePath);
    }


    // Internal methods
    private void EnsureDirectoriesExist()
    {
      // TODO: [LinkStatsService.EnsureDirectoriesExist] (TESTS) Add tests
      var statsDir = _config.StatsDirectory;

      if (!_directory.Exists(statsDir))
        _directory.CreateDirectory(statsDir);

      if (!_directory.Exists(statsDir))
        throw new Exception($"Unable to create directory: {statsDir}");

      _logger.LogInformation("Stats dir set to: {path}", statsDir);
    }

    private string GenerateStatsFilePath(RnGoLink link)
    {
      // TODO: [LinkStatsService.GenerateStatsFilePath] (TESTS) Add tests
      return _path.GetFullPath(
        _path.Combine(
          _config.StatsDirectory,
          $"{link.ShortCode}.json".ToLower()
        ));
    }

    private void EnsureStatsFileExists(RnGoLink link, string filePath)
    {
      // TODO: [LinkStatsService.EnsureStatsFileExists] (TESTS) Add tests
      if(_file.Exists(filePath))
        return;

      var statsJson = _jsonHelper.SerializeObject(new LinkStats
      {
        FirstCallUtc = _dateTime.UtcNow,
        HitCount = 0,
        LastCallUtc = _dateTime.UtcNow,
        LinkId = link.LinkId,
        Url = link.Url,
        ShortCode = link.ShortCode
      }, true);

      _file.WriteAllText(filePath, statsJson);
    }
    
    private LinkStats GetStatsEntry(RnGoLink link, string filePath)
    {
      // TODO: [LinkStatsService.GetStatsEntry] (TESTS) Add tests
      EnsureStatsFileExists(link, filePath);
      var statsKey = link.ShortCode.ToUpper();

      if (_stats.ContainsKey(statsKey))
        return _stats[statsKey];

      var statsJson = _file.ReadAllText(filePath);
      var loadedStats = _jsonHelper.DeserializeObject<LinkStats>(statsJson);
      _stats[statsKey] = loadedStats;

      return _stats[statsKey];
    }

    private void SaveStatsEntry(LinkStats linkStats, string filePath)
    {
      // TODO: [LinkStatsService.SaveStatsEntry] (TESTS) Add tests
      if(_file.Exists(filePath))
        _file.Delete(filePath);

      var linkStatsJson = _jsonHelper.SerializeObject(linkStats, true);
      _file.WriteAllText(filePath, linkStatsJson);
    }
  }
}
