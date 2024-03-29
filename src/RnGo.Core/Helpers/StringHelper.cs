using System.Text;

namespace RnGo.Core.Helpers;

public interface IStringHelper
{
  string Base64Encode(string input);
  string Base64Decode(string input);
  string GenerateLinkString(long input);
}

public class StringHelper : IStringHelper
{
  public string Base64Encode(string input) =>
    Convert.ToBase64String(Encoding.UTF8.GetBytes(input));

  public string Base64Decode(string input) =>
    Encoding.UTF8.GetString(Convert.FromBase64String(input));

  public string GenerateLinkString(long input) =>
    Base36.NumberToBase36(input);
}
