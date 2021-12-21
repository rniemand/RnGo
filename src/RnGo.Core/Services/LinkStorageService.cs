using Microsoft.Extensions.Logging;
using RnGo.Core.Configuration;
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

    public LinkStorageService(
      ILogger<LinkStorageService> logger,
      IRnGoConfigProvider configProvider)
    {
      // TODO: [LinkStorageService] (TESTS) Add tests
      _logger = logger;
      _config = configProvider.Provide();

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




    }
  }
}
