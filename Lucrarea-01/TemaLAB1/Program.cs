using LAB1_PSSC_LA.Domain;
using System;
using System.Collections.Generic;
using static LAB1_PSSC_LA.Domain.ShoppingCarts;

namespace LAB1_PSSC_LA
{
    class Program
    {
        private static readonly Random random = new Random();

        static void Main(string[] args)
        {
            string question = ReadValue("You want to start shopping?: ");
            if (question.Contains("Yes"))
            {
                var listOfShoppingCarts = ReadListOfShoppingCarts().ToArray();

                ShoppingCarts.EmptyShoppingCart emptyShoppingCarts = new(listOfShoppingCarts);

                IShoppingCarts result = ValidateShoppingCarts(emptyShoppingCarts);

                result.Match(
                    whenEmptyShoppingCart: emptyResult => emptyShoppingCarts,
                    whenUnvalidatedShoppingCart: unvalidatedResult => unvalidatedResult,
                    whenPaidShoppingCart: paidResult => paidResult,
                    whenValidatedShoppingCart: validatedResult => PayShoppingCart(validatedResult)
                );

                Console.WriteLine(result);

            }
            Console.WriteLine("Hello World!");
        }

        private static List<ShoppingCarts.EmptyShoppingCart> ReadListOfShoppingCarts()
        {
            List<ShoppingCarts.EmptyShoppingCart> listOfShoppingCarts = new();
            object question2 = null;
            do
            {
                question2 = ReadValue("To add a product type 'Yes': ");
                if (question2.Equals("Yes"))
                {
                    
                    var product_quantity = ReadValue("Quantity: ");
                    if (string.IsNullOrEmpty(product_quantity))
                    {
                        break;
                    }

                    var product_ID = ReadValue("Product ID: ");
                    if (string.IsNullOrEmpty(product_ID))
                    {
                        break;
                    }

                    var address = ReadValue("Address: ");
                    if (string.IsNullOrEmpty(address))
                    {
                        break;
                    }
                    
                    listOfShoppingCarts.Add( new (product_quantity, product_ID, address));
                }
            } while (!question2.Equals("Nu"));

            return listOfShoppingCarts;
        }

        private static IShoppingCarts ValidateShoppingCarts(ShoppingCarts.EmptyShoppingCart emptyShoppingCarts) => random.Next(100) > 50 ?
            new global::LAB1_PSSC_LA.Domain.ShoppingCarts.UnvalidatedShoppingCart(new global::System.Collections.Generic.List<global::LAB1_PSSC_LA.Domain.ShoppingCarts.UnvalidatedShoppingCart>(), "Random errror")
            : new global::LAB1_PSSC_LA.Domain.ShoppingCarts.ValidatedShoppingCart(new global::System.Collections.Generic.List<global::LAB1_PSSC_LA.Domain.ShoppingCarts.ValidatedShoppingCart>());


        private static IShoppingCarts PayShoppingCart(ShoppingCarts.ValidatedShoppingCart validExamGrades) =>
            new ShoppingCarts.PaidShoppingCart(new List<ShoppingCarts.ValidatedShoppingCart>(), DateTime.Now);

        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
