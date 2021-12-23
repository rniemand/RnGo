using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace RnGo.Core.Models.Responses
{
  public class AddLinkResponse
  {
    [JsonProperty("success"), JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonProperty("messages"), JsonPropertyName("messages")]
    public string[] Messages { get; set; }

    [JsonProperty("shortCode"), JsonPropertyName("shortCode")]
    public string ShortCode { get; set; }

    public AddLinkResponse()
    {
      // TODO: [AddLinkResponse] (TESTS) Add tests
      Success = false;
      Messages = Array.Empty<string>();
      ShortCode = string.Empty;
    }

    public AddLinkResponse WithFailure(string message)
    {
      // TODO: [AddLinkResponse] (TESTS) Add tests
      Success = false;
      Messages = new[] {message};
      return this;
    }

    public AddLinkResponse WithSuccess(string shortCode)
    {
      // TODO: [AddLinkResponse] (TESTS) Add tests
      Success = true;
      ShortCode = shortCode;

      return this;
    }
  }
}
