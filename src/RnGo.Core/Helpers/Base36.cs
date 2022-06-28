using System.Diagnostics.CodeAnalysis;

namespace RnGo.Core.Helpers;

[ExcludeFromCodeCoverage]
public struct Base36
{
  // https://www.codeproject.com/Articles/10619/Base-36-type-for-NET-C
  public static readonly Base36 MaxValue = new(long.MaxValue);
  public static readonly Base36 MinValue = new(long.MinValue + 1);

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
      //Make sure value is between allowed ranges
      if (value is <= long.MinValue or > long.MaxValue)
      {
        //throw new InvalidBase36ValueException(value);
        throw new Exception("InvalidBase36ValueException");
      }

      _numericValue = value;
    }
  }

  public string Value
  {
    get => Base36.NumberToBase36(_numericValue);
    set
    {
      try
      {
        _numericValue = Base36.Base36ToNumber(value);
      }
      catch
      {
        //Catch potential errors
        //throw new InvalidBase36NumberException(value);
        throw new Exception("InvalidBase36NumberException");
      }
    }
  }

  public static long Base36ToNumber(string base36Value)
  {
    //Make sure we have passed something
    if (base36Value == "")
    {
      //throw new InvalidBase36NumberException(Base36Value);
      throw new Exception("InvalidBase36NumberException");
    }

    //Make sure the number is in upper case:
    base36Value = base36Value.ToUpper();

    //Account for negative values:
    var isNegative = false;

    if (base36Value[0] == '-')
    {
      base36Value = base36Value[1..];
      isNegative = true;
    }

    //Loop through our string and calculate its value
    try
    {
      //Keep a running total of the value
      long returnValue = Base36DigitToNumber(base36Value[^1]);

      //Loop through the character in the string (right to left) and add
      //up increasing powers as we go.
      for (var i = 1; i < base36Value.Length; i++)
      {
        returnValue += ((long)Math.Pow(36, i) * Base36DigitToNumber(base36Value[^(i + 1)]));
      }

      //Do negative correction if required:
      if (isNegative)
      {
        return returnValue * -1;
      }

      return returnValue;
    }
    catch
    {
      //If something goes wrong, this is not a valid number
      //throw new InvalidBase36NumberException(Base36Value);
      throw new Exception("InvalidBase36NumberException");
    }
  }

  public static string NumberToBase36(long numericValue)
  {
    try
    {
      //Handle negative values:
      // ReSharper disable once ConvertIfStatementToReturnStatement
      if (numericValue < 0)
      {
        return string.Concat("-", PositiveNumberToBase36(Math.Abs(numericValue)));
      }
      else
      {
        return PositiveNumberToBase36(numericValue);
      }
    }
    catch
    {
      //throw new InvalidBase36ValueException(NumericValue);
      throw new Exception("InvalidBase36ValueException");
    }
  }

  private static string PositiveNumberToBase36(long numericValue)
  {
    //This is a clever recursively called function that builds
    //the base-36 string representation of the long base-10 value
    // ReSharper disable once ConvertIfStatementToReturnStatement
    if (numericValue < 36)
    {
      //The get out clause; fires when we reach a number less than 
      //36 - this means we can add the last digit.
      return NumberToBase36Digit((byte)numericValue).ToString();
    }

    //Add digits from left to right in powers of 36 
    //(recursive)
    return string.Concat(PositiveNumberToBase36(numericValue / 36), NumberToBase36Digit((byte)(numericValue % 36)).ToString());
  }

  private static byte Base36DigitToNumber(char base36Digit)
  {
    //Converts one base-36 digit to it's base-10 value
    if (!char.IsLetterOrDigit(base36Digit))
    {
      //throw new InvalidBase36DigitException(Base36Digit);
      throw new Exception("InvalidBase36DigitException");
    }

    if (char.IsDigit(base36Digit))
    {
      //Handles 0 - 9
      return byte.Parse(base36Digit.ToString());
    }
    else
    {
      //Handles A - Z
      return (byte)(base36Digit - 55);
    }
  }

  private static char NumberToBase36Digit(byte numericValue)
  {
    //Converts a number to it's base-36 value.
    //Only works for numbers <= 35.
    // ReSharper disable once ConvertIfStatementToSwitchStatement
    // ReSharper disable once ConvertIfStatementToSwitchExpression
    if (numericValue > 35)
    {
      //throw new InvalidBase36DigitValueException(NumericValue);
      throw new Exception("InvalidBase36DigitValueException");
    }

    //Numbers:
    if (numericValue <= 9)
    {
      return numericValue.ToString()[0];
    }
    else
    {
      //Note that A is code 65, and in this
      //scheme, A = 10.
      return (char)(numericValue + 55);
    }
  }

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
    => new(value._numericValue++);

  public static Base36 operator --(Base36 value)
    => new(value._numericValue--);

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
      return (int)value._numericValue;
    }
    catch
    {
      throw new OverflowException("Overflow: Value too large to return as an integer");
    }
  }

  public static implicit operator short(Base36 value)
  {
    try
    {
      return (short)value._numericValue;
    }
    catch
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

  public override bool Equals(object? obj)
  {
    if (obj is not Base36 base36)
    {
      return false;
    }

    return this == base36;
  }

  public string ToString(int minimumDigits)
  {
    var base36Value = NumberToBase36(_numericValue);

    if (base36Value.Length >= minimumDigits)
    {
      return base36Value;
    }

    var padding = new string('0', (minimumDigits - base36Value.Length));
    return $"{padding}{base36Value}";
  }
}
