namespace RnGo.Core.RepoQueries
{
  public interface ILinkRepoQueries
  {
    string AddLink();
    string GetByUrl();
    string GetMaxLinkId();
    string GetById();
    string GetByShortCode();
    string UpdateFollowCount();
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

    public string GetById()
    {
      return @"SELECT * FROM `Links` WHERE `LinkId` = @LinkId";
    }

    public string GetByShortCode()
    {
      return @"SELECT *
      FROM `Links` l
      WHERE
	      l.`Deleted` = 0 AND
	      l.`ShortCode` = @ShortCode";
    }

    public string UpdateFollowCount()
    {
      return @"UPDATE `Links` l
      SET
	      l.`FollowCount` = FollowCount + 1,
	      l.`DateLastFollowedUtc` = utc_timestamp(6)
      WHERE
	      l.`LinkId` = @LinkId";
    }
  }
}
