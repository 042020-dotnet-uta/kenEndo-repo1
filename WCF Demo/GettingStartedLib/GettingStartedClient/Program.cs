using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GettingStartedClient.ServiceReference1;

namespace GettingStartedClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //Step 1: Create an instance of the WCF proxy.
            CalculatorClient client = new CalculatorClient();

            // Step 2: Call the service operations.
            // Call the Add service operation.
            string fruit = "apple";
            int count = 10;
            int result = client.Add(fruit, count);
            Console.WriteLine("Adding {0} {1} to the fridge. There is {2} {1} in the fridge", count,fruit,result);

            // Call the Subtract service operation.
            fruit = "apple";
            count = 2;
            result = client.Subtract(fruit, count);
            Console.WriteLine("Taking {0} {1} from the fridge. There are {2} {1} left in the fridge", count, fruit,result);

            // Call the Multiply service operation.
            fruit = "apple";
            result = client.Get(fruit);
            Console.WriteLine("There is {1} {0} in the fridge", fruit, result);

            // Step 3: Close the client to gracefully close the connection and clean up resources.
            Console.WriteLine("\nPress <Enter> to terminate the client.");
            Console.ReadLine();
            client.Close();
        }
    }
}