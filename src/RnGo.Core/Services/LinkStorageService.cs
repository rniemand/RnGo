using Microsoft.Extensions.Logging;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Helpers;
using RnGo.Core.Configuration;
using RnGo.Core.Helpers;
using RnGo.Core.Models;
using RnGo.Core.Providers;

namespace RnGo.Core.Services
{
  public interface ILinkStorageService
  {
    Task<ResolvedLink?> GetByUrl(string url);
    Task<string> StoreLink(ResolvedLink link);
    Task<ResolvedLink?> GetByShortCode(string shortCode);
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
    private readonly string _storageFilePath;
    private readonly Dictionary<string, ResolvedLink> _links;
    private long _nextLinkId;

    public LinkStorageService(
      ILogger<LinkStorageService> logger,
      IRnGoConfigProvider configProvider,
      IDirectoryAbstraction directory,
      IFileAbstraction file,
      IJsonHelper jsonHelper,
      IStringHelper stringHelper)
    {
      // TODO: [LinkStorageService] (TESTS) Add tests
      _logger = logger;
      _directory = directory;
      _file = file;
      _jsonHelper = jsonHelper;
      _stringHelper = stringHelper;
      _nextLinkId = 0;

      _links = new Dictionary<string, ResolvedLink>();
      _config = configProvider.Provide();

      _storageFilePath = "{root}links.store.json"
        .Replace("{root}", _config.StorageDirectory);

      _logger.LogInformation("Setting data file path to: {path}", _storageFilePath);

      InitializeStorage();
    }

    public async Task<ResolvedLink?> GetByUrl(string url)
    {
      // TODO: [LinkStorageService.GetByUrl] (TESTS) Add tests
      if (string.IsNullOrWhiteSpace(url))
        return null;

      if (_links.Count == 0)
        return null;

      await Task.CompletedTask;
      var (_, value) = _links
        .FirstOrDefault(x => x.Value.Url.Equals(url));

      return value ?? null;
    }

    public async Task<string> StoreLink(ResolvedLink link)
    {
      // TODO: [LinkStorageService.StoreLink] (TESTS) Add tests
      link.LinkId = _nextLinkId++;
      var shortCode = _stringHelper.GenerateLinkString(link.LinkId);
      link.ShortCode = shortCode;
      
      _links[shortCode.ToUpper()] = link;
      SaveLinks();

      await Task.CompletedTask;
      return shortCode;
    }

    public async Task<ResolvedLink?> GetByShortCode(string shortCode)
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
      var links = new List<ResolvedLink>();
      var linksJson = _jsonHelper.SerializeObject(links, true);
      _file.WriteAllText(_storageFilePath, linksJson);
    }

    private void LoadStorageFile()
    {
      // TODO: [LinkStorageService.LoadStorageFile] (TESTS) Add tests
      if (!_file.Exists(_storageFilePath))
        CreateInitialStorageFile();

      if (!_file.Exists(_storageFilePath))
        throw new Exception($"Unable to load storage file: {_storageFilePath}");

      var fileJson = _file.ReadAllText(_storageFilePath);
      var fileLinks = _jsonHelper.DeserializeObject<List<ResolvedLink>>(fileJson);
      
      _links.Clear();
      foreach (var link in fileLinks)
      {
        if (string.IsNullOrWhiteSpace(link.ShortCode))
          link.ShortCode = _stringHelper.GenerateLinkString(link.LinkId);

        _links[link.ShortCode.ToUpper()] = link;
        if (link.LinkId > _nextLinkId) _nextLinkId = link.LinkId;
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
