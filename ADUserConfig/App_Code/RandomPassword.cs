using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Summary description for RandomPassword
/// </summary>
public class RandomPassword
{
    private GetRandomChar grc = new GetRandomChar();
    private CryptoRandom rnd = new CryptoRandom();
    private StringBuilder password = new StringBuilder();

    private Regex containsLowerCase = new Regex("[a-z]");
    private Regex containsUpperCase = new Regex("[A-Z]");
    private Regex containsSpecial = new Regex(@"[\W]");
    private Regex containsNumber = new Regex("[0-9]");

    //Lowercase min and max
    #region lowerCaseMinMax
    private int _lowerCaseMin = 0;
    public int lowerCaseMin
    {
        get { return _lowerCaseMin; }
        set
        {
            if (_lowerCaseMax != -1 && value > _lowerCaseMax)
                throw new RuleIsOutOfRangeException(RuleIsOutOfRangeException.BrokenRule.lowerCaseMin);
            else
                _lowerCaseMin = value < 0 ? 0 : value;
        }
    }

    private int _lowerCaseMax = -1;
    public int lowerCaseMax
    {
        get { return _lowerCaseMax; }
        set
        {
            if (value != -1 && value < _lowerCaseMin)
                throw new RuleIsOutOfRangeException(RuleIsOutOfRangeException.BrokenRule.lowerCaseMax);
            else
                _lowerCaseMax = value < 0 ? -1 : value;
        }
    }
    #endregion

    //Uppercase min and max
    #region UppercaseMinMax
    private int _upperCaseMin = 0;
    public int upperCaseMin
    {
        get { return _upperCaseMin; }
        set
        {
            if (_upperCaseMax != -1 && value > _upperCaseMax)
                throw new RuleIsOutOfRangeException(RuleIsOutOfRangeException.BrokenRule.upperCaseMin);
            else
                _upperCaseMin = value < 0 ? 0 : value;
        }
    }

    private int _upperCaseMax = -1;
    public int upperCaseMax
    {
        get { return _upperCaseMax; }
        set
        {
            if (value != -1 && value < _upperCaseMin)
                throw new RuleIsOutOfRangeException(RuleIsOutOfRangeException.BrokenRule.upperCaseMax);
            else
                _upperCaseMax = value < 0 ? -1 : value;
        }
    }
    #endregion

    //Special min and max
    #region SpecialMinMax
    private int _specialMin = 0;
    public int specialMin
    {
        get { return _specialMin; }
        set
        {
            if (_specialMax != -1 && value > _specialMax)
                throw new RuleIsOutOfRangeException(RuleIsOutOfRangeException.BrokenRule.specialMin);
            else
                _specialMin = value < 0 ? 0 : value;
        }
    }

    private int _specialMax = -1;
    public int specialMax
    {
        get { return _specialMax; }
        set
        {
            if (value != -1 && value < _specialMin)
                throw new RuleIsOutOfRangeException(RuleIsOutOfRangeException.BrokenRule.specialMax);
            else
                _specialMax = value < 0 ? -1 : value;
        }
    }
    #endregion

    //NumberMinMax
    #region NumberMinMax
    private int _numberMin = 0;
    public int numberMin
    {
        get { return _numberMin; }
        set
        {
            if (_numberMax != -1 && value > _numberMax)
                throw new RuleIsOutOfRangeException(RuleIsOutOfRangeException.BrokenRule.numberMin);
            else
                _numberMin = value < 0 ? 0 : value;
        }
    }

    private int _numberMax = -1;
    public int numberMax
    {
        get { return _numberMax; }
        set
        {
            if (value != -1 && value < _numberMin)
                throw new RuleIsOutOfRangeException(RuleIsOutOfRangeException.BrokenRule.numberMax);
            else
                _numberMax = value < 0 ? -1 : value;
        }
    }
    #endregion

    public RandomPassword()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public RandomPassword(int lowercaseMin, int lowercaseMax, int uppercaseMin, int uppercaseMax, int specialMin, int specialMax, int numberMin, int numberMax)
    {
        this.lowerCaseMin = lowercaseMin;
        this.lowerCaseMax = lowercaseMax;
        this.upperCaseMin = uppercaseMin;
        this.upperCaseMax = uppercaseMax;
        this.specialMin = specialMin;
        this.specialMax = specialMax;
        this.numberMin = numberMin;
        this.numberMax = numberMax;
    }

