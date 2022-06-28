using Microsoft.Extensions.DependencyInjection;
using Rn.NetCore.Common.Helpers;
using Rn.NetCore.Common.Logging;
using RnGo.Core.Entities;
using RnGo.Core.Helpers;
using RnGo.Core.Models;
using RnGo.Core.Repos;
using RnGo.Core.Services;

namespace DevConsole;

public class RnGoDevelopment
{
  public RnGoDevelopment DoNothing()
  {
    return this;
  }

  public RnGoDevelopment HelloWorld()
  {
    DIContainer.Services
      .GetRequiredService<ILoggerAdapter<RnGoDevelopment>>()
      .LogInformation("Hello World");

    return this;
  }

  public RnGoDevelopment ResolveLink()
  {
    DIContainer.Services
      .GetRequiredService<ILinkService>()
      .Resolve("a");

    return this;
  }

  public RnGoDevelopment Base64Encode()
  {
    var helper = DIContainer.Services.GetRequiredService<IStringHelper>();
    var encoded = helper.Base64Encode("hello");
    Console.WriteLine("Encoded: " + encoded);
    var decoded = helper.Base64Decode(encoded);
    Console.WriteLine("Decoded: " + decoded);
    return this;
  }

  public RnGoDevelopment GenerateLinkString(long input)
  {
    var linkString = DIContainer.Services
      .GetRequiredService<IStringHelper>()
      .GenerateLinkString(input);

    Console.WriteLine($"Generated '{linkString}' from '{input}'");
    return this;
  }

  public RnGoDevelopment AddLink(string url, string? apiKey = null)
  {
    var linkService = DIContainer.Services.GetRequiredService<ILinkService>();

    var response = linkService
      .AddLink(new AddLinkRequest
      {
        Url = url,
        ApiKey = apiKey ?? "18A8B66F-B4F1-4814-8771-D1EABD9CFB43"
      })
      .GetAwaiter()
      .GetResult();

    Console.WriteLine($"Stored as '{response.ShortCode}'");

    return this;
  }

  public RnGoDevelopment ResolveLink(string shortCode)
  {
    var services = DIContainer.Services;
    var jsonHelper = services.GetRequiredService<IJsonHelper>();

    var resolvedLink = services
      .GetRequiredService<ILinkService>()
      .Resolve(shortCode)
      .GetAwaiter()
      .GetResult();

    Console.WriteLine($"Resolved as: {jsonHelper.SerializeObject(resolvedLink)}");

    return this;
  }

  public RnGoDevelopment AddDatabaseLink()
  {
    var repo = DIContainer.Services.GetRequiredService<ILinkRepo>();

    repo
      .AddLink(new LinkEntity
      {
        Url = "https://docs.google.com/spreadsheets",
        ShortCode = "2"
      })
      .GetAwaiter()
      .GetResult();


    return this;
  }

  public RnGoDevelopment GetLinkCount()
  {
    var urlCount = DIContainer.Services
      .GetRequiredService<ILinkService>()
      .GetLinkCount()
      .GetAwaiter()
      .GetResult();

    Console.WriteLine("URL Count: {0}", urlCount);

    return this;
  }

  public RnGoDevelopment StoreApiKey(string apiKey)
  {
    var apiKeyRepo = DIContainer.Services.GetRequiredService<IApiKeyRepo>();
    var apiKeyEntity = apiKeyRepo.GetByApiKey(apiKey).GetAwaiter().GetResult();
    if (apiKeyEntity is null)
    {
      apiKeyRepo.Add(apiKey).GetAwaiter().GetResult();
      apiKeyEntity = apiKeyRepo.GetByApiKey(apiKey).GetAwaiter().GetResult();
    }

    Console.WriteLine(apiKeyEntity.ApiKey);

    return this;
  }
}
