using System.Text.RegularExpressions;

namespace LAB1_PSSC_LA.Domain
{
    public record ProductID
    {
        private static readonly Regex ValidPattern = new("^C[0-9]{4}$");

        public string Value { get; }

        private ProductID(string value)
        {
            if (ValidPattern.IsMatch(value))
            {
                Value = value;
            }
            else
            {
                throw new ProductIDException($"{value} is an invalid ProductID.");
            }
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
