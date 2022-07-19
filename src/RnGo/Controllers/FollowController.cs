using Microsoft.AspNetCore.Mvc;
using RnGo.Core.Services;

namespace RnGo.Controllers;

[ApiController]
[Route("f")]
public class FollowController : ControllerBase
{
  private readonly ILinkService _linkSvc;

  public FollowController(ILinkService linkSvc)
  {
    _linkSvc = linkSvc;
  }

  [HttpGet, Route("{shortCode}")]
  public async Task<ActionResult> Get(string shortCode)
  {
    var resolvedUrl = await _linkSvc.ResolveAsync(shortCode);

    if (string.IsNullOrWhiteSpace(resolvedUrl))
      return BadRequest();

    return Redirect(resolvedUrl);
  }
}
