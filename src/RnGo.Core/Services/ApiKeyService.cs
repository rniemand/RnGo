using Microsoft.Extensions.Logging;
using Rn.NetCore.Common.Abstractions;
using RnGo.Core.Repos;

namespace RnGo.Core.Services;

public interface IApiKeyService
{
  Task<bool> IsValidApiKey(string apiKey);
}

public class ApiKeyService : IApiKeyService
{
  public List<string> ApiKeys { get; } = new();
  private readonly ILogger<ApiKeyService> _logger;
  private readonly IApiKeyRepo _apiKeyRepo;
  private readonly IDateTimeAbstraction _dateTime;
  private DateTime _nextRefreshTime = DateTime.MinValue;

  public ApiKeyService(
    ILogger<ApiKeyService> logger,
    IApiKeyRepo apiKeyRepo,
    IDateTimeAbstraction dateTime)
  {
    _logger = logger;
    _apiKeyRepo = apiKeyRepo;
    _dateTime = dateTime;
  }


  // Public methods
  public async Task<bool> IsValidApiKey(string apiKey)
  {
    await RefreshApiKeys();
    if (ApiKeys.Count == 0)
      return false;

    var upperKey = apiKey.ToUpper();
    var isValid = ApiKeys.Any(x => x.Equals(upperKey));

    if (isValid)
      return true;

    _logger.LogWarning("Invalid API provided: {apiKey}", apiKey);
    return false;
  }

  public async Task RefreshApiKeys()
  {
    if (_dateTime.Now < _nextRefreshTime)
      return;

    _nextRefreshTime = _dateTime.Now.AddMinutes(10);

    // Will be extended out to revoke keys in the future
    ApiKeys.Clear();

    var dbApiKeys = await _apiKeyRepo.GetEnabledAsync();
    ApiKeys.AddRange(dbApiKeys.Select(x => x.ApiKey));

    _logger.LogInformation("Loaded {count} enabled API keys", ApiKeys.Count);
  }
}
