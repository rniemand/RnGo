using RnGo.Core.Entities;
using RnGo.Core.Models.Dto;

namespace RnGo.Core.Extensions;

public static class DtoExtensions
{
  public static RnGoLinkDto? ToDto(this LinkEntity? entity) => entity is null
      ? null
      : new RnGoLinkDto
      {
        LinkId = entity.LinkId,
        ShortCode = entity.ShortCode,
        Url = entity.Url
      };
}
