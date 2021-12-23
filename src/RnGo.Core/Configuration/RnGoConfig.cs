using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace RnGo.Core.Configuration
{
  public class RnGoConfig
  {
    public const string Key = "RnGo";

    [JsonProperty("RootDirectory"), JsonPropertyName("RootDirectory")]
    public string RootDirectory { get; set; }

    [JsonProperty("StorageDirectory"), JsonPropertyName("StorageDirectory")]
    public string StorageDirectory { get; set; }

    [JsonProperty("StatsDirectory"), JsonPropertyName("StatsDirectory")]
    public string StatsDirectory { get; set; }

    [JsonProperty("ApiKeys"), JsonPropertyName("ApiKeys")]
    public string[] ApiKeys { get; set; }

    public RnGoConfig()
    {
      // TODO: [RnGoConfig] (TESTS) Add tests
      RootDirectory = "./";
      StorageDirectory = "{root}storage";
      StatsDirectory = "{storage}stats";
      ApiKeys = Array.Empty<string>();
    }
  }
}
