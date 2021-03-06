// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Collections.Generic;

namespace Common_Library.Combinatorics
{
    /// <summary>
    /// Interface for Permutations, Combinations and any other classes that present
    /// a collection of collections based on an input collection.  The enumerators that 
    /// this class inherits defines the mechanism for enumerating through the collections.  
    /// </summary>
    /// <typeparam name="T">The of the elements in the collection, not the type of the collection.</typeparam>
    public interface IMetaCollection<T> : IEnumerable<IList<T>>
    {
        /// <summary>
        /// The count of items in the collection.  This is not inherited from
        /// ICollection since this meta-collection cannot be extended by users.
        /// </summary>
        Int64 Count { get; }

        /// <summary>
        /// The type of the meta-collection, determining how the collections are 
        /// determined from the inputs.
        /// </summary>
        GenerateOption Type { get; }

        /// <summary>
        /// The upper index of the meta-collection, which is the size of the input collection.
        /// </summary>
        Int32 UpperIndex { get; }

        /// <summary>
        /// The lower index of the meta-collection, which is the size of each output collection.
        /// </summary>
        Int32 LowerIndex { get; }
    }
}