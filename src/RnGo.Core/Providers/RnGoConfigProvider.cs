using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RnGo.Core.Configuration;

namespace RnGo.Core.Providers
{
  public interface IRnGoConfigProvider
  {
    RnGoConfig Provide();
  }

  public class RnGoConfigProvider : IRnGoConfigProvider
  {
    private readonly ILogger<RnGoConfigProvider> _logger;
    private readonly IConfiguration _configuration;
    private RnGoConfig? _config = null;

    public RnGoConfigProvider(
      ILogger<RnGoConfigProvider> logger,
      IConfiguration configuration)
    {
      // TODO: [RnGoConfigProvider] (TESTS) Add tests
      _logger = logger;
      _configuration = configuration;
    }

    public RnGoConfig Provide()
    {
      // TODO: [RnGoConfigProvider.Provide] (TESTS) Add tests
      if (_config is null)
      {
        _config = new RnGoConfig();
        var section = _configuration.GetSection(RnGoConfig.Key);
        if(section.Exists())
          section.Bind(_config);
      }

      return _config;
    }
  }
}
