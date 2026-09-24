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

  public Task<int> AddAsync(string apiKey) =>
    ExecuteAsync(nameof(AddAsync), _queries.Add(), new ApiKeyEntity
    {
      ApiKey = apiKey
    });

  public Task<ApiKeyEntity?> GetAsync(string apiKey) =>
    GetSingle<ApiKeyEntity>(nameof(GetAsync), _queries.Get(), new ApiKeyEntity
    {
      ApiKey = apiKey
    });

  public Task<List<ApiKeyEntity>> GetEnabledAsync() =>
    GetList<ApiKeyEntity>(nameof(GetEnabledAsync), _queries.GetEnabled());
}
