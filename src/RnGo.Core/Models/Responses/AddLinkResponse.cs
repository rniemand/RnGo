using Newtonsoft.Json;

namespace RnGo.Core.Models.Responses;

public class AddLinkResponse
{
  [JsonProperty("success")]
  public bool Success { get; set; }

  [JsonProperty("messages")]
  public string[] Messages { get; set; } = Array.Empty<string>();

  [JsonProperty("shortCode")]
  public string ShortCode { get; set; } = string.Empty;

  public AddLinkResponse WithFailure(string message)
  {
    Success = false;
    Messages = new[] {message};
    return this;
  }

  public AddLinkResponse WithSuccess(string shortCode)
  {
    Success = true;
    ShortCode = shortCode;
    return this;
  }
}