    /// <summary>
    /// Generates a random password
    /// </summary>
    /// <param name="length">The length of the password.</param>
    /// <returns>A random password</returns>
    public string GeneratePassword(int length)
    {
        if (!EnoughMaxes(length))
            throw new RuleIsOutOfRangeException(RuleIsOutOfRangeException.BrokenRule.maxesNotEnough);

        password.Clear();

        while (containsLowerCase.Matches(password.ToString()).Count < lowerCaseMin)
            InsertIntoPassword(charType.lowerCase);

        while (containsUpperCase.Matches(password.ToString()).Count < upperCaseMin)
            InsertIntoPassword(charType.upperCase);

        while (containsSpecial.Matches(password.ToString()).Count < specialMin)
            InsertIntoPassword(charType.special);

        while (containsNumber.Matches(password.ToString()).Count < numberMin)
            InsertIntoPassword(charType.number);

        while (password.Length < length)
        {
            InsertIntoPassword();
        }

        return password.ToString();
    }

    private void InsertIntoPassword(charType type)
    {
        if (!EnoughOfType(type))
            password.Insert(rnd.Next(password.Length + 1), grc.GetChar(type));
        else
            grc.RemoveType(type);
    }

    private void InsertIntoPassword()
    {
        CharWithType newChar = grc.GetChar();

        if (!EnoughOfType(newChar.type))
            password.Insert(rnd.Next(password.Length + 1), newChar.character);
        else
            grc.RemoveType(newChar.type);
    }

    private bool EnoughOfType(charType type)
    {
        switch (type)
        {
            case charType.lowerCase:
                return _lowerCaseMax != -1 && containsLowerCase.Matches(password.ToString()).Count >= _lowerCaseMax;
            case charType.upperCase:
                return _upperCaseMax != -1 && containsUpperCase.Matches(password.ToString()).Count >= _upperCaseMax;
            case charType.special:
                return _specialMax != -1 && containsSpecial.Matches(password.ToString()).Count >= _specialMax;
            case charType.number:
                return _numberMax != -1 && containsNumber.Matches(password.ToString()).Count >= _numberMax;
            default:
                return false;
        }
    }

    private bool EnoughMaxes(int length)
    {
        if (_lowerCaseMax == -1 || _upperCaseMax == -1 || _specialMax == -1 || _numberMax == -1)
            return true;

        return length <= (_lowerCaseMax + _upperCaseMax + _specialMax + _numberMax);
    }

    private enum charType
    {
        lowerCase,
        upperCase,
        special,
        number
    }

    private class GetRandomChar
    {
        //Removed the following from possible characters:
        //0 - Zero since it is hard to distinguish from the uppercase o
        //O - Uppercase o since it is hard to distinguish from the number 0
        //I - Uppercase i since it is hard to distinguish from lowercase L
        //l - Lowercase L since it is hard to distinguish from uppercase i
        //_ - Doesnt get recognized ny Regex [\W]
        //Multiple special characters - Not all people know where to find them on the keyboard
        private const string LOWERCASE = "abcdefghijkmnopqrstuvwxyz";
        private const string UPPERCASE = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        private const string SPECIAL = "*-+?&=!%#";
        private const string NUMBER = "123456789";
        private CryptoRandom rnd = new CryptoRandom();

        private string availableChars
        {
            get
            {
                string result = "";

                foreach (charType type in types)
                {
                    switch (type)
                    {
                        case charType.lowerCase:
                            result += LOWERCASE;
                            break;
                        case charType.upperCase:
                            result += UPPERCASE;
                            break;
                        case charType.special:
                            result += SPECIAL;
                            break;
                        case charType.number:
                            result += NUMBER;
                            break;
                        default:
                            break;
                    }
                }

                return result;
            }
        }

        private List<charType> types = new List<charType>(new charType[] { charType.lowerCase, charType.upperCase, charType.special, charType.number });
        public List<charType> CurrentTypes
        {
            get
            {
                return types;
            }
        }

        public CharWithType GetChar()
        {
            charType type = (charType)4;

            char charToReturn = availableChars[rnd.Next(availableChars.Length)];

            foreach (charType t in types)
            {
                if (LOWERCASE.Contains(charToReturn))
                {
                    type = charType.lowerCase;
                    break;
                }
                else if (UPPERCASE.Contains(charToReturn))
                {
                    type = charType.upperCase;
                    break;
                }
                else if (SPECIAL.Contains(charToReturn))
                {
                    type = charType.special;
                    break;
                }
                else if (NUMBER.Contains(charToReturn))
                {
                    type = charType.number;
                    break;
                }
            }

            return new CharWithType(charToReturn, type);
        }

