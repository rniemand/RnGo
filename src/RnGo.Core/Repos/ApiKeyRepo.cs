using Rn.NetCore.DbCommon;
using RnGo.Core.Entities;
using RnGo.Core.RepoQueries;

namespace RnGo.Core.Repos;

public interface IApiKeyRepo
{
  Task<int> AddAsync(string apiKey);
  Task<ApiKeyEntity?> GetAsync(string apiKey);
  Task<List<ApiKeyEntity>> GetEnabledAsync();
}

public class ApiKeyRepo : BaseRepo<ApiKeyRepo>, IApiKeyRepo
{
  private readonly IApiKeyRepoQueries _queries;

  public ApiKeyRepo(IBaseRepoHelper baseRepoHelper, IApiKeyRepoQueries queries)
    : base(baseRepoHelper)
  {
    _queries = queries;

  }

  public async Task<int> AddAsync(string apiKey) =>
    await ExecuteAsync(nameof(AddAsync), _queries.Add(), new
    {
      ApiKey = apiKey
    });

  public async Task<ApiKeyEntity?> GetAsync(string apiKey) =>
    await GetSingle<ApiKeyEntity>(nameof(GetAsync), _queries.Get(), new
    {
      ApiKey = apiKey
    });

  public async Task<List<ApiKeyEntity>> GetEnabledAsync() =>
    await GetList<ApiKeyEntity>(nameof(GetEnabledAsync), _queries.GetEnabled());
}
