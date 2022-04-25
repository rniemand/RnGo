using Rn.NetCore.DbCommon;
using RnGo.Core.Entities;
using RnGo.Core.RepoQueries;

namespace RnGo.Core.Repos;

public interface ILinkRepo
{
  Task<int> AddLink(LinkEntity entity);
  Task<LinkEntity?> GetByUrl(string url);
  Task<GenericCountEntity?> GetMaxLinkId();
  Task<LinkEntity?> GetById(long linkId);
  Task<LinkEntity?> GetByShortCode(string shortCode);
  Task<int> UpdateFollowCount(long linkId);
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

  public async Task<LinkEntity?> GetByUrl(string url)
  {
    // TODO: [LinkRepo.GetByUrl] (TESTS) Add tests
    return await GetSingle<LinkEntity>(
      nameof(GetByUrl),
      _queries.GetByUrl(),
      new { Url = url }
    );
  }

  public async Task<GenericCountEntity?> GetMaxLinkId()
  {
    // TODO: [LinkRepo.GetMaxLinkId] (TESTS) Add tests
    return await GetSingle<GenericCountEntity>(
      nameof(GetMaxLinkId),
      _queries.GetMaxLinkId()
    );
  }

  public async Task<LinkEntity?> GetById(long linkId)
  {
    // TODO: [LinkRepo.GetById] (TESTS) Add tests
    return await GetSingle<LinkEntity>(
      nameof(GetById),
      _queries.GetById(),
      new { LinkId = linkId }
    );
  }

  public async Task<LinkEntity?> GetByShortCode(string shortCode)
  {
    // TODO: [LinkRepo.GetByShortCode] (TESTS) Add tests
    return await GetSingle<LinkEntity>(
      nameof(GetByShortCode),
      _queries.GetByShortCode(),
      new { ShortCode = shortCode }
    );
  }

  public async Task<int> UpdateFollowCount(long linkId)
  {
    // TODO: [LinkRepo.UpdateFollowCount] (TESTS) Add tests
    return await ExecuteAsync(
      nameof(UpdateFollowCount),
      _queries.UpdateFollowCount(),
      new { LinkId = linkId }
    );
  }
}