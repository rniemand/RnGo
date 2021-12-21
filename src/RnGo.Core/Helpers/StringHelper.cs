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



  public struct Base36
  {
    // https://www.codeproject.com/Articles/10619/Base-36-type-for-NET-C
    public static readonly Base36 MaxValue = new Base36(long.MaxValue);
    public static readonly Base36 MinValue = new Base36(long.MinValue + 1);

    private long numericValue;


    public Base36(long NumericValue)
    {
      numericValue = 0; //required by the struct.
      this.NumericValue = NumericValue;
    }

    public Base36(string Value)
    {
      numericValue = 0; //required by the struct.
      this.Value = Value;
    }

    public long NumericValue
    {
      get
      {
        return numericValue;
      }
      set
      {
        //Make sure value is between allowed ranges
        if (value <= long.MinValue || value > long.MaxValue)
        {
          //throw new InvalidBase36ValueException(value);
          throw new Exception("InvalidBase36ValueException");
        }

        numericValue = value;
      }
    }


    public string Value
    {
      get
      {
        return Base36.NumberToBase36(numericValue);
      }
      set
      {
        try
        {
          numericValue = Base36.Base36ToNumber(value);
        }
        catch
        {
          //Catch potential errors
          //throw new InvalidBase36NumberException(value);
          throw new Exception("InvalidBase36NumberException");
        }
      }
    }

    public static long Base36ToNumber(string Base36Value)
    {
      //Make sure we have passed something
      if (Base36Value == "")
      {
        //throw new InvalidBase36NumberException(Base36Value);
        throw new Exception("InvalidBase36NumberException");
      }

      //Make sure the number is in upper case:
      Base36Value = Base36Value.ToUpper();

      //Account for negative values:
      bool isNegative = false;

      if (Base36Value[0] == '-')
      {
        Base36Value = Base36Value.Substring(1);
        isNegative = true;
      }

      //Loop through our string and calculate its value
      try
      {
        //Keep a running total of the value
        long returnValue = Base36DigitToNumber(Base36Value[Base36Value.Length - 1]);

        //Loop through the character in the string (right to left) and add
        //up increasing powers as we go.
        for (int i = 1; i < Base36Value.Length; i++)
        {
          returnValue += ((long)Math.Pow(36, i) * Base36DigitToNumber(Base36Value[Base36Value.Length - (i + 1)]));
        }

        //Do negative correction if required:
        if (isNegative)
        {
          return returnValue * -1;
        }
        else
        {
          return returnValue;
        }
      }
      catch
      {
        //If something goes wrong, this is not a valid number
        //throw new InvalidBase36NumberException(Base36Value);
        throw new Exception("InvalidBase36NumberException");
      }
    }

    public static string NumberToBase36(long NumericValue)
    {
      try
      {
        //Handle negative values:
        if (NumericValue < 0)
        {
          return string.Concat("-", PositiveNumberToBase36(Math.Abs(NumericValue)));
        }
        else
        {
          return PositiveNumberToBase36(NumericValue);
        }
      }
      catch
      {
        //throw new InvalidBase36ValueException(NumericValue);
        throw new Exception("InvalidBase36ValueException");
      }
    }

    private static string PositiveNumberToBase36(long NumericValue)
    {
      //This is a clever recursively called function that builds
      //the base-36 string representation of the long base-10 value
      if (NumericValue < 36)
      {
        //The get out clause; fires when we reach a number less than 
        //36 - this means we can add the last digit.
        return NumberToBase36Digit((byte)NumericValue).ToString();
      }
      else
      {
        //Add digits from left to right in powers of 36 
        //(recursive)
        return string.Concat(PositiveNumberToBase36(NumericValue / 36), NumberToBase36Digit((byte)(NumericValue % 36)).ToString());
      }
    }

    private static byte Base36DigitToNumber(char Base36Digit)
    {
      //Converts one base-36 digit to it's base-10 value
      if (!char.IsLetterOrDigit(Base36Digit))
      {
        //throw new InvalidBase36DigitException(Base36Digit);
        throw new Exception("InvalidBase36DigitException");
      }

      if (char.IsDigit(Base36Digit))
      {
        //Handles 0 - 9
        return byte.Parse(Base36Digit.ToString());
      }
      else
      {
        //Handles A - Z
        return (byte)((int)Base36Digit - 55);
      }
    }

    private static char NumberToBase36Digit(byte NumericValue)
    {
      //Converts a number to it's base-36 value.
      //Only works for numbers <= 35.
      if (NumericValue > 35)
      {
        //throw new InvalidBase36DigitValueException(NumericValue);
        throw new Exception("InvalidBase36DigitValueException");
      }

      //Numbers:
      if (NumericValue <= 9)
      {
        return NumericValue.ToString()[0];
      }
      else
      {
        //Note that A is code 65, and in this
        //scheme, A = 10.
        return (char)(NumericValue + 55);
      }
    }

    public static bool operator >(Base36 LHS, Base36 RHS)
    {
      return LHS.numericValue > RHS.numericValue;
    }

    public static bool operator <(Base36 LHS, Base36 RHS)
    {
      return LHS.numericValue < RHS.numericValue;
    }

    public static bool operator >=(Base36 LHS, Base36 RHS)
    {
      return LHS.numericValue >= RHS.numericValue;
    }

    public static bool operator <=(Base36 LHS, Base36 RHS)
    {
      return LHS.numericValue <= RHS.numericValue;
    }

    public static bool operator ==(Base36 LHS, Base36 RHS)
    {
      return LHS.numericValue == RHS.numericValue;
    }

    public static bool operator !=(Base36 LHS, Base36 RHS)
    {
      return !(LHS == RHS);
    }

    public static Base36 operator +(Base36 LHS, Base36 RHS)
    {
      return new Base36(LHS.numericValue + RHS.numericValue);
    }

    public static Base36 operator -(Base36 LHS, Base36 RHS)
    {
      return new Base36(LHS.numericValue - RHS.numericValue);
    }

    public static Base36 operator ++(Base36 Value)
    {
      return new Base36(Value.numericValue++);
    }

    public static Base36 operator --(Base36 Value)
    {
      return new Base36(Value.numericValue--);
    }

    public static Base36 operator *(Base36 LHS, Base36 RHS)
    {
      return new Base36(LHS.numericValue * RHS.numericValue);
    }

    public static Base36 operator /(Base36 LHS, Base36 RHS)
    {
      return new Base36(LHS.numericValue / RHS.numericValue);
    }

    public static Base36 operator %(Base36 LHS, Base36 RHS)
    {
      return new Base36(LHS.numericValue % RHS.numericValue);
    }

    public static implicit operator long(Base36 Value)
    {
      return Value.numericValue;
    }

    public static implicit operator int(Base36 Value)
    {
      try
      {
        return (int)Value.numericValue;
      }
      catch
      {
        throw new OverflowException("Overflow: Value too large to return as an integer");
      }
    }

    public static implicit operator short(Base36 Value)
    {
      try
      {
        return (short)Value.numericValue;
      }
      catch
      {
        throw new OverflowException("Overflow: Value too large to return as a short");
      }
    }

    public static implicit operator Base36(long Value)
    {
      return new Base36(Value);
    }

    public static explicit operator string(Base36 Value)
    {
      return Value.Value;
    }

    public static implicit operator Base36(string Value)
    {
      return new Base36(Value);
    }

    public override string ToString()
    {
      return Base36.NumberToBase36(numericValue);
    }

    public override int GetHashCode()
    {
      return numericValue.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Base36))
      {
        return false;
      }
      else
      {
        return this == (Base36)obj;
      }
    }

    public string ToString(int MinimumDigits)
    {
      string base36Value = Base36.NumberToBase36(numericValue);

      if (base36Value.Length >= MinimumDigits)
      {
        return base36Value;
      }
      else
      {
        string padding = new string('0', (MinimumDigits - base36Value.Length));
        return string.Format("{0}{1}", padding, base36Value);
      }
    }
  }
}
