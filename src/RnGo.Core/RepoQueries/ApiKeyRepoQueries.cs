namespace RnGo.Core.RepoQueries;

public interface IApiKeyRepoQueries
{
  string Add();
  string Get();
  string GetEnabled();
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

  public string Get()
  {
    return @"SELECT *
      FROM `ApiKeys` a
      WHERE
	      a.`Deleted` = 0 AND
	      a.`ApiKey` = @ApiKey";
  }

  public string GetEnabled()
  {
    return @"SELECT *
      FROM `ApiKeys` a
      WHERE
	      a.`Deleted` = 0 AND
	      a.`Enabled` = 1";
  }
}