using Microsoft.AspNetCore.Mvc;
using RnGo.Core.Services;

namespace RnGo.Controllers
{
  [ApiController]
  [Route("f")]
  public class FollowController : ControllerBase
  {
    private readonly ILinkService _linkService;

    public FollowController(ILinkService linkService)
    {
      _linkService = linkService;
    }

    [HttpGet, Route("{shortCode}")]
    public async Task<ActionResult> Get(string shortCode)
    {
      // TODO: [FollowController.Get] (TESTS) Add tests
      var resolvedLink = await _linkService.Resolve(shortCode);

      if (resolvedLink is null)
        return NotFound();

      return Redirect(resolvedLink.Url);
    }
  }
}
