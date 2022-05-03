using RnGo.Core.Configuration;

namespace RnGo.Core.T1.Tests.TestSupport.Builders;

public class RnGoConfigBuilder
{
  public static RnGoConfig ValidDefault = new RnGoConfigBuilder().BuildWithValidDefaults();
  public const string ValidApiKey = "445BC4CC-DE49-463E-A1AF-089FBBCC3A6A";

  private readonly RnGoConfig _config;

  public RnGoConfigBuilder()
  {
    _config = new RnGoConfig();
  }

  public RnGoConfigBuilder WithValidDefaults()
  {
    _config.ApiKeys = new[] {ValidApiKey};
    return this;
  }

  public RnGoConfig Build()
  {
    return _config;
  }

  public RnGoConfig BuildWithValidDefaults()
    => WithValidDefaults().Build();
}