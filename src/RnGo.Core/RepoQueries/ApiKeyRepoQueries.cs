namespace RnGo.Core.RepoQueries
{
  public interface IApiKeyRepoQueries
  {
    string Add();
    string GetByApiKey();
  }

  public class ApiKeyRepoQueries : IApiKeyRepoQueries
  {
    public string Add()
    {
      return @"INSERT INTO `ApiKeys`
	      (`ApiKey`)
      VALUES
	      (@ApiKey)";
    }

    public string GetByApiKey()
    {
      return @"SELECT *
      FROM `ApiKeys` a
      WHERE
	      a.`Deleted` = 0 AND
	      a.`ApiKey` = @ApiKey";
    }
  }
}
