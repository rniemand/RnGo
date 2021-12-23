using Rn.NetCore.DbCommon;

namespace RnGo.Core.Repositories
{
  public interface ILinkRepo
  {
  }

  public class LinkRepo : BaseRepo<LinkRepo>, ILinkRepo
  {
    public LinkRepo(IServiceProvider serviceProvider)
      : base(serviceProvider)
    {
      // TODO: [LinkRepo] (TESTS) Add tests
      Console.WriteLine("");
    }
  }
}
