using System;
using System.Collections.Generic;

namespace Common_Library.Combinatorics
{
    public static class Combinatorics
    {
        public enum CombinatoricsType
        {
            Permutations,
            PermutationsWithRepetition,

            Combinations,
            CombinationsWithRepetition,

            Variations,
            VariationsWithRepetition
        }

        public static IMetaCollection<T> Factory<T>(IList<T> values, CombinatoricsType type, Int32 take = 2)
        {
            return type switch
            {
                CombinatoricsType.Permutations => (IMetaCollection<T>) new Permutations<T>(values),
                CombinatoricsType.PermutationsWithRepetition => new Permutations<T>(values, GenerateOption.WithRepetition),
                CombinatoricsType.Combinations => new Combinations<T>(values, take),
                CombinatoricsType.CombinationsWithRepetition => new Combinations<T>(values, take, GenerateOption.WithRepetition),
                CombinatoricsType.Variations => new Variations<T>(values, take),
                CombinatoricsType.VariationsWithRepetition => new Variations<T>(values, take, GenerateOption.WithRepetition),
                _ => throw new NotImplementedException()
            };
        }
    }
}