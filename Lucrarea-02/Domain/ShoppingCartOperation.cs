using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Exemple.Domain.ShoppingCart;

namespace Exemple.Domain
{
    public static class ShoppingCartOperation
    {
        public static IShoppingCart ValidateShoppingCart(Func<ProductID, bool> checkProductExists, UnvalidatedSCart shoppingCart)
        {
            List<ValidatedCart> validatedShoppingCart = new();
            bool isValidList = true;
            string invalidReson = string.Empty;
            foreach (var unvalidatedShoppingCart in shoppingCart.ProductsList)
            {
                if(!Address.TryParseAdress(unvalidatedShoppingCart.Address, out Address address))
                {
                    invalidReson = $"Invalid address ({unvalidatedShoppingCart.ProductID}, {unvalidatedShoppingCart.Address})";
                    isValidList = false;
                    break;
                }

                if (!Quantity.TryParseAmount(unvalidatedShoppingCart.Quantity, out Quantity quantity))
                {
                    invalidReson = $"Invalid quantity ({unvalidatedShoppingCart.ProductID}, {unvalidatedShoppingCart.Quantity})";
                    isValidList = false;
                    break;
                }
                if (!Quantity.TryParseAmount(unvalidatedShoppingCart.Price, out Quantity price))
                {
                    invalidReson = $"Invalid price ({unvalidatedShoppingCart.ProductID}, {unvalidatedShoppingCart.Price})";
                    isValidList = false;
                    break;
                }
                if (!ProductID.TryParseProductID(unvalidatedShoppingCart.ProductID, out ProductID productID))
                {
                    invalidReson = $"Invalid productID ({unvalidatedShoppingCart.ProductID})";
                    isValidList = false;
                    break;
                }
                ValidatedCart validCart = new(address, productID, quantity, price);
                validatedShoppingCart.Add(validCart);
            }

            if (isValidList)
            {
                return new ValidatedSCart(validatedShoppingCart);
            }
            else
            {
                return new InvalidatedSCart(shoppingCart.ProductsList, invalidReson);
            }

        }

        public static IShoppingCart CalculateFinalPrice(IShoppingCart shoppingCart) => shoppingCart.Match(
            whenUnvalidatedSCart: unvalidatedCart => unvalidatedCart,
            whenInvalidatedSCart: invalidCart => invalidCart,
            whenCalculatedSCart: calculatedCart => calculatedCart,
            whenPublishedSCart: publishedCart => publishedCart,
            whenValidatedSCart: validCart =>
            {
                var calculatedPrice = validCart.ProductsList.Select(validPrice =>
                                            new PriceCalculation(validPrice.ProductID,
                                                                      validPrice.Address,
                                                                      validPrice.Quantity,
                                                                      validPrice.Price,
                                                                      validPrice.Quantity * validPrice.Price));
                return new CalculatedSCart(calculatedPrice.ToList().AsReadOnly());
            }
        );

        public static IShoppingCart PublishSCart(IShoppingCart shoppingCart) => shoppingCart.Match(
            whenUnvalidatedSCart: unvalidatedCart => unvalidatedCart,
            whenInvalidatedSCart: invalidCart => invalidCart,
            whenValidatedSCart: validatedCart => validatedCart,
            whenPublishedSCart: publishedCart => publishedCart,
            whenCalculatedSCart: calculatedCart =>
            {
                StringBuilder csv = new();
                calculatedCart.ProductsList.Aggregate(csv, (export, cart) => export.AppendLine($"{cart.ProductID.Value}, {cart.Address}, {cart.Quantity}, {cart.Price}, {cart.FinalPrice}"));

                PublishedSCart publishedCart = new(calculatedCart.ProductsList, csv.ToString(), DateTime.Now);

                return publishedCart;
            });
    }
}
