using Microsoft.Extensions.DependencyInjection;
using RnGo.Core.Entities;
using RnGo.Core.Helpers;
using RnGo.Core.Models;
using RnGo.Core.Models.Responses;
using RnGo.Core.Repos;
using RnGo.Core.Services;

namespace DevConsole;

public static class RnGoDevelopment
{
  public static async Task<string> ResolveLink(string shortCode)
  {
    return await DIContainer.Services
      .GetRequiredService<ILinkService>()
      .Resolve(shortCode);
  }

  public static string Base64Encode(string input)
  {
    var helper = DIContainer.Services.GetRequiredService<IStringHelper>();
    var encoded = helper.Base64Encode(input);
    Console.WriteLine("Encoded: " + encoded);
    var decoded = helper.Base64Decode(encoded);
    Console.WriteLine("Decoded: " + decoded);
    return encoded;
  }

  public static string GenerateLinkString(long input)
  {
    var linkString = DIContainer.Services
      .GetRequiredService<IStringHelper>()
      .GenerateLinkString(input);

    Console.WriteLine($"Generated '{linkString}' from '{input}'");
    return linkString;
  }

  public static async Task<AddLinkResponse> AddLink(string url, string? apiKey = null)
  {
    var linkService = DIContainer.Services.GetRequiredService<ILinkService>();

    var response = await linkService
      .AddLink(new AddLinkRequest
      {
        Url = url,
        ApiKey = apiKey ?? "18A8B66F-B4F1-4814-8771-D1EABD9CFB43"
      });

    Console.WriteLine($"Stored as '{response.ShortCode}'");

    return response;
  }
  
  public static async Task AddDatabaseLink()
  {
    await DIContainer.Services
      .GetRequiredService<ILinkRepo>()
      .AddLink(new LinkEntity
      {
        Url = "https://docs.google.com/spreadsheets",
        ShortCode = "2"
      });
  }

  public static async Task<long> GetLinkCount()
  {
    var urlCount = await DIContainer.Services
      .GetRequiredService<ILinkService>()
      .GetLinkCount();

    Console.WriteLine("URL Count: {0}", urlCount);

    return urlCount;
  }

  public static async Task<ApiKeyEntity> StoreApiKey(string apiKey)
  {
    var apiKeyRepo = DIContainer.Services.GetRequiredService<IApiKeyRepo>();
    var apiKeyEntity = await apiKeyRepo.GetByApiKey(apiKey);

    if (apiKeyEntity is null)
    {
      await apiKeyRepo.Add(apiKey);
      apiKeyEntity = await apiKeyRepo.GetByApiKey(apiKey);
    }

    Console.WriteLine(apiKeyEntity!.ApiKey);

    return apiKeyEntity;
  }
}
