using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace RnGo.Core.Models
{
  public class AddLinkRequest
  {
    [JsonProperty("apiKey"), JsonPropertyName("apiKey")]
    public string ApiKey { get; set; }
    
    [JsonProperty("url"), JsonPropertyName("url")]
    public string Url { get; set; }

    public AddLinkRequest()
    {
      // TODO: [AddLinkRequest] (TESTS) Add tests
      ApiKey = string.Empty;
      Url = string.Empty;
    }
  }
}
