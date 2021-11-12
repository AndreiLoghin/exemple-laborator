using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Exemple.Domain
{
    public record Address
    {
        private static readonly Regex ValidPattern = new("^Str[0-9]{5}$");

        public string Value { get; }

        private Address(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidAdressException("");
            }
        }

        private static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);

        public override string ToString()
        {
            return Value;
        }

        public static bool TryParseAdress(string stringValue, out Address address)
        {
            bool isValid = false;
            address = null;

            if (IsValid(stringValue))
            {
                isValid = true;
                address = new(stringValue);
            }

            return isValid;
        }
    }
}
