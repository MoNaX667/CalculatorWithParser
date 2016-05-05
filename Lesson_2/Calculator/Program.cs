using System;
using System.Text.RegularExpressions;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console setting and start message
            Console.Title = ("Calculator");
            Console.WriteLine("Welcome to calculator");
            Console.WriteLine(new string('_',60)+Environment.NewLine);

            // Work application loop
            StartWorkLoop();
        }

        /// <summary>
        /// Work application loop if user iin the end of iteration press escape 
        /// loop will be broken
        /// </summary>
        static void StartWorkLoop() {
            string statement = "";
            double result = 0;

            while (true) {
                // Session info block
                statement = ShowDialog();

                // Perform statement
                result = PerformStatement(statement);

                // Output result
                Console.WriteLine("Answer --->>> {0}", result);

                // User next action block
                Console.WriteLine(new string('_', 60) + Environment.NewLine);
                Console.WriteLine("Press any key for new operation or for \n"+
                "exit from application press \"Escape\"");

                if (Console.ReadKey().Key == ConsoleKey.Escape)
                    break;
                else
                    Console.Clear();
            }
        }

        /// <summary>
        /// Show session information block
        /// </summary>
        /// <returns></returns>
        static string ShowDialog() {
            string statement = "";

            // Input dialog
            Console.WriteLine(@"For inputing you can you next symbols:
        0-9 +  -  *  /");
            Console.WriteLine();
            Console.WriteLine(new string('_', 60) + Environment.NewLine);
            Console.Write("Input here ---->>> ");

            // Check statement block
            while (true) {
                if (!string.IsNullOrEmpty(statement = Console.ReadLine()))
                    break;

                Console.WriteLine("Bad input, please input again ---> ");
            }
            
            return statement;
        }

        /// <summary>
        /// Calculate statemnt with Parse class methods
        /// </summary>
        /// <param name="statement">string statement</param>
        /// <returns>result of statement</returns>
        static double PerformStatement(string statement) {
            double result = Parser.PerformStatement(statement);

            return result;
        }

    }
}
