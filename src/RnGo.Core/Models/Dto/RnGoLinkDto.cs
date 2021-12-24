﻿using System.Text.Json.Serialization;
using Newtonsoft.Json;
using RnGo.Core.Entities;

namespace RnGo.Core.Models.Dto
{
  public class RnGoLinkDto
  {
    [JsonProperty("url"), JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonProperty("linkId"), JsonPropertyName("linkId")]
    public long LinkId { get; set; }

    [JsonProperty("shortCode"), JsonPropertyName("shortCode")]
    public string ShortCode { get; set; }

    public RnGoLinkDto()
    {
      // TODO: [RnGoLinkDto] (TESTS) Add tests
      Url = string.Empty;
      LinkId = 0;
      ShortCode = string.Empty;
    }

    public static RnGoLinkDto? FromEntity(LinkEntity? entity)
    {
      // TODO: [RnGoLinkDto.FromEntity] (TESTS) Add tests
      if (entity is null)
        return null;

      return new RnGoLinkDto
      {
        LinkId = entity.LinkId,
        ShortCode = entity.ShortCode,
        Url = entity.Url
      };
    }
  }
}