using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace RnGo.Core.Models;

public class RnGoLink
{
  [JsonProperty("u"), JsonPropertyName("u")]
  public string Url { get; set; }

  [JsonProperty("i"), JsonPropertyName("i")]
  public long LinkId { get; set; }

  [JsonProperty("s"), JsonPropertyName("s")]
  public string ShortCode { get; set; }

  public RnGoLink()
  {
    // TODO: [RnGoLink] (TESTS) Add tests
    Url = string.Empty;
    LinkId = 0;
    ShortCode = string.Empty;
  }

  public RnGoLink(string url, long linkId, string shortCode)
    : this()
  {
    // TODO: [RnGoLink] (TESTS) Add tests
    Url = url;
    LinkId = linkId;
    ShortCode = shortCode;
  }
}