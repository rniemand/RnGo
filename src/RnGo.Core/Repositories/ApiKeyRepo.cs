using Rn.NetCore.DbCommon;
using RnGo.Core.Entities;
using RnGo.Core.RepoQueries;

namespace RnGo.Core.Repositories
{
  public interface IApiKeyRepo
  {
    Task<int> Add(string apiKey);
    Task<ApiKeyEntity?> GetByApiKey(string apiKey);
  }

  public class ApiKeyRepo : BaseRepo<ApiKeyRepo>, IApiKeyRepo
  {
    private readonly IApiKeyRepoQueries _queries;

    public ApiKeyRepo(IServiceProvider serviceProvider, IApiKeyRepoQueries queries)
      : base(serviceProvider)
    {
      // TODO: [ApiKeyRepo] (TESTS) Add tests
      _queries = queries;

    }

    public async Task<int> Add(string apiKey)
    {
      // TODO: [ApiKeyRepo.Add] (TESTS) Add tests
      return await ExecuteAsync(
        nameof(Add),
        _queries.Add(),
        new {ApiKey = apiKey}
      );
    }

    public async Task<ApiKeyEntity?> GetByApiKey(string apiKey)
    {
      // TODO: [ApiKeyRepo.GetByApiKey] (TESTS) Add tests
      return await GetSingle<ApiKeyEntity>(
        nameof(GetByApiKey),
        _queries.GetByApiKey(),
        new {ApiKey = apiKey}
      );
    }
  }
}
