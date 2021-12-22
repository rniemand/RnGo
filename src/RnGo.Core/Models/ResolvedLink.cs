﻿namespace RnGo.Core.Models
{
  public class ResolvedLink
  {
    public string Url { get; set; }
    public long LinkId { get; set; }

    public ResolvedLink()
    {
      // TODO: [ResolvedLink] (TESTS) Add tests
      Url = string.Empty;
      LinkId = 0;
    }
  }
}
