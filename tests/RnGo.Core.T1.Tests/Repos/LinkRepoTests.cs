using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Rn.NetCore.DbCommon;
using RnGo.Core.Entities;
using RnGo.Core.RepoQueries;
using RnGo.Core.Repos;
using RnGo.Core.T1.Tests.TestSupport;
using RnGo.Core.T1.Tests.TestSupport.Builders;

namespace RnGo.Core.T1.Tests.Repos;

[TestFixture]
public class LinkRepoTests
{
  private const string ConnectionName = nameof(LinkRepo);
  private const string SqlQuery = "SELECT * FROM Table";
  private const string LinkUrl = "https://richardn.ca";
  private const string ShortCode = "B";
  private const long LinkId = 11;

  private IDbConnectionHelper _connectionHelper = null!;
  private IDbConnection _dbConnection = null!;
  private ILinkRepoQueries _queries = null!;
  private LinkRepo _repo = null!;

  [SetUp]
  public void SetUp()
  {
    _connectionHelper = Substitute.For<IDbConnectionHelper>();
    _dbConnection = Substitute.For<IDbConnection>();
    _queries = Substitute.For<ILinkRepoQueries>();

    _connectionHelper
      .GetConnection(ConnectionName)
      .Returns(_dbConnection);

    var baseRepoHelper = BaseRepoHelperFactory.Create<LinkRepo>(
      connectionHelper: _connectionHelper,
      connectionName: ConnectionName);

    _repo = new LinkRepo(baseRepoHelper, _queries);
  }

  [Test]
  public async Task AddAsync_GivenEntity_ShouldExecuteAddQuery()
  {
    // arrange
    var entity = new LinkEntity(LinkUrl, ShortCode);

    _queries.Add().Returns(SqlQuery);

    _connectionHelper
      .ExecuteAsync(_dbConnection, SqlQuery, entity)
      .Returns(1);

    // act
    var rowCount = await _repo.AddAsync(entity);

    // assert
    Assert.That(rowCount, Is.EqualTo(1));
    await _connectionHelper.Received(1).ExecuteAsync(_dbConnection, SqlQuery, entity);
  }

  [Test]
  public async Task GetByUrlAsync_GivenUrl_ShouldQueryByUrl()
  {
    // arrange
    var link = BuildLink();

    _queries.GetByUrl().Returns(SqlQuery);

    _connectionHelper
      .QueryAsync<LinkEntity>(_dbConnection, SqlQuery, Arg.Any<object>())
      .Returns(new List<LinkEntity> { link });

    // act
    var dbLink = await _repo.GetByUrlAsync(LinkUrl);

    // assert
    Assert.That(dbLink, Is.EqualTo(link));
    await _connectionHelper.Received(1).QueryAsync<LinkEntity>(_dbConnection, SqlQuery,
      Arg.Is<object>(x => LinkUrl.Equals(BaseRepoHelperFactory.GetProperty(x, "Url"))));
  }

  [Test]
  public async Task GetByUrlAsync_GivenNoResults_ShouldReturnNull()
  {
    // arrange
    _queries.GetByUrl().Returns(SqlQuery);

    _connectionHelper
      .QueryAsync<LinkEntity>(_dbConnection, SqlQuery, Arg.Any<object>())
      .Returns(new List<LinkEntity>());

    // act
    var dbLink = await _repo.GetByUrlAsync(LinkUrl);

    // assert
    Assert.That(dbLink, Is.Null);
  }

  [Test]
  public async Task GetMaxLinkIdAsync_GivenCalled_ShouldReturnCountEntity()
  {
    // arrange
    var countEntity = new GenericCountEntity(42);

    _queries.GetMaxLinkId().Returns(SqlQuery);

    _connectionHelper
      .QueryAsync<GenericCountEntity>(_dbConnection, SqlQuery)
      .Returns(new List<GenericCountEntity> { countEntity });

    // act
    var result = await _repo.GetMaxLinkIdAsync();

    // assert
    Assert.That(result, Is.EqualTo(countEntity));
    await _connectionHelper.Received(1).QueryAsync<GenericCountEntity>(_dbConnection, SqlQuery);
  }

  [Test]
  public async Task GetByIdAsync_GivenLinkId_ShouldQueryByLinkId()
  {
    // arrange
    var link = BuildLink();

    _queries.GetById().Returns(SqlQuery);

    _connectionHelper
      .QueryAsync<LinkEntity>(_dbConnection, SqlQuery, Arg.Any<object>())
      .Returns(new List<LinkEntity> { link });

    // act
    var dbLink = await _repo.GetByIdAsync(LinkId);

    // assert
    Assert.That(dbLink, Is.EqualTo(link));
    await _connectionHelper.Received(1).QueryAsync<LinkEntity>(_dbConnection, SqlQuery,
      Arg.Is<object>(x => LinkId.Equals(BaseRepoHelperFactory.GetProperty(x, "LinkId"))));
  }

  [Test]
  public async Task GetByShortCodeAsync_GivenShortCode_ShouldQueryByShortCode()
  {
    // arrange
    var link = BuildLink();

    _queries.GetByShortCode().Returns(SqlQuery);

    _connectionHelper
      .QueryAsync<LinkEntity>(_dbConnection, SqlQuery, Arg.Any<object>())
      .Returns(new List<LinkEntity> { link });

    // act
    var dbLink = await _repo.GetByShortCodeAsync(ShortCode);

    // assert
    Assert.That(dbLink, Is.EqualTo(link));
    await _connectionHelper.Received(1).QueryAsync<LinkEntity>(_dbConnection, SqlQuery,
      Arg.Is<object>(x => ShortCode.Equals(BaseRepoHelperFactory.GetProperty(x, "ShortCode"))));
  }

  [Test]
  public async Task UpdateFollowCountAsync_GivenLinkId_ShouldExecuteUpdateQuery()
  {
    // arrange
    _queries.UpdateFollowCount().Returns(SqlQuery);

    _connectionHelper
      .ExecuteAsync(_dbConnection, SqlQuery, Arg.Any<object>())
      .Returns(1);

    // act
    var rowCount = await _repo.UpdateFollowCountAsync(LinkId);

    // assert
    Assert.That(rowCount, Is.EqualTo(1));
    await _connectionHelper.Received(1).ExecuteAsync(_dbConnection, SqlQuery,
      Arg.Is<object>(x => LinkId.Equals(BaseRepoHelperFactory.GetProperty(x, "LinkId"))));
  }

  private static LinkEntity BuildLink() =>
    new LinkEntityBuilder()
      .WithLinkId(LinkId)
      .WithShortCode(ShortCode)
      .WithUrl(LinkUrl)
      .Build();
}
