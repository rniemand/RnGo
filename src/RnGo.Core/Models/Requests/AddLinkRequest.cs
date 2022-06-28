using Newtonsoft.Json;

namespace RnGo.Core.Models;

public class AddLinkRequest
{
  [JsonProperty("apiKey")]
  public string ApiKey { get; set; } = string.Empty;

  [JsonProperty("url")]
  public string Url { get; set; } = string.Empty;
}
