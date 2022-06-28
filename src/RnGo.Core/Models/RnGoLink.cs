using Newtonsoft.Json;

namespace RnGo.Core.Models;

public class RnGoLink
{
  [JsonProperty("u")]
  public string Url { get; set; } = string.Empty;

  [JsonProperty("i")]
  public long LinkId { get; set; }

  [JsonProperty("s")]
  public string ShortCode { get; set; } = string.Empty;

  public RnGoLink() { }

  public RnGoLink(string url, long linkId, string shortCode)
  {
    Url = url;
    LinkId = linkId;
    ShortCode = shortCode;
  }
}
