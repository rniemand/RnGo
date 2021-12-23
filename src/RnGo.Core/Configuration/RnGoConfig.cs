namespace RnGo.Core.Configuration
{
  public class RnGoConfig
  {
    public const string Key = "RnGo";

    public string RootDirectory { get; set; }
    public string StorageDirectory { get; set; }
    public string StatsDirectory { get; set; }

    public RnGoConfig()
    {
      // TODO: [RnGoConfig] (TESTS) Add tests
      RootDirectory = "./";
      StorageDirectory = "{root}storage";
      StatsDirectory = "{storage}stats";
    }
  }
}
