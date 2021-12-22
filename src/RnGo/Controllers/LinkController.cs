using Microsoft.AspNetCore.Mvc;
using RnGo.Core.Models;
using RnGo.Core.Services;

namespace RnGo.Controllers
{
  [ApiController]
  [Route("links")]
  public class LinkController : ControllerBase
  {
    private readonly ILinkService _linkService;

    public LinkController(ILinkService linkService)
    {
      _linkService = linkService;
    }

    [HttpPost, Route("")]
    public async Task<string> StoreLink(ResolvedLink link)
    {
      // TODO: [LinkController.StoreLink] (TESTS) Add tests
      return await _linkService.StoreLink(link);
    }
  }
}
