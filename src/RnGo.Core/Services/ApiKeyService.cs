using Microsoft.Extensions.Logging;
using RnGo.Core.Configuration;
using RnGo.Core.Providers;
using RnGo.Core.Repos;

namespace RnGo.Core.Services;

public interface IApiKeyService
{
  Task<bool> IsValidApiKey(string apiKey);
}

public class ApiKeyService : IApiKeyService
{
  public List<string> ApiKeys { get; }
  private readonly ILogger<ApiKeyService> _logger;
  private readonly IApiKeyRepo _apiKeyRepo;
  private readonly RnGoConfig _config;
  private int _apiKeyCount;

  public ApiKeyService(
    ILogger<ApiKeyService> logger,
    IRnGoConfigProvider configProvider,
    IApiKeyRepo apiKeyRepo)
  {
    _logger = logger;
    _apiKeyRepo = apiKeyRepo;

    _config = configProvider.Provide();
    ApiKeys = new List<string>();
    _apiKeyCount = 0;

    RefreshApiKeys();
  }


  // Public methods
  public async Task<bool> IsValidApiKey(string apiKey)
  {
    await Task.CompletedTask;
    if (_apiKeyCount == 0)
      return false;

    var upperKey = apiKey.ToUpper();
    var isValid = ApiKeys.Any(x => x.Equals(upperKey));

    if (!isValid)
    {
      _logger.LogWarning("Invalid API provided: {apiKey}", apiKey);
      return false;
    }

    return true;
  }

  public void RefreshApiKeys()
  {
    // Will be extended out to revoke keys in the future
    ApiKeys.Clear();

    LoadConfigApiKeys();
    LoadDatabaseApiKeys();

    _apiKeyCount = ApiKeys.Count;
    _logger.LogInformation("Loaded {count} enabled API keys", _apiKeyCount);
  }


  // Internal methods
  private void LoadConfigApiKeys()
  {
    var apiKeys = _config.ApiKeys
      .Select(x => x.ToUpper())
      .ToList();

    if(apiKeys.Count == 0)
      return;

    _logger.LogDebug("Loaded {count} API keys from config", apiKeys.Count);
    ApiKeys.AddRange(apiKeys);
  }

  private void LoadDatabaseApiKeys()
  {
    var apiKeys = _apiKeyRepo
      .GetEnabledApiKeys()
      .GetAwaiter()
      .GetResult();

    if(apiKeys.Count == 0)
      return;
      
    _logger.LogDebug("Loaded {count} API keys from the DB", apiKeys.Count);
    ApiKeys.AddRange(apiKeys.Select(x => x.ApiKey));
  }
}
