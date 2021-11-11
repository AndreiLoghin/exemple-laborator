using System.Text.RegularExpressions;

namespace LAB1_PSSC_LA.Domain
{
    class Address
    {

        public string Value { get; }

        private Address(string address)
        {
            Value = address;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
