using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace RnGo.Core.Models;

public class RnGoLink
{
  [JsonProperty("u"), JsonPropertyName("u")]
  public string Url { get; set; } = string.Empty;

  [JsonProperty("i"), JsonPropertyName("i")]
  public long LinkId { get; set; }

  [JsonProperty("s"), JsonPropertyName("s")]
  public string ShortCode { get; set; } = string.Empty;

  public RnGoLink(string url, long linkId, string shortCode)
  {
    Url = url;
    LinkId = linkId;
    ShortCode = shortCode;
  }
}
