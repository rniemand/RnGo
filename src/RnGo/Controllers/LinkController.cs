﻿using Microsoft.AspNetCore.Mvc;
using RnGo.Core.Models;
using RnGo.Core.Models.Responses;
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
    public async Task<AddLinkResponse> StoreLink([FromBody] AddLinkRequest request)
    {
      // TODO: [LinkController.StoreLink] (TESTS) Add tests
      return await _linkService.AddLink(request);
    }

    [HttpGet, Route("count")]
    public async Task<long> GetLinkCount()
    {
      // TODO: [LinkController.GetLinkCount] (TESTS) Add tests
      return await _linkService.GetLinkCount();
    }
  }
}