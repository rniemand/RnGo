using RnGo.Core.Models;

namespace RnGo.Core.T1.Tests.TestSupport.Builders;

public class AddLinkRequestBuilder
{
  private readonly AddLinkRequest _request = new();

  public AddLinkRequestBuilder WithUrl(string url)
  {
    _request.Url = url;
    return this;
  }

  public AddLinkRequestBuilder WithApiKey(string apiKey)
  {
    _request.ApiKey = apiKey;
    return this;
  }

  public AddLinkRequest Build() => _request;
}
