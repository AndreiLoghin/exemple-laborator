using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Exemple.Domain.ShoppingCartsPaidEvent;
using static Exemple.Domain.ShoppingCarts;
using static Exemple.Domain.ShoppingCartsOperations;
using LanguageExt;

namespace Exemple.Domain
{
    public class PayShoppingCartWorkflow
    {
        public async Task<IShoppingCartsPaidEvent> ExecuteAsync(PayShoppingCartCommand command, Func<ProductID, TryAsync<bool>> checkProductExists, Func<ProductID, Quantity, TryAsync<bool>> checkStock, Func<Address, TryAsync<bool>> checkAddress)
        {
            EmptyShoppingCarts emptyShoppingCarts = new EmptyShoppingCarts(command.InputShoppingCarts);
            IShoppingCarts shoppingCarts = await ValidateShoppingCarts(checkProductExists, checkStock, checkAddress, emptyShoppingCarts);
            shoppingCarts = CalculateFinalPrices(shoppingCarts);
            shoppingCarts = PayShoppingCarts(shoppingCarts);

            return shoppingCarts.Match(
                    whenEmptyShoppingCarts: emptyShoppingCarts => new ShoppingCartsPaidFailedEvent("Unexpected unvalidated state") as IShoppingCartsPaidEvent,
                    whenUnvalidatedShoppingCarts: unvalidatedShoppingCarts => new ShoppingCartsPaidFailedEvent(unvalidatedShoppingCarts.Reason),
                    whenValidatedShoppingCarts: validatedShoppingCarts => new ShoppingCartsPaidFailedEvent("Unexpected validated state"),
                    whenCalculatedShoppingCarts: calculatedShoppingCarts => new ShoppingCartsPaidFailedEvent("Unexpected calculated state"),
                    whenPaidShoppingCarts: paidShoppingCarts => new ShoppingCartsPaidScucceededEvent(paidShoppingCarts.Csv, paidShoppingCarts.PublishedDate)
                );
        }
    }
}
