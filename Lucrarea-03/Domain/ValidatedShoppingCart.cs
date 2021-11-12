using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain
{
    public record ValidatedShoppingCart(ProductID ProductID, Quantity quantity, Address address, PriceCalculation price);
}