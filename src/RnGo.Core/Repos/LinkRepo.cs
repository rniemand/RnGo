using Rn.NetCore.DbCommon;
using RnGo.Core.Entities;
using RnGo.Core.RepoQueries;

namespace RnGo.Core.Repos;

public interface ILinkRepo
{
  Task<int> AddAsync(LinkEntity entity);
  Task<LinkEntity?> GetByUrlAsync(string url);
  Task<GenericCountEntity?> GetMaxLinkIdAsync();
  Task<LinkEntity?> GetByIdAsync(long linkId);
  Task<LinkEntity?> GetByShortCodeAsync(string shortCode);
  Task<int> UpdateFollowCountAsync(long linkId);
}

public class LinkRepo : BaseRepo<LinkRepo>, ILinkRepo
{
  private readonly ILinkRepoQueries _queries;

  public LinkRepo(IBaseRepoHelper baseRepoHelper, ILinkRepoQueries queries)
    : base(baseRepoHelper)
  {
    _queries = queries;
  }

  // Interface methods
  public Task<int> AddAsync(LinkEntity entity) =>
    ExecuteAsync(nameof(AddAsync), _queries.Add(), entity);

  public Task<LinkEntity?> GetByUrlAsync(string url) =>
    GetSingle<LinkEntity>(nameof(GetByUrlAsync), _queries.GetByUrl(), new
    {
      Url = url
    });

  public Task<GenericCountEntity?> GetMaxLinkIdAsync() =>
    GetSingle<GenericCountEntity>(nameof(GetMaxLinkIdAsync), _queries.GetMaxLinkId());

  public Task<LinkEntity?> GetByIdAsync(long linkId) =>
    GetSingle<LinkEntity>(nameof(GetByIdAsync), _queries.GetById(), new
    {
      LinkId = linkId
    });

  public Task<LinkEntity?> GetByShortCodeAsync(string shortCode) =>
    GetSingle<LinkEntity>(nameof(GetByShortCodeAsync), _queries.GetByShortCode(), new
    {
      ShortCode = shortCode
    });

  public Task<int> UpdateFollowCountAsync(long linkId) =>
    ExecuteAsync(nameof(UpdateFollowCountAsync), _queries.UpdateFollowCount(), new
    {
      LinkId = linkId
    });
}
