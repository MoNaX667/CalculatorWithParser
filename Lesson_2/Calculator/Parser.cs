using System;
using System.Collections.Generic;

namespace Calculator
{
    static class Parser
    {
        /// <summary>
        /// Start method
        /// </summary>
        /// <param name="statemant">string format math statement</param>
        /// <returns></returns>
        public static double PerformStatement(string statemant)
        {
            return DoSecondPriority(statemant, 0);
        }

        /// <summary>
        /// Second priority operation method
        /// </summary>
        /// <param name="statemant">string format math statement</param>
        /// <param name="index">index of current position in statement</param>
        /// <returns></returns>
        private static double DoSecondPriority(string statement, int index)
        {
            // Find first priority operation
            double x = DoFirstPriority(statement, ref index);

            // Look at current symbols
            while (true){
                char operation = statement[index];

                // if its operation !+ or !- return finded operand
                if (operation != '+' && operation != '-')
                    return x;
               
                // ... else increment index
                index++;

                // check first priority and check second operand
                double y = DoFirstPriority(statement, ref index);

                // Do operation
                if (operation == '+')
                    x += y;
                else
                    x -= y;
            }
        }

        /// <summary>
        /// Perform first turn operation
        /// </summary>
        /// <param name="statemnt">statement(char array form)</param>
        /// <param name="index">current statement position</param>
        /// <returns></returns>
        private static double DoFirstPriority(string statement, ref int index)
        {
            // ... take operand
            double x = GetDouble(statement, ref index);

            // ...find operation symbol
            while (true){
                char operation = statement[index];

                // if current char is operation symbol
                if (operation != '/' && operation != '*' && operation != '^')
                    return x;
                
                // ...than increment index and go next operator....
                index++;
                
                // ...Find next operand
                double y = GetDouble(statement, ref index);

                // Operation block
                if (operation == '/') {
                    try {
                        if (y == 0)
                            throw new DivideByZeroException();
                        else
                            x /= y;
                    }
                    catch(DivideByZeroException excep) {
                        System.Windows.Forms.MessageBox.Show(
                            "Попытка деления на ноль","Error");
                        return 0;
                    }
                }
                else if (operation == '*')
                    x *= y;
                else if (operation == '^')
                    x = Math.Pow(x, y);
            }

        }

        /// <summary>
        /// Method choosen number from statement(char array)
        /// </summary>
        /// <param name="statement">statement(char array form)</param>
        /// <param name="index">current statement position</param>
        /// <returns></returns>
        private static double GetDouble(string statement, ref int index)
        {
            string operand = "";

            // While symbol is the number members all is good alse return num
            while (char.IsNumber(statement[index]) ||
                char.IsSeparator(statement[index]))
            {
                // Add symbol to list for parsing
                operand+=statement[index];
                index++;

                // if index so out from range break and return for normal index
                if (index == statement.Length)
                {
                    index--;
                    break;
                }
            }

            // Parsing with culture parametr
            try
            {
                return double.Parse(operand, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (System.FormatException excep) {
                System.Windows.Forms.MessageBox.Show("Выражение не может быть приобразованно",
                    "Error");
                return 0;
            }
        }
    }
}
