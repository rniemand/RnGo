using System;
using NSubstitute.Exceptions;
using RnGo.Core.Entities;

namespace RnGo.Core.T1.Tests.TestSupport.Builders;

public class ApiKeyEntityBuilder
{
  public static ApiKeyEntity Default = new ApiKeyEntityBuilder().BuildWithValidDefaults();

  private readonly ApiKeyEntity _entity = new();

  public ApiKeyEntityBuilder() { }

  public ApiKeyEntityBuilder(ApiKeyEntity apiKey)
  {
    FromApiKeyEntity(apiKey);
  }

  public ApiKeyEntityBuilder FromApiKeyEntity(ApiKeyEntity apiKey)
  {
    _entity.ApiKey = apiKey.ApiKey;
    _entity.ApiKeyId = apiKey.ApiKeyId;
    _entity.DateAddedUtc = apiKey.DateAddedUtc;
    _entity.Deleted = apiKey.Deleted;
    _entity.Enabled = apiKey.Enabled;
    return this;
  }

  public ApiKeyEntityBuilder WithApiKey(string apiKey)
  {
    _entity.ApiKey = apiKey;
    return this;
  }

  public ApiKeyEntityBuilder WithValidDefaults()
  {
    _entity.DateAddedUtc = DateTime.UtcNow;
    _entity.ApiKey = "3BC034D6-B059-401A-8C5F-1BB1B5CA214B";
    _entity.Enabled = true;
    _entity.Deleted = false;
    return this;
  }

  public ApiKeyEntity Build()
  {
    return _entity;
  }

  public ApiKeyEntity BuildWithValidDefaults()
    => WithValidDefaults().Build();
}
