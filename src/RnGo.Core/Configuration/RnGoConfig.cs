using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace RnGo.Core.Configuration
{
  public class RnGoConfig
  {
    public const string Key = "RnGo";

    [JsonProperty("RootDirectory"), JsonPropertyName("RootDirectory")]
    public string RootDirectory { get; set; }
    
    [JsonProperty("ApiKeys"), JsonPropertyName("ApiKeys")]
    public string[] ApiKeys { get; set; }

    public RnGoConfig()
    {
      // TODO: [RnGoConfig] (TESTS) Add tests
      RootDirectory = "./";
      ApiKeys = Array.Empty<string>();
    }
  }
}
