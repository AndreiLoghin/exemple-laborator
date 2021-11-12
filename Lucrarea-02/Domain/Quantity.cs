using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain
{
    public record Quantity
    {
        public decimal Value { get; }

        public Quantity(decimal value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidAmountException($"{value:0.##} is an invalid quantity value.");
            }
        }

        public static Quantity operator *(Quantity a, Quantity b) => new Quantity((a.Value * b.Value));

        public Quantity Round()
        {
            var roundedValue = Math.Round(Value);
            return new Quantity(roundedValue);
        }

        public override string ToString()
        {
            return $"{Value:0.##}";
        }

        public static bool TryParseAmount(string amountString, out Quantity quantity)
        {
            bool isValid = false;
            quantity = null;
            if(decimal.TryParse(amountString, out decimal numericAmount))
            {
                if (IsValid(numericAmount))
                {
                    isValid = true;
                    quantity = new(numericAmount);
                }
            }
            return isValid;
        }

        private static bool IsValid(decimal numericAmount) => numericAmount > 0 && numericAmount <= 20;
    }
}
