namespace RnGo.Core.RepoQueries
{
  public interface ILinkRepoQueries
  {
    string AddLink();
    string GetByUrl();
    string GetMaxLinkId();
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

    public string GetByUrl()
    {
      return @"SELECT *
      FROM `Links` l
      WHERE
	      l.`Deleted` = 0 AND
	      l.`Url` = @Url";
    }

    public string GetMaxLinkId()
    {
      return @"SELECT MAX(`LinkId`) AS 'CountLong' FROM `Links`";
    }
  }
}
