namespace LAB1_PSSC_LA.Domain
{
    public record ProductsQuantity
    {
        public int Value { get; }

        public ProductsQuantity(int value)
        {
            if (value > 0 && value < 100)
            {
                Value = value;
            }
            else
            {
                throw new ProductsQuantityException($"{value} is an invalid quantity value.");
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
