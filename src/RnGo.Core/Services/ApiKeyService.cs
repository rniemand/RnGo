using Microsoft.Extensions.Logging;
using RnGo.Core.Configuration;
using RnGo.Core.Providers;
using RnGo.Core.Repositories;

namespace RnGo.Core.Services
{
  public interface IApiKeyService
  {
    Task<bool> IsValidApiKey(string apiKey);
  }

  public class ApiKeyService : IApiKeyService
  {
    private readonly ILogger<ApiKeyService> _logger;
    private readonly IApiKeyRepo _apiKeyRepo;
    private readonly List<string> _apiKeys;
    private readonly RnGoConfig _config;
    private int _apiKeyCount;

    public ApiKeyService(
      ILogger<ApiKeyService> logger,
      IRnGoConfigProvider configProvider,
      IApiKeyRepo apiKeyRepo)
    {
      // TODO: [ApiKeyService] (TESTS) Add tests
      _logger = logger;
      _apiKeyRepo = apiKeyRepo;

      _config = configProvider.Provide();
      _apiKeys = new List<string>();
      _apiKeyCount = 0;

      RefreshApiKeys();
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
    private void RefreshApiKeys()
    {
      // TODO: [ApiKeyService.RefreshApiKeys] (TESTS) Add tests
      // Will be extended out to revoke keys in the future
      _apiKeys.Clear();

      LoadConfigApiKeys();
      LoadDatabaseApiKeys();

      _apiKeyCount = _apiKeys.Count;
      _logger.LogInformation("Loaded {count} enabled API keys", _apiKeyCount);
    }

    private void LoadConfigApiKeys()
    {
      // TODO: [ApiKeyService.LoadConfigApiKeys] (TESTS) Add tests
      var apiKeys = _config.ApiKeys
        .Select(x => x.ToUpper())
        .ToList();

      if(apiKeys.Count == 0)
        return;

      _logger.LogDebug("Loaded {count} API keys from config", apiKeys.Count);
      _apiKeys.AddRange(apiKeys);
    }

    private void LoadDatabaseApiKeys()
    {
      // TODO: [ApiKeyService.LoadDatabaseApiKeys] (TESTS) Add tests
      var apiKeys = _apiKeyRepo
        .GetEnabledApiKeys()
        .GetAwaiter()
        .GetResult();

      if(apiKeys.Count == 0)
        return;
      
      _logger.LogDebug("Loaded {count} API keys from the DB", apiKeys.Count);
      _apiKeys.AddRange(apiKeys.Select(x => x.ApiKey));
    }
  }
}
