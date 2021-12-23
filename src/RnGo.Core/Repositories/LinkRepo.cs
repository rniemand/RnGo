using Rn.NetCore.DbCommon;
using RnGo.Core.Entities;
using RnGo.Core.RepoQueries;

namespace RnGo.Core.Repositories
{
  public interface ILinkRepo
  {
    Task<int> AddLink(LinkEntity entity);
  }

  public class LinkRepo : BaseRepo<LinkRepo>, ILinkRepo
  {
    private readonly ILinkRepoQueries _queries;

    public LinkRepo(IServiceProvider serviceProvider, ILinkRepoQueries queries)
      : base(serviceProvider)
    {
      // TODO: [LinkRepo] (TESTS) Add tests
      _queries = queries;
    }

    // Interface methods
    public async Task<int> AddLink(LinkEntity entity)
    {
      // TODO: [LinkRepo.AddLink] (TESTS) Add tests
      return await ExecuteAsync(
        nameof(AddLink),
        _queries.AddLink(),
        entity
      );
    }
  }
}
