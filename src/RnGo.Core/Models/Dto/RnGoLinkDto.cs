using Newtonsoft.Json;

namespace RnGo.Core.Models.Dto;

public class RnGoLinkDto
{
  [JsonProperty("url")]
  public string Url { get; set; } = string.Empty;

  [JsonProperty("linkId")]
  public long LinkId { get; set; }

  [JsonProperty("shortCode")]
  public string ShortCode { get; set; } = string.Empty;
}
