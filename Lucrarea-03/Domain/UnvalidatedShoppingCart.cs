namespace Exemple.Domain
{
    public record UnvalidatedShoppingCart(ProductID ProductID, Quantity quantity, Address address, PriceCalculation price);
}
