// <copyright file="Parser.cs" company="Some Company">
// Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>Vitalit Belyakov</author>

namespace Calculator
{
    using System;

    /// <summary>
    /// Parser of math
    /// </summary>
    internal static class Parser
    {
        /// <summary>
        /// operation error
        /// </summary>
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
        /// <param name="statement">someStatement for calculating</param>
        /// <returns>Double answer</returns>
        public static double Run(string statement)
        {
            string expression = statement;

            // Check for brackets if true than perform bracket's someStatement and replece 
            // brackets someStatement from general someStatement and pass for general performing
            if (statement.Contains("(") && statement.Contains(")"))
            {
                expression = Parser.OpenBrackets(expression);
            }

            expression = CalcAllFactorial(expression);

            double result = PerformStatement(expression);
            foreach (var value in result.ToString())
            {
                if (char.IsDigit(value))
                {
                    operationError = ParserErrors.None;
                }
                else
                {
                    operationError = ParserErrors.StatemantCantBePerformed;
                }
            }

            return result;
        }

        /// <summary>
        /// Start method
        /// </summary>
        /// <param name="statement">String format math someStatement</param>
        /// <returns>Double answer</returns>
        public static double PerformStatement(string statement)
        {
            return DoSecondPriority(statement, 0);
        }

        /// <summary>
        /// Second priority operation method
        /// </summary>
        /// <param name="statement">String format math someStatement</param>
        /// <param name="index">Index of current position in someStatement</param>
        /// <returns>Second priority answer</returns>
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
        /// <param name="someStatement">Statement(char array form)</param>
        /// <param name="index">Current someStatement position</param>
        /// <returns>First level answer</returns>
        private static double DoFirstPriority(string someStatement, ref int index)
        {
            string statement = someStatement;

            // ... take operand
            double x = GetDouble(statement, ref index);

            // ...find operation symbol
            while (true)
            {
                char operation = statement[index];

                operation = statement[index];

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
        /// Calculate all factorials
        /// </summary>
        /// <param name="statemant">Target statement</param>
        /// <returns>Statement without factorials</returns>
        private static string CalcAllFactorial(string statemant)
        {
            int temp = 0;
            int index = 0;
            double currentNumber = 0;
            string answer = statemant;

            while (answer.Contains("!"))
            {
                if (char.IsDigit(answer[index]))
                {
                    currentNumber = GetDouble(answer, ref index);
                }

                if (answer[index] == '!')
                {
                    answer = answer.Remove(index, 1);

                    answer = answer.Replace(
                        currentNumber.ToString(),
                        Facttorial(currentNumber, ref temp).ToString());
                    index--;
                }
                else
                {
                    index++;
                }
            }

            return answer;
        }

        /// <summary>
        /// Calculate factorial
        /// </summary>
        /// <param name="x">Last x</param>
        /// <param name="index">Current index</param>
        /// <returns>Calculated answer</returns>
        private static double Facttorial(double x, ref int index)
        {
            double result = 1;
            int i = 1;

            while (i <= x)
            {
                result *= i;
                i++;
            }

            return result;
        }

        /// <summary>
        /// Method take number from someStatement(char array)
        /// </summary>
        /// <param name="statement">someStatement(char array form)</param>
        /// <param name="index">current someStatement position</param>
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

        /// <summary>
        /// Open brackets
        /// </summary>
        /// <param name="statement">Current expression</param>
        /// <returns>New expression</returns>
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
                    int tempStart = openBracket + 1;
                    int tempEnd = closeBracket;

                    // Replece performed braket block's someStatement in result someStatement
                    string smallExpression = result.Substring(tempStart, tempEnd - tempStart);
                    string smallAnswer = PerformStatement(smallExpression).ToString();

                    // Replace answer with brackets and set index in zero for second validation
                    result = result.Replace(
                        result.Substring(openBracket, closeBracket - openBracket + 1),
                        smallAnswer);
                    index = 0;
                }
            }

            return result;
        }
    }
}
