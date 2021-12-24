namespace RnGo.Core.Entities
{
  public class LinkEntity
  {
    public long LinkId { get; set; }
    public bool Deleted { get; set; }
    public DateTime DateAddedUtc { get; set; }
    public DateTime DateLastFollowedUtc { get; set; }
    public int FollowCount { get; set; }
    public string ShortCode { get; set; }
    public string Url { get; set; }

    public LinkEntity()
    {
      // TODO: [LinkEntity] (TESTS) Add tests
      LinkId = 0;
      Deleted = false;
      DateAddedUtc = DateTime.UtcNow;
      DateLastFollowedUtc = DateTime.UtcNow;
      FollowCount = 0;
      ShortCode = string.Empty;
      Url = string.Empty;
    }

    public LinkEntity(string url, string shortCode)
      : this()
    {
      // TODO: [LinkEntity] (TESTS) Add tests
      Url = url;
      ShortCode = shortCode;
    }
	}
}
