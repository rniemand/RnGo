using NSubstitute;
using Rn.NetCore.Common.Logging;
using Rn.NetCore.DbCommon;
using Rn.NetCore.Metrics;

namespace RnGo.Core.T1.Tests.TestSupport;

public static class BaseRepoHelperFactory
{
  public static IBaseRepoHelper Create<TRepo>(
    IDbConnectionHelper? connectionHelper = null,
    string? connectionName = null,
    ILoggerAdapter<TRepo>? logger = null,
    IMetricService? metrics = null,
    ISqlFormatter? sqlFormatter = null,
    RnDbConfig? dbConfig = null)
  {
    var baseRepoHelper = Substitute.For<IBaseRepoHelper>();

    baseRepoHelper
      .ResolveLogger<TRepo>()
      .Returns(logger ?? Substitute.For<ILoggerAdapter<TRepo>>());

    baseRepoHelper
      .ResolveConnectionHelper()
      .Returns(connectionHelper ?? Substitute.For<IDbConnectionHelper>());

    baseRepoHelper
      .ResolveMetricService()
      .Returns(metrics ?? Substitute.For<IMetricService>());

    baseRepoHelper
      .ResolveConnectionName(Arg.Any<string>(), Arg.Any<string>())
      .Returns(string.IsNullOrWhiteSpace(connectionName) ? typeof(TRepo).Name : connectionName);

    baseRepoHelper
      .ResolveSqlFormatter()
      .Returns(sqlFormatter ?? Substitute.For<ISqlFormatter>());

    baseRepoHelper
      .GetRnDbConfig()
      .Returns(dbConfig ?? new RnDbConfig());

    return baseRepoHelper;
  }

  public static object? GetProperty(object? source, string name) =>
    source?.GetType().GetProperty(name)?.GetValue(source);
}
