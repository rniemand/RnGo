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

      _links = new Dictionary<string, ResolvedLink>();
      _config = configProvider.Provide();
      _storageFilePath = "{root}links.store.json"
        .Replace("{root}", _config.StorageDirectory);

      InitializeStorage();
    }

    public async Task<ResolvedLink?> GetByUrl(string url)
    {
      // TODO: [LinkStorageService.GetByUrl] (TESTS) Add tests
      if (string.IsNullOrWhiteSpace(url))
        return null;

      await Task.CompletedTask;
      return null;
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
        _links[_stringHelper.GenerateLinkString(link.LinkId)] = link;
      }
    }
  }
}
