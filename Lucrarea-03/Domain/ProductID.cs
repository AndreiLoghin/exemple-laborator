using LanguageExt;
using static LanguageExt.Prelude;
using System.Text.RegularExpressions;

namespace Exemple.Domain
{
    public record ProductID
    {
        private static readonly Regex ValidPattern = new("^[0-9]*$");

        public string IDValue { get; }

        public ProductID(string value)
        {
            if (ValidPattern.IsMatch(value))
            {
                IDValue = value;
            }
            else
            {
                throw new InvalidProductIDException("");
            }
        }

        public override string ToString()
        {
            return IDValue;
        }

        private static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);

        public static Option<ProductID> TryParse(string productIDString)
        {
            if (IsValid(productIDString))
            {
                return Some<ProductID>(new(productIDString));
            }
            else
            {
                return None;
            }
        }
    }
}