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
  public async Task<int> AddAsync(LinkEntity entity) =>
    await ExecuteAsync(nameof(AddAsync), _queries.Add(), entity);

  public async Task<LinkEntity?> GetByUrlAsync(string url) =>
    await GetSingle<LinkEntity>(nameof(GetByUrlAsync), _queries.GetByUrl(), new
    {
      Url = url
    });

  public async Task<GenericCountEntity?> GetMaxLinkIdAsync() =>
    await GetSingle<GenericCountEntity>(nameof(GetMaxLinkIdAsync), _queries.GetMaxLinkId());

  public async Task<LinkEntity?> GetByIdAsync(long linkId) =>
    await GetSingle<LinkEntity>(nameof(GetByIdAsync), _queries.GetById(), new
    {
      LinkId = linkId
    });

  public async Task<LinkEntity?> GetByShortCodeAsync(string shortCode) =>
    await GetSingle<LinkEntity>(nameof(GetByShortCodeAsync), _queries.GetByShortCode(), new
    {
      ShortCode = shortCode
    });

  public async Task<int> UpdateFollowCountAsync(long linkId) =>
    await ExecuteAsync(nameof(UpdateFollowCountAsync), _queries.UpdateFollowCount(), new
    {
      LinkId = linkId
    });
}
