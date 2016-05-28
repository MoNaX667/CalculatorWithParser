namespace Calculator
{
    using System;

    internal static class Parser
    {
        private static ParserErrors operationError;

        /// <summary>
        /// Gets status of last operation
        /// </summary>
        public static ParserErrors OperationError
        {
            get
            {
                return operationError;
            }
        }

        /// <summary>Run calculate</summary>
        /// <param name="statement">statement for calculating</param>
        /// <returns>answer</returns>
        public static double Run(string statement)
        {
            string expression = statement;

            // Check for brackets if true than perform bracket's statement and replece 
            // brackets statement from general statement and pass for general performing
            if (statement.Contains("(") && statement.Contains(")"))
            {
                expression = Parser.OpenBrackets(expression);
            }

            return PerformStatement(expression);
        }

        /// <summary>
        /// Start method
        /// </summary>
        /// <param name="statemant">string format math statement</param>
        /// <returns>answer</returns>
        public static double PerformStatement(string statement)
        {             
            return DoSecondPriority(statement, 0);
        }

        /// <summary>
        /// Second priority operation method
        /// </summary>
        /// <param name="statemant">string format math statement</param>
        /// <param name="index">index of current position in statement</param>
        /// <returns>second priority answer</returns>
        private static double DoSecondPriority(string statement, int index)
        {
            // Find first priority operations
            double x = DoFirstPriority(statement, ref index);

            // Look at current symbols
            while (true)
            {
                char operation = statement[index];

                // if its operation !+ or !- return finded operand
                if (operation != '+' && operation != '-')
                {
                    return x;
                }

                // ... else increment index
                index++;

                // check first priority and check second operand
                double y = DoFirstPriority(statement, ref index);

                // Do operation
                if (operation == '+')
                {
                    x += y;
                }
                else
                {
                    x -= y;
                }
            }
        }

        /// <summary>
        /// Perform first turn operation
        /// </summary>
        /// <param name="statemnt">statement(char array form)</param>
        /// <param name="index">current statement position</param>
        /// <returns>first level answer</returns>
        private static double DoFirstPriority(string statement, ref int index)
        {
            // ... take operand
            double x = GetDouble(statement, ref index);

            // ...find operation symbol
            while (true)
            {
                char operation = statement[index];

                // if current char is operation symbol
                if (operation != '/' && operation != '*' && operation != '^')
                {
                    return x;
                }

                // ...than increment index and go next operator....
                index++;
                
                // ...Find next operand
                double y = GetDouble(statement, ref index);

                // Operation block
                if (operation == '/')
                {
                    try
                    {
                        if (y == 0)
                        {
                            throw new DivideByZeroException();
                        }
                        else
                        {
                            x /= y;
                        }

                        operationError = ParserErrors.None;
                    }
                    catch (DivideByZeroException)
                    {
                        operationError = ParserErrors.DivideByZero;
                        return 0;
                    }
                }
                else if (operation == '*')
                {
                    x *= y;
                }
                else if (operation == '^')
                {
                    x = Math.Pow(x, y);
                }
            }
        }

        /// <summary>
        /// Method take number from statement(char array)
        /// </summary>
        /// <param name="statement">statement(char array form)</param>
        /// <param name="index">current statement position</param>
        /// <returns>some number</returns>
        private static double GetDouble(string statement, ref int index)
        {
            string operand = string.Empty;

            // While symbol is the number members all is good alse return num
            while (char.IsNumber(statement[index]) ||
                char.IsSeparator(statement[index]))
            {
                // Add symbol to list for parsing
                operand += statement[index];
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
            catch (System.FormatException)
            {
                operationError = ParserErrors.StatemantCantBePerformed;
                return 0;
            }
        }

        private static string OpenBrackets(string statement)
        {
            int openBracket = 0, closeBracket = 0;
            string result = statement;

            for (int index = 0; index < result.Length; index++)
            {
                // Check for open bracket
                if (result[index] == '(')
                {
                    openBracket = index;
                }
                
                // Check for close bracket
                if (result[index] == ')')
                {
                    closeBracket = index;

                    // Replece performed braket block's statement in result statement
                    result = result.Replace(
                        result.Substring(openBracket, closeBracket - openBracket),
                        PerformStatement(result.Substring(openBracket + 1, closeBracket - openBracket)).ToString());
                }
            }

            return result;
        }
    }
}
