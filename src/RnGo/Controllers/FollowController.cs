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
      var resolvedUrl = await _linkService.Resolve(shortCode);

      if (string.IsNullOrWhiteSpace(resolvedUrl))
        return BadRequest();

      return Redirect(resolvedUrl);
    }
  }
}
