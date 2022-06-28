using Microsoft.AspNetCore.Mvc;
using RnGo.Core.Models;
using RnGo.Core.Models.Responses;
using RnGo.Core.Services;

namespace RnGo.Controllers;

[ApiController]
[Route("links")]
public class LinkController : ControllerBase
{
  private readonly ILinkService _linkSvc;

  public LinkController(ILinkService linkSvc)
  {
    _linkSvc = linkSvc;
  }

  [HttpPost, Route("")]
  public async Task<AddLinkResponse> StoreLink(
    [FromBody] AddLinkRequest request) =>
    await _linkSvc.AddLink(request);

  [HttpGet, Route("count")]
  public async Task<long> GetLinkCount() =>
    await _linkSvc.GetLinkCount();
}
