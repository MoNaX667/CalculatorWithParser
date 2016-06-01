// <copyright file="ParserErrors.cs" company="Some Company">
// Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>Vitalit Belyakov</author>

namespace Calculator
{
    /// <summary>
    /// Some parser calculator errors
    /// </summary>
    internal enum ParserErrors
    {
        /// <summary>
        /// None error ... all good
        /// </summary>
        None,

        /// <summary>
        /// Divide by zero exception error
        /// </summary>
        DivideByZero,

        /// <summary>
        /// statement perform error
        /// </summary>
        StatemantCantBePerformed,
    }
}
