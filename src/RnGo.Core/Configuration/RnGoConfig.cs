namespace RnGo.Core.Configuration
{
  public class RnGoConfig
  {
    public const string Key = "RnGo";

    public string StorageDirectory { get; set; }

    public RnGoConfig()
    {
      // TODO: [RnGoConfig] (TESTS) Add tests
      StorageDirectory = "./storage";
    }
  }
}
