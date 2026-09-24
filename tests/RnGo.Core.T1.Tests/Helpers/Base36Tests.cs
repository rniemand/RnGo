using System;
using NUnit.Framework;
using RnGo.Core.Helpers;

namespace RnGo.Core.T1.Tests.Helpers;

[TestFixture]
public class Base36Tests
{
  [TestCase(0, "0")]
  [TestCase(9, "9")]
  [TestCase(10, "A")]
  [TestCase(35, "Z")]
  [TestCase(36, "10")]
  [TestCase(1296, "100")]
  [TestCase(-36, "-10")]
  public void NumberToBase36_GivenValue_ShouldReturnExpected(long input, string expected) =>
    Assert.That(Base36.NumberToBase36(input), Is.EqualTo(expected));

  [TestCase("0", 0)]
  [TestCase("Z", 35)]
  [TestCase("z", 35)]
  [TestCase("10", 36)]
  [TestCase("2H", 89)]
  [TestCase("-10", -36)]
  public void Base36ToNumber_GivenValue_ShouldReturnExpected(string input, long expected) =>
    Assert.That(Base36.Base36ToNumber(input), Is.EqualTo(expected));

  [TestCase(1)]
  [TestCase(1021)]
  [TestCase(123456789)]
  [TestCase(long.MaxValue)]
  public void RoundTrip_GivenValue_ShouldReturnOriginal(long input) =>
    Assert.That(Base36.Base36ToNumber(Base36.NumberToBase36(input)), Is.EqualTo(input));

  [TestCase("")]
  [TestCase("A-B")]
  [TestCase("A B")]
  public void Base36ToNumber_GivenInvalidValue_ShouldThrow(string input) =>
    Assert.Throws<Exception>(() => Base36.Base36ToNumber(input));

  [Test]
  public void Value_GivenNumericValue_ShouldReturnBase36String() =>
    Assert.That(new Base36(1021).Value, Is.EqualTo("SD"));

  [Test]
  public void NumericValue_GivenBase36String_ShouldReturnNumber() =>
    Assert.That(new Base36("SD").NumericValue, Is.EqualTo(1021));
}
