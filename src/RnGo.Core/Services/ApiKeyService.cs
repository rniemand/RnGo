using Microsoft.Extensions.Logging;
using Rn.NetCore.Common.Abstractions;
using RnGo.Core.Repos;

namespace RnGo.Core.Services;

public interface IApiKeyService
{
  Task<bool> IsValidApiKeyAsync(string apiKey);
}

public class ApiKeyService : IApiKeyService
{
  private static readonly TimeSpan RefreshInterval = TimeSpan.FromMinutes(10);

  // Replaced wholesale on refresh (never mutated) so concurrent readers always see a complete set
  private volatile HashSet<string> _apiKeys = new(StringComparer.OrdinalIgnoreCase);
  private readonly ILogger<ApiKeyService> _logger;
  private readonly IApiKeyRepo _apiKeyRepo;
  private readonly IDateTimeAbstraction _dateTime;
  private DateTime _nextRefreshTime = DateTime.MinValue;

  public IReadOnlyCollection<string> ApiKeys => _apiKeys;

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
  public async Task<bool> IsValidApiKeyAsync(string apiKey)
  {
    await RefreshApiKeys();

    var apiKeys = _apiKeys;
    if (apiKeys.Count == 0 || string.IsNullOrEmpty(apiKey))
      return false;

    if (apiKeys.Contains(apiKey))
      return true;

    _logger.LogWarning("Invalid API provided: {apiKey}", apiKey);
    return false;
  }

  public async Task RefreshApiKeys()
  {
    if (_dateTime.Now < _nextRefreshTime)
      return;

    _nextRefreshTime = _dateTime.Now.Add(RefreshInterval);

    // Will be extended out to revoke keys in the future
    var dbApiKeys = await _apiKeyRepo.GetEnabledAsync();
    _apiKeys = new HashSet<string>(dbApiKeys.Select(x => x.ApiKey), StringComparer.OrdinalIgnoreCase);

    _logger.LogInformation("Loaded {count} enabled API keys", _apiKeys.Count);
  }
}
