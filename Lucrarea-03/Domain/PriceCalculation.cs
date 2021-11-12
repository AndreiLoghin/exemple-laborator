using LanguageExt;
using static LanguageExt.Prelude;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exemple.Domain;

namespace Exemple.Domain
{
    public record PriceCalculation
    {
        public decimal Value { get; }

        public PriceCalculation(decimal value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidPriceException($"{value:0.##} is an invalid price value.");
            }
        }

        public static PriceCalculation operator *(PriceCalculation a, Quantity b) => new PriceCalculation((a.Value * b.Value));

        public PriceCalculation Round()
        {
            var roundedValue = Math.Round(Value);
            return new PriceCalculation(roundedValue);
        }

        public override string ToString()
        {
            return $"{Value:0.##}";
        }

        public static Option<PriceCalculation> TryParse(string priceString)
        {
            if (decimal.TryParse(priceString, out decimal numericPrice) && IsValid(numericPrice))
            {
                return Some<PriceCalculation>(new(numericPrice));
            }
            else
            {
                return None;
            }
        }

        private static bool IsValid(decimal numericPrice) => numericPrice >= 0;
    }
}