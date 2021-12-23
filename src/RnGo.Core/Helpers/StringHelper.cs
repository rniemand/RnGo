using System.Text;

namespace RnGo.Core.Helpers
{
  public interface IStringHelper
  {
    string Base64Encode(string input);
    string Base64Decode(string input);
    string GuidString();
    string GenerateLinkString(long input);
    long LinkStringToLong(string linkString);
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
      return Base36.NumberToBase36(input);
    }

    public long LinkStringToLong(string linkString)
    {
      // TODO: [StringHelper.LinkStringToLong] (TESTS) Add tests
      return Base36.Base36ToNumber(linkString);
    }
  }
}
