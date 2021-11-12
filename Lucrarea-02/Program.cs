using Exemple.Domain;
using System;
using System.Collections.Generic;
using static Exemple.Domain.ShoppingCart;
using static Exemple.Domain.ShoppingCartOperation;

namespace Exemple
{
    class Program
    {
        private static readonly Random random = new Random();

        static void Main(string[] args)
        {
            var listOfProductID = ReadListOfProduct().ToArray();
            PublishShoppingCartCommand command = new(listOfProductID);
            PublishCartWorkflow workflow = new PublishCartWorkflow();
            var result = workflow.Execute(command, (productID) => true);

            result.Match(
                whenShoppingCartPublishFaildEvent: @event =>
                {
                    Console.WriteLine($"Publish failed: {@event.Reason }");
                    return @event;
                },
                whenShoppingCartPublishSucceededEvent: @event =>
                {
                    Console.WriteLine($"Publish succeded.");
                    Console.WriteLine(@event.Csv);
                    return @event;
                }
           );

            Console.WriteLine("Goodbye!");
        }

        private static List<UnvalidatedCart> ReadListOfProduct()
        {
            List<UnvalidatedCart> listOfProducts = new();
            do
            {
                var ProductID = ReadValue("Product ID: ");
                if (string.IsNullOrEmpty(ProductID))
                {
                    break;
                }

                var Address = ReadValue("Address: ");
                if (string.IsNullOrEmpty(Address))
                {
                    break;
                }

                var Quantity = ReadValue("Quantity: ");
                if (string.IsNullOrEmpty(Quantity))
                {
                    break;
                }

                var Price = ReadValue("Price: ");
                if (string.IsNullOrEmpty(Price))
                {
                    break;
                }

                listOfProducts.Add(new(ProductID, Address, Quantity, Price));

                string question = ReadValue("Continue shopping? ('N' to stop)");
                if (question.Contains('N'))
                {
                    break;
                }
                
            } while (true);
            return listOfProducts;
        }

        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
