namespace RnGo.Core.Entities;

public class LinkEntity
{
  public long LinkId { get; set; }
  public bool Deleted { get; set; }
  public DateTime DateAddedUtc { get; set; } = DateTime.UtcNow;
  public DateTime DateLastFollowedUtc { get; set; } = DateTime.UtcNow;
  public int FollowCount { get; set; }
  public string ShortCode { get; set; } = string.Empty;
  public string Url { get; set; } = string.Empty;

  public LinkEntity() { }

  public LinkEntity(string url, string shortCode)
  {
    Url = url;
    ShortCode = shortCode;
  }
}
