using NUnit.Framework;
using RnGo.Core.Helpers;

namespace RnGo.Core.T1.Tests.Helpers;

[TestFixture]
public class StringHelperTests
{
  [TestCase("Hello", "SGVsbG8=")]
  public void Base64Encode_GivenValue_ShouldReturnEncodedValue(string input, string expected) =>
    Assert.That(GetStringHelper().Base64Encode(input), Is.EqualTo(expected));

  [TestCase("SGVsbG8=", "Hello")]
  public void Base64Decode_GivenValue_ShouldReturnEncodedValue(string input, string expected) =>
    Assert.That(GetStringHelper().Base64Decode(input), Is.EqualTo(expected));

  [TestCase(1021, "SD")]
  [TestCase(1, "1")]
  [TestCase(11, "B")]
  public void GenerateLinkString_GivenValue_ShouldReturnEncodedValue(long input, string expected) =>
    Assert.That(GetStringHelper().GenerateLinkString(input), Is.EqualTo(expected));

  private static StringHelper GetStringHelper() => new();
}
