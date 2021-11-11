using CSharp.Choices;
using System;
using System.Collections.Generic;

namespace LAB1_PSSC_LA.Domain
{
    [AsChoice]
    public static partial class ShoppingCarts
    {
        public interface IShoppingCarts { }

        public record EmptyShoppingCart(IReadOnlyCollection<EmptyShoppingCart> ShoppingCarts) : IShoppingCarts;

        public record UnvalidatedShoppingCart(IReadOnlyCollection<UnvalidatedShoppingCart> ShoppingCarts, string reason) : IShoppingCarts;

        public record ValidatedShoppingCart(IReadOnlyCollection<ValidatedShoppingCart> ShoppingCarts) : IShoppingCarts;

        public record PaidShoppingCart(IReadOnlyCollection<ValidatedShoppingCart> ShoppingCarts, DateTime PublishedDate) : IShoppingCarts;
    }
}
