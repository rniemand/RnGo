namespace RnGo.Core.Entities;

public class ApiKeyEntity
{
  public int ApiKeyId { get; set; }
  public DateTime DateAddedUtc { get; set; }
  public bool Deleted { get; set; }
  public bool Enabled { get; set; }
  public string ApiKey { get; set; }

  public ApiKeyEntity()
  {
    // TODO: [ApiKeyEntity] (TESTS) Add tests
    ApiKeyId = 0;
    DateAddedUtc = DateTime.UtcNow;
    Deleted = false;
    Enabled = true;
    ApiKey = string.Empty;
  }
}