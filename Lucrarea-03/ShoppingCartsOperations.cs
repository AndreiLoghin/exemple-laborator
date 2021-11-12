using LanguageExt;
using static LanguageExt.Prelude;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Exemple.Domain.ShoppingCarts;

namespace Exemple.Domain
{
    public static class ShoppingCartsOperations
    {
        public static Task<IShoppingCarts> ValidateShoppingCarts(Func<ProductID, TryAsync<bool>> checkProductExists, Func<ProductID, Quantity, TryAsync<bool>> checkStock, Func<Address, TryAsync<bool>> checkAddress, EmptyShoppingCarts shoppingCarts) =>
            shoppingCarts.ShoppingCartList
                      .Select(ValidateShoppingCart(checkProductExists, checkStock, checkAddress))
                      .Aggregate(CreateEmptyValidatedShoppingCartList().ToAsync(), ReduceValidShoppingCarts)
                      .MatchAsync(
                            Right: validatedShoppingCarts => new ValidatedShoppingCarts(validatedShoppingCarts),
                            LeftAsync: errorMessage => Task.FromResult((IShoppingCarts)new UnvalidatedShoppingCarts(shoppingCarts.ShoppingCartList, errorMessage))
                      );
        private static Func<EmptyShoppingCart, EitherAsync<string, ValidatedShoppingCart>> ValidateShoppingCart(Func<ProductID, TryAsync<bool>> checkProductExists, Func<ProductID, Quantity, TryAsync<bool>> checkStock, Func<Address, TryAsync<bool>> checkAddress) =>
        emptyShoppingCart => ValidateShoppingCart(checkProductExists, checkStock, checkAddress, emptyShoppingCart);

        private static EitherAsync<string, ValidatedShoppingCart> ValidateShoppingCart(Func<ProductID, TryAsync<bool>> checkProductExists, Func<ProductID, Quantity, TryAsync<bool>> checkStock, Func<Address, TryAsync<bool>> checkAddress, EmptyShoppingCart emptyShoppingCart) =>
            from address in Address.TryParse(emptyShoppingCart.address)
                                    .ToEitherAsync(() => $"Invalid address ({emptyShoppingCart.ProductID}, {emptyShoppingCart.address})")
            from ProductID in ProductID.TryParse(emptyShoppingCart.ProductID)
                                    .ToEitherAsync(() => $"Invalid product ID ({emptyShoppingCart.ProductID})")
            from quantity in Quantity.TryParse(emptyShoppingCart.quantity)
                                    .ToEitherAsync(() => $"Invalid quantity ({emptyShoppingCart.ProductID}, {emptyShoppingCart.quantity})")
            from price in PriceCalculation.TryParse(emptyShoppingCart.price)
                        .ToEitherAsync(() => $"Invalid price ({emptyShoppingCart.ProductID}, {emptyShoppingCart.price})")
            from productExists in checkProductExists(ProductID)
                                    .ToEither(error => error.ToString())
            from stockOK in checkStock(ProductID, quantity)
                                    .ToEither(error => error.ToString())
            from addressOK in checkAddress(address)
                                    .ToEither(error => error.ToString())
            select new ValidatedShoppingCart(ProductID, quantity, address, price);

        private static Either<string, List<ValidatedShoppingCart>> CreateEmptyValidatedShoppingCartList() =>
            Right(new List<ValidatedShoppingCart>());

        private static EitherAsync<string, List<ValidatedShoppingCart>> ReduceValidShoppingCarts(EitherAsync<string, List<ValidatedShoppingCart>> acc, EitherAsync<string, ValidatedShoppingCart> next) =>
            from list in acc
            from nextShoppingCart in next
            select list.AppendValidShoppingCart(nextShoppingCart);

        private static List<ValidatedShoppingCart> AppendValidShoppingCart(this List<ValidatedShoppingCart> list, ValidatedShoppingCart validShoppingCart)
        {
            list.Add(validShoppingCart);
            return list;
        }

        public static IShoppingCarts CalculateFinalPrices(IShoppingCarts shoppingCarts) => shoppingCarts.Match(
            whenEmptyShoppingCarts: emptyShoppingCart => emptyShoppingCart,
            whenUnvalidatedShoppingCarts: unvalidatedShoppingCart => unvalidatedShoppingCart,
            whenCalculatedShoppingCarts: calculatedShoppingCart => calculatedShoppingCart,
            whenPaidShoppingCarts: paidShoppingCart => paidShoppingCart,
            whenValidatedShoppingCarts: CalculateFinalPrice
        );

        private static IShoppingCarts CalculateFinalPrice(ValidatedShoppingCarts validShoppingCarts) =>
            new CalculatedShoppingCarts(validShoppingCarts.ShoppingCartList
                                                          .Select(CalculateShoppingCartFinalPrice)
                                                          .ToList()
                                                          .AsReadOnly());
        private static CalculatedShoppingCart CalculateShoppingCartFinalPrice(ValidatedShoppingCart validShoppingCart) =>
            new CalculatedShoppingCart(validShoppingCart.ProductID,
                                      validShoppingCart.quantity,
                                      validShoppingCart.address,
                                      validShoppingCart.price,
                                      validShoppingCart.price * validShoppingCart.quantity);
        public static IShoppingCarts PayShoppingCarts(IShoppingCarts shoppingCarts) => shoppingCarts.Match(
            whenEmptyShoppingCarts: emptyShoppingCart => emptyShoppingCart,
            whenUnvalidatedShoppingCarts: unvalidatedShoppingCart => unvalidatedShoppingCart,
            whenPaidShoppingCarts: paidShoppingCart => paidShoppingCart,
            whenValidatedShoppingCarts: validatedShoppingCart => validatedShoppingCart,
            whenCalculatedShoppingCarts: GenerateExport
        );

        private static IShoppingCarts GenerateExport(CalculatedShoppingCarts calculatedShoppingCart) =>
            new PaidShoppingCarts(calculatedShoppingCart.ShoppingCartList,
                                    calculatedShoppingCart.ShoppingCartList.Aggregate(new StringBuilder(), CreateCsvLine).ToString(),
                                    DateTime.Now);

        private static StringBuilder CreateCsvLine(StringBuilder export, CalculatedShoppingCart shoppingCart) =>
            export.AppendLine($"{shoppingCart.ProductID.IDValue}, {shoppingCart.price}, {shoppingCart.quantity}, {shoppingCart.finalPrice}");
    }
}