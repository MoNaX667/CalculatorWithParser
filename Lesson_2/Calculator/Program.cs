using System;

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
            Console.WriteLine(" For inputing you can you next symbols:\n"+
                 "  0-9 +  -  *  /\n"+
               "    and you can use brackets (statement)");
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
            double result = Parser.Run(statement);

            // Perform any situation
            switch (Parser.OperationError) {
                // None error
                case ParserErrors.None:
                    return result;

                // Divide by zero excep
                case ParserErrors.DivideByZero:
                    Console.WriteLine(new string('-', 60) + Environment.NewLine);
                    Console.WriteLine("Divide by zero exception");
                    return 0;

                // Format excep
                case ParserErrors.StatemantCantBePerformed:
                    Console.WriteLine(new string('-',60)+Environment.NewLine);
                    Console.WriteLine("The statement cant be performed; Format Exception");
                    return 0;

                // Defoult
                default:
                    Console.WriteLine("Not initilizate situation");
                    return 0;
            }

            
        }

    }
}
