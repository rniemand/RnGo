using System.Diagnostics.CodeAnalysis;

namespace RnGo.Core.Helpers;

[ExcludeFromCodeCoverage]
public struct Base36 : IEquatable<Base36>
{
  // Originally based on https://www.codeproject.com/Articles/10619/Base-36-type-for-NET-C
  public static readonly Base36 MaxValue = new(long.MaxValue);
  public static readonly Base36 MinValue = new(long.MinValue + 1);

  private const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

  // "-" followed by long.MaxValue in base 36 ("1Y2P0IJ32E8E7")
  private const int MaxLength = 14;

  private long _numericValue;

  public Base36(long numericValue)
  {
    _numericValue = 0; //required by the struct.
    NumericValue = numericValue;
  }

  public Base36(string value)
  {
    _numericValue = 0; //required by the struct.
    Value = value;
  }

  public long NumericValue
  {
    get => _numericValue;
    set
    {
      // long.MinValue has no positive counterpart, so it cannot be represented
      if (value == long.MinValue)
        throw new Exception("InvalidBase36ValueException");

      _numericValue = value;
    }
  }

  public string Value
  {
    get => NumberToBase36(_numericValue);
    set => _numericValue = Base36ToNumber(value);
  }

  public static long Base36ToNumber(string base36Value)
  {
    var digits = base36Value.AsSpan();
    var isNegative = digits.Length > 0 && digits[0] == '-';
    if (isNegative)
      digits = digits[1..];

    if (digits.IsEmpty)
      throw new Exception("InvalidBase36NumberException");

    long result = 0;
    foreach (var digit in digits)
    {
      var digitValue = Base36DigitToNumber(digit);
      if (digitValue < 0)
        throw new Exception("InvalidBase36NumberException");

      // Overflowing a long means the value is out of range rather than silently wrapping
      if (result > (long.MaxValue - digitValue) / 36)
        throw new Exception("InvalidBase36NumberException");

      result = result * 36 + digitValue;
    }

    return isNegative ? -result : result;
  }

  public static string NumberToBase36(long numericValue)
  {
    if (numericValue == long.MinValue)
      throw new Exception("InvalidBase36ValueException");

    var isNegative = numericValue < 0;
    var remaining = isNegative ? -numericValue : numericValue;

    Span<char> buffer = stackalloc char[MaxLength];
    var position = buffer.Length;

    do
    {
      buffer[--position] = Digits[(int)(remaining % 36)];
      remaining /= 36;
    } while (remaining > 0);

    if (isNegative)
      buffer[--position] = '-';

    return new string(buffer[position..]);
  }

  private static int Base36DigitToNumber(char digit) => digit switch
  {
    >= '0' and <= '9' => digit - '0',
    >= 'A' and <= 'Z' => digit - 'A' + 10,
    >= 'a' and <= 'z' => digit - 'a' + 10,
    _ => -1
  };

  public static bool operator >(Base36 lhs, Base36 rhs)
    => lhs._numericValue > rhs._numericValue;

  public static bool operator <(Base36 lhs, Base36 rhs)
    => lhs._numericValue < rhs._numericValue;

  public static bool operator >=(Base36 lhs, Base36 rhs)
    => lhs._numericValue >= rhs._numericValue;

  public static bool operator <=(Base36 lhs, Base36 rhs)
    => lhs._numericValue <= rhs._numericValue;

  public static bool operator ==(Base36 lhs, Base36 rhs)
    => lhs._numericValue == rhs._numericValue;

  public static bool operator !=(Base36 lhs, Base36 rhs)
    => !(lhs == rhs);

  public static Base36 operator +(Base36 lhs, Base36 rhs)
    => new(lhs._numericValue + rhs._numericValue);

  public static Base36 operator -(Base36 lhs, Base36 rhs)
    => new(lhs._numericValue - rhs._numericValue);

  public static Base36 operator ++(Base36 value)
    => new(value._numericValue + 1);

  public static Base36 operator --(Base36 value)
    => new(value._numericValue - 1);

  public static Base36 operator *(Base36 lhs, Base36 rhs)
    => new(lhs._numericValue * rhs._numericValue);

  public static Base36 operator /(Base36 lhs, Base36 rhs)
    => new(lhs._numericValue / rhs._numericValue);

  public static Base36 operator %(Base36 lhs, Base36 rhs)
    => new(lhs._numericValue % rhs._numericValue);

  public static implicit operator long(Base36 value)
    => value._numericValue;

  public static implicit operator int(Base36 value)
  {
    try
    {
      return checked((int)value._numericValue);
    }
    catch (OverflowException)
    {
      throw new OverflowException("Overflow: Value too large to return as an integer");
    }
  }

  public static implicit operator short(Base36 value)
  {
    try
    {
      return checked((short)value._numericValue);
    }
    catch (OverflowException)
    {
      throw new OverflowException("Overflow: Value too large to return as a short");
    }
  }

  public static implicit operator Base36(long value)
    => new(value);

  public static explicit operator string(Base36 value)
    => value.Value;

  public static implicit operator Base36(string value)
    => new(value);

  public override string ToString()
    => NumberToBase36(_numericValue);

  public override int GetHashCode()
    => _numericValue.GetHashCode();

  public bool Equals(Base36 other)
    => _numericValue == other._numericValue;

  public override bool Equals(object? obj)
    => obj is Base36 other && Equals(other);

  public string ToString(int minimumDigits)
    => NumberToBase36(_numericValue).PadLeft(minimumDigits, '0');
}
