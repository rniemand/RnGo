namespace RnGo.Core.RepoQueries
{
  public interface ILinkRepoQueries
  {
    string AddLink();
  }
  
  public class LinkRepoQueries : ILinkRepoQueries
  {
    public string AddLink()
    {
      return @"INSERT INTO `Links`
	      (`ShortCode`, `Url`)
      VALUES
	      (@ShortCode, @Url)";
    }
  }
}
