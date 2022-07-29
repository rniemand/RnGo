using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Rn.NetCore.Common.Logging;
using Rn.NetCore.DbCommon;
using Rn.NetCore.Metrics;
using RnGo.Core.Entities;
using RnGo.Core.RepoQueries;
using RnGo.Core.Repos;
using RnGo.Core.T1.Tests.TestSupport.Builders;

namespace RnGo.Core.T1.Tests.Repos;

[TestFixture]
public class ApiKeyRepoTests
{
  private const string ApiKey = "1BD9DE45-E447-4313-9C36-A4868DC8D968";
  private const string ConnectionName = nameof(ApiKeyRepo);
  private const string SqlQuery = "SELECT * FROM Table";

  [Test]
  public async Task AddAsync_GivenCalled_ShouldCall_ExecuteAsync()
  {
    // arrange
    var connectionHelper = Substitute.For<IDbConnectionHelper>();
    var dbConnection = Substitute.For<IDbConnection>();
    var repoQueries = Substitute.For<IApiKeyRepoQueries>();

    repoQueries
      .Add()
      .Returns(SqlQuery);

    connectionHelper
      .GetConnection(ConnectionName)
      .Returns(dbConnection);

    connectionHelper
      .ExecuteAsync(dbConnection, SqlQuery, Arg.Any<ApiKeyEntity>())
      .Returns(1);

    var baseRepoHelper = GetBaseRepoHelper(
      connectionHelper: connectionHelper,
      connectionName: ConnectionName);

    // act
    var repo = GetApiKeyRepo(baseRepoHelper, repoQueries);
    var rowCount = await repo.AddAsync(ApiKey);

    // assert
    Assert.That(rowCount, Is.EqualTo(1));

    connectionHelper
      .Received(1)
      .GetConnection(ConnectionName);

    await connectionHelper
      .Received(1)
      .ExecuteAsync(dbConnection, SqlQuery, Arg.Is<ApiKeyEntity>(x => x.ApiKey.Equals(ApiKey)));
  }

  [Test]
  public async Task GetAsync_GivenCalled_ShouldCall_GetSingle()
  {
    // arrange
    var connectionHelper = Substitute.For<IDbConnectionHelper>();
    var dbConnection = Substitute.For<IDbConnection>();
    var repoQueries = Substitute.For<IApiKeyRepoQueries>();
    var apiKey1 = ApiKeyEntityBuilder.Default;

    repoQueries
      .Get()
      .Returns(SqlQuery);

    connectionHelper
      .GetConnection(ConnectionName)
      .Returns(dbConnection);

    connectionHelper
      .QueryAsync<ApiKeyEntity>(dbConnection, SqlQuery, Arg.Any<ApiKeyEntity>())
      .Returns(new List<ApiKeyEntity> { apiKey1 });

    var baseRepoHelper = GetBaseRepoHelper(
      connectionHelper: connectionHelper,
      connectionName: ConnectionName);

    // act
    var repo = GetApiKeyRepo(baseRepoHelper, repoQueries);
    var dbApiKey = await repo.GetAsync(ApiKey);

    // assert
    Assert.That(dbApiKey, Is.Not.Null.And.InstanceOf<ApiKeyEntity>());
    Assert.That(dbApiKey, Is.EqualTo(apiKey1));

    connectionHelper
      .Received(1)
      .GetConnection(ConnectionName);

    await connectionHelper
      .Received(1)
      .QueryAsync<ApiKeyEntity>(dbConnection, SqlQuery, Arg.Is<ApiKeyEntity>(x => x.ApiKey.Equals(ApiKey)));
  }

  [Test]
  public async Task GetEnabledAsync_GivenCalled_ShouldCall_GetSingle()
  {
    // arrange
    var connectionHelper = Substitute.For<IDbConnectionHelper>();
    var dbConnection = Substitute.For<IDbConnection>();
    var repoQueries = Substitute.For<IApiKeyRepoQueries>();
    var apiKey1 = ApiKeyEntityBuilder.Default;
    var apiKey2 = new ApiKeyEntityBuilder(apiKey1)
      .WithApiKey("EA0EA5E3-F494-4458-B93D-AA6E06F58EBD")
      .Build();

    repoQueries
      .GetEnabled()
      .Returns(SqlQuery);

    connectionHelper
      .GetConnection(ConnectionName)
      .Returns(dbConnection);

    connectionHelper
      .QueryAsync<ApiKeyEntity>(dbConnection, SqlQuery)
      .Returns(new List<ApiKeyEntity> { apiKey1, apiKey2 });

    var baseRepoHelper = GetBaseRepoHelper(
      connectionHelper: connectionHelper,
      connectionName: ConnectionName);

    // act
    var repo = GetApiKeyRepo(baseRepoHelper, repoQueries);
    var dbApiKeys = await repo.GetEnabledAsync();

    // assert
    Assert.That(dbApiKeys.Count, Is.EqualTo(2));
    Assert.That(dbApiKeys[0], Is.EqualTo(apiKey1));
    Assert.That(dbApiKeys[1], Is.EqualTo(apiKey2));

    connectionHelper
      .Received(1)
      .GetConnection(ConnectionName);

    await connectionHelper
      .Received(1)
      .QueryAsync<ApiKeyEntity>(dbConnection, SqlQuery);
  }


  // Internal methods
  private static IBaseRepoHelper GetBaseRepoHelper(
    ILoggerAdapter<ApiKeyRepo>? logger = null,
    IDbConnectionHelper? connectionHelper = null,
    IMetricService? metrics = null,
    ISqlFormatter? sqlFormatter = null,
    RnDbConfig? dbConfig = null,
    string? connectionName = null)
  {
    var baseRepoHelper = Substitute.For<IBaseRepoHelper>();

    baseRepoHelper
      .ResolveLogger<ApiKeyRepo>()
      .Returns(logger ?? Substitute.For<ILoggerAdapter<ApiKeyRepo>>());

    baseRepoHelper
      .ResolveConnectionHelper()
      .Returns(connectionHelper ?? Substitute.For<IDbConnectionHelper>());

    baseRepoHelper
      .ResolveMetricService()
      .Returns(metrics ?? Substitute.For<IMetricService>());

    baseRepoHelper
      .ResolveConnectionName(Arg.Any<string>(), Arg.Any<string>())
      .Returns(string.IsNullOrWhiteSpace(connectionName) ? nameof(ApiKeyRepo) : connectionName);

    baseRepoHelper
      .ResolveSqlFormatter()
      .Returns(sqlFormatter ?? Substitute.For<ISqlFormatter>());

    baseRepoHelper
      .GetRnDbConfig()
      .Returns(dbConfig ?? new RnDbConfig());

    return baseRepoHelper;
  }

  private static ApiKeyRepo GetApiKeyRepo(
    IBaseRepoHelper? helper = null,
    IApiKeyRepoQueries? repoQueries = null) =>
    new(helper ?? GetBaseRepoHelper(), repoQueries ?? Substitute.For<IApiKeyRepoQueries>());
}
