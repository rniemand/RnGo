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
  public Task<AddLinkResponse> StoreLink(
    [FromBody] AddLinkRequest request) =>
    _linkSvc.AddLinkAsync(request);

  [HttpGet, Route("count")]
  public Task<long> GetLinkCount() =>
    _linkSvc.GetLinkCount();
}
