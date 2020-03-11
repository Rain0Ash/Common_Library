// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Drawing;

namespace Common_Library.Colorful
{
    // NOTE: The StyledString class is meant to replace a number of instances of System.String in this project.  
    //       However, it's currently only used in the WriteLineStyled method, becuase I'd like to have better
    //       test coverage before using it in the rest of the project.

    /// <summary>
    /// A string encoded with style!
    /// </summary>
    public sealed class StyledString : IEquatable<StyledString>
    {
        /// <summary>
        /// The one-dimensional representation of the StyledString.  Maps 1:1 with System.String.
        /// </summary>
        public String AbstractValue { get; }

        /// <summary>
        /// The n-dimensional (n; 2, in this case) representation of the StyledString.  
        /// In the case of FIGlet fonts, for example, this would be the string's two-dimensional FIGlet representation.
        /// </summary>
        public String ConcreteValue { get; }

        //
        // The three geometry members, below, should be encapsulated into a single data structure.
        //

        /// <summary>
        /// A matrix of colors that corresponds to each concrete character in the StyledString.
        /// Dimensions are [row, column].
        /// </summary>
        public Color[,] ColorGeometry { get; set; }

        /// <summary>
        /// A matrix of concrete characters that corresponds to each concrete character in the StyledString.
        /// In other words, this is a two-dimensional representation of the StyledString.ConcreteValue property.
        /// </summary>
        public Char[,] CharacterGeometry { get; set; }

        /// <summary>
        /// A matrix of abstract character indices that corresponds to each concrete character in the StyledString.
        /// Dimensions are [row, column].
        /// </summary>
        public Int32[,] CharacterIndexGeometry { get; set; }

        public StyledString(String oneDimensionalRepresentation)
        {
            AbstractValue = oneDimensionalRepresentation;
        }

        public StyledString(String oneDimensionalRepresentation, String twoDimensionalRepresentation)
        {
            AbstractValue = oneDimensionalRepresentation;
            ConcreteValue = twoDimensionalRepresentation;
        }

        // Does not take styling information into account...and it needs to be taken into account.
        public Boolean Equals(StyledString other)
        {
            if (other == null)
            {
                return false;
            }

            return AbstractValue == other.AbstractValue
                   && ConcreteValue == other.ConcreteValue;
        }

        public override Boolean Equals(Object obj)
        {
            return Equals(obj as StyledString);
        }

        // Does not take styling information into account...and it needs to be taken into account.
        public override Int32 GetHashCode()
        {
            Int32 hash = 163;

            hash *= 79 + AbstractValue.GetHashCode();
            hash *= 79 + ConcreteValue.GetHashCode();

            return hash;
        }

        /// <summary>
        /// Returns the StyledString's concrete representation.
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return ConcreteValue;
        }
    }
}