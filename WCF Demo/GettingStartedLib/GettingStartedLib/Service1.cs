using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace GettingStartedLib
{
    public class CalculatorService : ICalculator
    {



        /*        public double Add(int n1)
                {
                    double result = n1 + n2;
                    Console.WriteLine("Received Add({0},{1})", n1, n2);
                    // Code added to write output to the console window.
                    Console.WriteLine("Return: {0}", result);
                    return result;
                }

                public double Subtract(double n1, double n2)
                {
                    double result = n1 - n2;
                    Console.WriteLine("Received Subtract({0},{1})", n1, n2);
                    Console.WriteLine("Return: {0}", result);
                    return result;
                }

                public double Multiply(double n1, double n2)
                {
                    double result = n1 * n2;
                    Console.WriteLine("Received Multiply({0},{1})", n1, n2);
                    Console.WriteLine("Return: {0}", result);
                    return result;
                }

                public double Divide(double n1, double n2)
                {
                    double result = n1 / n2;
                    Console.WriteLine("Received Divide({0},{1})", n1, n2);
                    Console.WriteLine("Return: {0}", result);
                    return result;
                }
        */
        public static Dictionary<string, int> fridge = new Dictionary<string, int>();
        public int Add(string fruit, int count)
        {
            if (!fridge.ContainsKey(fruit))
            {
                fridge.Add(fruit, count);
                return count;
            }
            else
            {
                fridge[fruit] += count;
                return fridge[fruit];
            }
        }

        public int Subtract(string fruit, int count)
        {
            if (!fridge.ContainsKey(fruit))
            {
                return 0;
            }
            else
            {
                fridge[fruit] -= count;
                return fridge[fruit];
            }
        }

        public int Get(string fruit)
        {
            if (fridge.ContainsKey(fruit))
                return fridge[fruit];
            return 0;
        }
    }
}