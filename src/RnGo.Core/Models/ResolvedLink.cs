using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace RnGo.Core.Models
{
  public class ResolvedLink
  {
    [JsonProperty("u"), JsonPropertyName("u")]
    public string Url { get; set; }

    [JsonProperty("i"), JsonPropertyName("i")]
    public long LinkId { get; set; }

    [JsonProperty("s"), JsonPropertyName("s")]
    public string ShortCode { get; set; }

    public ResolvedLink()
    {
      // TODO: [ResolvedLink] (TESTS) Add tests
      Url = string.Empty;
      LinkId = 0;
      ShortCode = string.Empty;
    }
  }
}
