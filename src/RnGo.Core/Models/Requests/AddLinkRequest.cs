using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace RnGo.Core.Models;

public class AddLinkRequest
{
  [JsonProperty("apiKey"), JsonPropertyName("apiKey")]
  public string ApiKey { get; set; } = string.Empty;

  [JsonProperty("url"), JsonPropertyName("url")]
  public string Url { get; set; } = string.Empty;
}
