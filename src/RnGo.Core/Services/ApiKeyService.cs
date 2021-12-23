using Microsoft.Extensions.Logging;
using RnGo.Core.Configuration;
using RnGo.Core.Providers;

namespace RnGo.Core.Services
{
  public interface IApiKeyService
  {
    Task<bool> IsValidApiKey(string apiKey);
  }

  public class ApiKeyService : IApiKeyService
  {
    private readonly ILogger<ApiKeyService> _logger;
    private readonly List<string> _apiKeys;
    private readonly RnGoConfig _config;
    private int _apiKeyCount;

    public ApiKeyService(
      ILogger<ApiKeyService> logger,
      IRnGoConfigProvider configProvider)
    {
      // TODO: [ApiKeyService] (TESTS) Add tests
      _logger = logger;

      _config = configProvider.Provide();
      _apiKeys = new List<string>();
      _apiKeyCount = 0;

      LoadApiKeys();
    }


    // Interface methods
    public async Task<bool> IsValidApiKey(string apiKey)
    {
      // TODO: [ApiKeyService.IsValidApiKey] (TESTS) Add tests
      await Task.CompletedTask;
      if (_apiKeyCount == 0)
        return false;

      var upperKey = apiKey.ToUpper();
      var isValid = _apiKeys.Any(x => x.Equals(upperKey));

      if (!isValid)
      {
        _logger.LogWarning("Invalid API provided: {apiKey}", apiKey);
        return false;
      }

      return true;
    }


    // Internal methods
    private void LoadApiKeys()
    {
      // TODO: [ApiKeyService.LoadApiKeys] (TESTS) Add tests
      // Will be extended out to revoke keys in the future
      _apiKeys.Clear();
      _apiKeys.AddRange(_config.ApiKeys.Select(x => x.ToUpper()));
      _apiKeyCount = _apiKeys.Count;
    }
  }
}
