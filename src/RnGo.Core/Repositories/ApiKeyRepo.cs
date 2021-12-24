using Rn.NetCore.DbCommon;
using RnGo.Core.RepoQueries;

namespace RnGo.Core.Repositories
{
  public interface IApiKeyRepo
  {
  }

  public class ApiKeyRepo : BaseRepo<ApiKeyRepo>, IApiKeyRepo
  {
    private readonly IApiKeyRepoQueries _queries;

    public ApiKeyRepo(IServiceProvider serviceProvider, IApiKeyRepoQueries queries)
      : base(serviceProvider)
    {
      _queries = queries;
      // TODO: [ApiKeyRepo] (TESTS) Add tests
    }
  }
}
