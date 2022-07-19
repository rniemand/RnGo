using RnGo.Core.Entities;
using RnGo.Core.Models;

namespace RnGo.Core.T1.Tests.TestSupport.Builders;

public class LinkEntityBuilder
{
  private readonly LinkEntity _entity = new();

  public LinkEntityBuilder FromAddLinkRequest(AddLinkRequest request) =>
    WithUrl(request.Url);

  public LinkEntityBuilder WithLinkId(long linkId)
  {
    _entity.LinkId = linkId;
    return this;
  }

  public LinkEntityBuilder WithShortCode(string shortCode)
  {
    _entity.ShortCode = shortCode;
    return this;
  }

  public LinkEntityBuilder WithUrl(string url)
  {
    _entity.Url = url;
    return this;
  }

  public LinkEntity Build() => _entity;
}