        public char GetChar(charType type)
        {
            switch (type)
            {
                case charType.lowerCase:
                    return LOWERCASE[rnd.Next(LOWERCASE.Length)];
                case charType.upperCase:
                    return UPPERCASE[rnd.Next(UPPERCASE.Length)];
                case charType.special:
                    return SPECIAL[rnd.Next(SPECIAL.Length)];
                case charType.number:
                    return NUMBER[rnd.Next(NUMBER.Length)];
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void RemoveType(charType type)
        {
            types.Remove(type);
        }

        private void fillTypes()
        {
            types.Clear();
            types.AddRange(new charType[] { charType.lowerCase, charType.upperCase, charType.special, charType.number });
        }
    }

    private class CharWithType
    {
        public char character { get; set; }
        public charType type { get; set; }

        public CharWithType(char character, charType type)
        {
            this.character = character;
            this.type = type;
        }
    }

    /// <summary>
    /// Exception for broken rule
    /// </summary>
    public class RuleIsOutOfRangeException : Exception
    {
        private BrokenRule _rule;

        public RuleIsOutOfRangeException(BrokenRule rule)
        {
            _rule = rule;
        }

        public override string Message
        {
            get
            {
                switch (_rule)
                {
                    case BrokenRule.lowerCaseMin:
                        return "Minimum lowercase count is higher than maximum lowercase count";
                    case BrokenRule.lowerCaseMax:
                        return "Maximum lowercase count is lower than minimum lowercase count";
                    case BrokenRule.upperCaseMin:
                        return "Minimum uppercase count is higher than maximum uppercase count";
                    case BrokenRule.upperCaseMax:
                        return "Maximum uppercase count is lower than minimum uppercase count";
                    case BrokenRule.specialMin:
                        return "Minimum special character count is higher than maximum special character count";
                    case BrokenRule.specialMax:
                        return "Maximum special character count is lower than minimum special character count";
                    case BrokenRule.numberMin:
                        return "Minimum number count is higher than maximum number count";
                    case BrokenRule.numberMax:
                        return "Maximum number count is lower than minimum number count";
                    case BrokenRule.maxesNotEnough:
                        return "Max rules equals less than length. Password can't be created.";
                    default:
                        return "";
                }
            }
        }

        public enum BrokenRule
        {
            lowerCaseMin,
            lowerCaseMax,
            upperCaseMin,
            upperCaseMax,
            specialMin,
            specialMax,
            numberMin,
            numberMax,
            maxesNotEnough
        }
    }
}

/// <summary>
/// Summary description for CryptoRandom
/// (Not my work. Taken from http://thinketg.com/how-to-generate-better-random-numbers-in-c-net-2/)
/// </summary>
public class CryptoRandom : RandomNumberGenerator
{
    private static RandomNumberGenerator r;

    ///<summary>
    /// Creates an instance of the default implementation of a cryptographic random number generator that can be used to generate random data.
    ///</summary>
    public CryptoRandom()
    {
        r = RandomNumberGenerator.Create();
    }

    ///<summary>
    /// Fills the elements of a specified array of bytes with random numbers.
    ///</summary>
    ///<param name="buffer">An array of bytes to contain random numbers.</param>
    public override void GetBytes(byte[] buffer)
    {
        r.GetBytes(buffer);
    }

    ///<summary>
    /// Returns a random number between 0.0 and 1.0.
    ///</summary>
    public double NextDouble()
    {
        byte[] b = new byte[4];
        r.GetBytes(b);
        return (double)BitConverter.ToUInt32(b, 0) / UInt32.MaxValue;
    }

    ///<summary>
    /// Returns a random number within the specified range.
    ///</summary>
    ///<param name="minValue">The inclusive lower bound of the random number returned.</param>
    ///<param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
    public int Next(int minValue, int maxValue)
    {
        return (int)Math.Round(NextDouble() * (maxValue - minValue - 1)) + minValue;
    }

    ///<summary>
    /// Returns a nonnegative random number.
    ///</summary>
    public int Next()
    {
        return Next(0, Int32.MaxValue);
    }

    ///<summary>
    /// Returns a nonnegative random number less than the specified maximum
    ///</summary>
    ///<param name="maxValue">The inclusive upper bound of the random number returned. maxValue must be greater than or equal 0</param>
    public int Next(int maxValue)
    {
        return Next(0, maxValue);
    }
}