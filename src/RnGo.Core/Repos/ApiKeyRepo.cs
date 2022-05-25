using Rn.NetCore.DbCommon;
using RnGo.Core.Entities;
using RnGo.Core.RepoQueries;

namespace RnGo.Core.Repos;

public interface IApiKeyRepo
{
  Task<int> Add(string apiKey);
  Task<ApiKeyEntity?> GetByApiKey(string apiKey);
  Task<List<ApiKeyEntity>> GetEnabledApiKeys();
}

public class ApiKeyRepo : BaseRepo<ApiKeyRepo>, IApiKeyRepo
{
  private readonly IApiKeyRepoQueries _queries;

  public ApiKeyRepo(IServiceProvider serviceProvider, IApiKeyRepoQueries queries)
    : base(serviceProvider)
  {
    _queries = queries;

  }

  public async Task<int> Add(string apiKey)
  {
    return await ExecuteAsync(
      nameof(Add),
      _queries.Add(),
      new {ApiKey = apiKey}
    );
  }

  public async Task<ApiKeyEntity?> GetByApiKey(string apiKey)
  {
    return await GetSingle<ApiKeyEntity>(
      nameof(GetByApiKey),
      _queries.GetByApiKey(),
      new {ApiKey = apiKey}
    );
  }

  public async Task<List<ApiKeyEntity>> GetEnabledApiKeys()
  {
    return await GetList<ApiKeyEntity>(
      nameof(GetEnabledApiKeys),
      _queries.GetEnabledApiKeys()
    );
  }
}
