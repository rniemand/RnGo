using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rn.NetCore.Common.Abstractions;
using RnGo.Core.Configuration;

namespace RnGo.Core.Providers;

public interface IRnGoConfigProvider
{
  RnGoConfig Provide();
}

public class RnGoConfigProvider : IRnGoConfigProvider
{
  private readonly ILogger<RnGoConfigProvider> _logger;
  private readonly IConfiguration _configuration;
  private readonly IEnvironmentAbstraction _environment;
  private readonly IPathAbstraction _path;
  private RnGoConfig? _config;

  public RnGoConfigProvider(
    ILogger<RnGoConfigProvider> logger,
    IConfiguration configuration,
    IEnvironmentAbstraction environment,
    IPathAbstraction path)
  {
    // TODO: [RnGoConfigProvider] (TESTS) Add tests
    _logger = logger;
    _configuration = configuration;
    _environment = environment;
    _path = path;
  }

  public RnGoConfig Provide()
  {
    // TODO: [RnGoConfigProvider.Provide] (TESTS) Add tests
    if (_config is null)
    {
      _config = new RnGoConfig();
      var section = _configuration.GetSection(RnGoConfig.Key);
      if(section.Exists()) section.Bind(_config);
    }

    // Generate paths
    _config.RootDirectory = GenerateRootDir(_config.RootDirectory);

    // Return bound config
    return _config;
  }

  // Internal methods
  private string GenerateRootDir(string configRoot)
  {
    // TODO: [RnGoConfigProvider.GenerateRootDir] (TESTS) Add tests
    if (string.IsNullOrWhiteSpace(configRoot) || configRoot == "./")
      configRoot = _environment.CurrentDirectory;

    var fullPath = _path.GetFullPath(configRoot);

    if (!_path.EndsInDirectorySeparator(fullPath))
      fullPath = _path.GetFullPath(fullPath + _path.DirectorySeparatorChar);

    _logger.LogInformation("Setting {{root}} to: {path}", fullPath);
    return fullPath;
  }
}