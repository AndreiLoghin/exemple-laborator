using LanguageExt;
using static LanguageExt.Prelude;

using System.Text.RegularExpressions;

namespace Exemple.Domain
{
    public record Address
    {
        private static readonly Regex ValidPattern = new("^Str[0-9]{2}$");

        public string Value { get; }

        public Address(string address)
        {
            if (ValidPattern.IsMatch(address))
            {
                Value = address;
            }
            else
            {
                throw new InvalidAddressException("");
            }
        }

        public override string ToString()
        {
            return Value;
        }
        private static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);

        public static Option<Address> TryParse(string addressString)
        {
            if (IsValid(addressString))
            {
                return Some<Address>(new(addressString));
            }
            else
            {
                return None;
            }
        }

    }
}