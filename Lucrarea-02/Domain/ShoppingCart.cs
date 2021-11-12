using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain
{
    [AsChoice]
    public static partial class ShoppingCart
    {
        public interface IShoppingCart { }

        public record UnvalidatedSCart : IShoppingCart
        {
            public UnvalidatedSCart(IReadOnlyCollection<UnvalidatedCart> productsList)
            {
                ProductsList = productsList;
            }

            public IReadOnlyCollection<UnvalidatedCart> ProductsList { get; }
        }

        public record InvalidatedSCart : IShoppingCart
        {
            internal InvalidatedSCart(IReadOnlyCollection<UnvalidatedCart> productsList, string reason)
            {
                ProductsList = productsList;
                Reason = reason;
            }

            public IReadOnlyCollection<UnvalidatedCart> ProductsList { get; }
            public string Reason { get; }
        }

        public record ValidatedSCart : IShoppingCart
        {
            internal ValidatedSCart(IReadOnlyCollection<ValidatedCart> productsList)
            {
                ProductsList = productsList;
            }

            public IReadOnlyCollection<ValidatedCart> ProductsList { get; }
        }

        public record CalculatedSCart: IShoppingCart
        {
            internal CalculatedSCart(IReadOnlyCollection<PriceCalculation> productsList)
            {
                ProductsList = productsList;
            }

            public IReadOnlyCollection<PriceCalculation> ProductsList { get; }
        }

        public record PublishedSCart: IShoppingCart
        {
            internal PublishedSCart(IReadOnlyCollection<PriceCalculation> productsList, string csv, DateTime publishedDate)
            {
                ProductsList = productsList;
                PublishedDate = publishedDate;
                Csv = csv;
            }

            public IReadOnlyCollection<PriceCalculation> ProductsList { get; }
            public DateTime PublishedDate { get; }
            public string Csv { get; }
        }
    
    }
}
