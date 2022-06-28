using RnGo.Core.Entities;
using RnGo.Core.Models.Dto;

namespace RnGo.Core.Extensions;

public static class EntityExtensions
{
  public static LinkEntity ToEntity(this RnGoLinkDto dto) =>
    new()
    {
      LinkId = dto.LinkId,
      Url = dto.Url,
      ShortCode = dto.ShortCode
    };
}
