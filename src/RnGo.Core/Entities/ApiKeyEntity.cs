namespace RnGo.Core.Entities;

public class ApiKeyEntity
{
  public int ApiKeyId { get; set; }
  public DateTime DateAddedUtc { get; set; } = DateTime.UtcNow;
  public bool Deleted { get; set; }
  public bool Enabled { get; set; } = true;
  public string ApiKey { get; set; } = string.Empty;
}
