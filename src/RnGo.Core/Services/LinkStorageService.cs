using RnGo.Core.Models;

namespace RnGo.Core.Services
{
  public interface ILinkStorageService
  {
    Task<ResolvedLink?> GetByUrl(string url);
  }

  public class LinkStorageService : ILinkStorageService
  {
    public async Task<ResolvedLink?> GetByUrl(string url)
    {
      // TODO: [LinkStorageService.GetByUrl] (TESTS) Add tests
      if (string.IsNullOrWhiteSpace(url))
        return null;

      await Task.CompletedTask;
      return null;
    }
  }
}
