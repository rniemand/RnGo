using Microsoft.Extensions.Logging;

namespace RnGo.Core.Services
{
  public interface IApiKeyService
  {
    Task<bool> IsValidApiKey(string key);
  }

  public class ApiKeyService : IApiKeyService
  {
    private readonly ILogger<ApiKeyService> _logger;

    public ApiKeyService(
      ILogger<ApiKeyService> logger)
    {
      // TODO: [ApiKeyService] (TESTS) Add tests
      _logger = logger;
    }

    public async Task<bool> IsValidApiKey(string key)
    {
      // TODO: [ApiKeyService.IsValidApiKey] (TESTS) Add tests

      await Task.CompletedTask;

      return false;
    }
  }
}
