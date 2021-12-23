using Microsoft.Extensions.Logging;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Helpers;
using RnGo.Core.Configuration;
using RnGo.Core.Entities;
using RnGo.Core.Helpers;
using RnGo.Core.Models;
using RnGo.Core.Models.Dto;
using RnGo.Core.Providers;
using RnGo.Core.Repositories;

namespace RnGo.Core.Services
{
  public interface ILinkStorageService
  {
    Task<RnGoLinkDto?> GetByUrl(string url);
    Task<string> StoreLink(string link);
    Task<RnGoLink?> GetByShortCode(string shortCode);
    Task<int> GetLinkCount();
  }

  public class LinkStorageService : ILinkStorageService
  {
    private readonly ILogger<LinkStorageService> _logger;
    private readonly RnGoConfig _config;
    private readonly IDirectoryAbstraction _directory;
    private readonly IFileAbstraction _file;
    private readonly IJsonHelper _jsonHelper;
    private readonly IStringHelper _stringHelper;
    private readonly ILinkRepo _linkRepo;
    private readonly string _storageFilePath;
    private readonly Dictionary<string, RnGoLink> _links;
    private long _nextLinkId;

    public LinkStorageService(
      ILogger<LinkStorageService> logger,
      IRnGoConfigProvider configProvider,
      IDirectoryAbstraction directory,
      IFileAbstraction file,
      IJsonHelper jsonHelper,
      IStringHelper stringHelper,
      ILinkRepo linkRepo)
    {
      // TODO: [LinkStorageService] (TESTS) Add tests
      _logger = logger;
      _directory = directory;
      _file = file;
      _jsonHelper = jsonHelper;
      _stringHelper = stringHelper;
      _linkRepo = linkRepo;
      _nextLinkId = 0;

      _links = new Dictionary<string, RnGoLink>();
      _config = configProvider.Provide();

      _storageFilePath = "{root}links.store.json"
        .Replace("{root}", _config.StorageDirectory);

      _logger.LogInformation("Setting data file path to: {path}", _storageFilePath);

      InitializeStorage();
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

      var linkEntity = new LinkEntity
      {
        LinkId = linkId,
        ShortCode = shortCode,
        Url = url
      };

      // Add the link to the DB
      var rowCount = await _linkRepo.AddLink(linkEntity);
      if (rowCount <= 0)
      {
        _logger.LogError("Failed to store link: {url}", url);
        return string.Empty;
      }

      // Fetch the generated link from the DB



      var link = new RnGoLink(url, linkId, shortCode);
      
      _links[shortCode.ToUpper()] = link;
      SaveLinks();

      await Task.CompletedTask;
      return shortCode;
    }

    public async Task<RnGoLink?> GetByShortCode(string shortCode)
    {
      // TODO: [LinkStorageService.GetByShortCode] (TESTS) Add tests
      var upperCode = shortCode.ToUpper();
      await Task.CompletedTask;
      return !_links.ContainsKey(upperCode) ? null : _links[upperCode];
    }

    public async Task<int> GetLinkCount()
    {
      // TODO: [LinkStorageService.GetLinkCount] (TESTS) Add tests
      await Task.CompletedTask;
      return _links.Count;
    }


    // Internal methods
    private void InitializeStorage()
    {
      // TODO: [LinkStorageService.InitializeStorage] (TESTS) Add tests
      if (!_directory.Exists(_config.StorageDirectory))
        _directory.CreateDirectory(_config.StorageDirectory);

      if (!_directory.Exists(_config.StorageDirectory))
        throw new Exception($"Unable to find storage dir: {_config.StorageDirectory}");

      LoadStorageFile();
    }

    private void CreateInitialStorageFile()
    {
      // TODO: [LinkStorageService.CreateInitialStorageFile] (TESTS) Add tests
      var links = new List<RnGoLink>();
      var linksJson = _jsonHelper.SerializeObject(links, true);
      _file.WriteAllText(_storageFilePath, linksJson);
    }

    private void LoadStorageFile()
    {
      // TODO: [LinkStorageService.LoadStorageFile] (TESTS) Add tests
      _nextLinkId = 1;
      var countEntity = _linkRepo.GetMaxLinkId().GetAwaiter().GetResult();

      if(countEntity is null)
        return;

      _nextLinkId = countEntity.CountLong;
      if (_nextLinkId <= 0)
      {
        _nextLinkId = 1;
        return;
      }

      _nextLinkId += 1;
    }

    private void BackupLinksFile()
    {
      // TODO: [LinkStorageService.BackupLinksFile] (TESTS) Add tests
      var backupFile = $"{_storageFilePath}.backup";

      if (_file.Exists(backupFile))
        _file.Delete(backupFile);

      _file.Move(_storageFilePath, backupFile);
    }

    private void SaveLinks()
    {
      // TODO: [LinkStorageService.SaveLinks] (TESTS) Add tests
      BackupLinksFile();

      var links = _links
        .Select(link => link.Value)
        .ToList();

      var linksJson = _jsonHelper.SerializeObject(links);
      _file.WriteAllText(_storageFilePath, linksJson);
    }
  }
}
