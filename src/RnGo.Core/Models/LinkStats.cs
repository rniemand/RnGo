using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace RnGo.Core.Models
{
  public class LinkStats
  {
    [JsonProperty("linkId"), JsonPropertyName("linkId")]
    public long LinkId { get; set; }

    [JsonProperty("shortCode"), JsonPropertyName("shortCode")]
    public string ShortCode { get; set; }

    [JsonProperty("url"), JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonProperty("hitCount"), JsonPropertyName("hitCount")]
    public long HitCount { get; set; }

    [JsonProperty("firstCallUtc"), JsonPropertyName("firstCallUtc")]
    public DateTime FirstCallUtc { get; set; }

    [JsonProperty("lastCallUtc"), JsonPropertyName("lastCallUtc")]
    public DateTime LastCallUtc { get; set; }

    public LinkStats()
    {
      // TODO: [LinkStats] (TESTS) Add tests
      LinkId = 0;
      ShortCode = string.Empty;
      Url = string.Empty;
      HitCount = 0;
      FirstCallUtc = DateTime.UtcNow;
      LastCallUtc = DateTime.UtcNow;
    }
  }
}
