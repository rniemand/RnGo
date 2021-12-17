using Microsoft.AspNetCore.Mvc;

namespace RnGo.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class FollowController : ControllerBase
  {
    [HttpGet, Route("{id}")]
    public ActionResult Get(string id)
    {
      return Redirect($"https://google.ca?q={id}");
    }
  }
}
