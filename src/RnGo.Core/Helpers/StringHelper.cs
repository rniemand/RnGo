using System.Text;

namespace RnGo.Core.Helpers
{
  public interface IStringHelper
  {
    string Base64Encode(string input);
    string Base64Decode(string input);
    string GuidString();
    string GenerateLinkString(long input);
  }

  public class StringHelper : IStringHelper
  {
    public string Base64Encode(string input)
    {
      return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
    }

    public string Base64Decode(string input)
    {
      return Encoding.UTF8.GetString(Convert.FromBase64String(input));
    }

    public string GuidString()
    {
      return Guid.NewGuid().ToString("N");
    }

    public string GenerateLinkString(long input)
    {
      var numberToBase36 = Base36.NumberToBase36(input);
      var base36ToNumber = Base36.Base36ToNumber(numberToBase36);

      var lower = numberToBase36.ToLower();
      var base36 = new Base36(lower).NumericValue;

      return "";
    }
  }
}
