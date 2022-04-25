using System;
using RnGo.Core.Entities;

namespace RnGo.Core.T1.Tests.TestSupport.Builders;

public class ApiKeyEntityBuilder
{
  public const string DefaultApiKey = "D74937C6-BC0A-4563-9917-928F9F98C6A8";
  public static ApiKeyEntity ValidEntity = new ApiKeyEntityBuilder().BuildWithValidDefaults();

  private readonly ApiKeyEntity _entity;

  public ApiKeyEntityBuilder()
  {
    _entity = new ApiKeyEntity();
  }

  public ApiKeyEntityBuilder WithValidDefaults()
  {
    _entity.DateAddedUtc = DateTime.UtcNow;
    _entity.ApiKey = DefaultApiKey;
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