// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Colorful
{
    /// <summary>
    /// Exposes properties describing the indices of the beginning and end of a pattern match.
    /// </summary>
    public class MatchLocation : IEquatable<MatchLocation>, IComparable<MatchLocation>, IPrototypable<MatchLocation>
    {
        /// <summary>
        /// The index of the beginning of the pattern match.
        /// </summary>
        public Int32 Beginning { get; }
        /// <summary>
        /// The index of the end of the pattern match.
        /// </summary>
        public Int32 End { get; }

        /// <summary>
        /// Exposes properties describing the indices of the beginning and end of a pattern match.
        /// </summary>
        /// <param name="beginning">The index of the beginning of the pattern match.</param>
        /// <param name="end">The index of the end of the pattern match.</param>
        public MatchLocation(Int32 beginning, Int32 end)
        {
            Beginning = beginning;
            End = end;
        }

        public MatchLocation Prototype()
        {
            return new MatchLocation(Beginning, End);
        }

        public Boolean Equals(MatchLocation other)
        {
            if (other == null)
            {
                return false;
            }

            return Beginning == other.Beginning
                && End == other.End;
        }

        public override Boolean Equals(Object obj) => Equals(obj as MatchLocation);
       
        public override Int32 GetHashCode()
        {
            Int32 hash = 163;

            hash *= 79 + Beginning.GetHashCode();
            hash *= 79 + End.GetHashCode();

            return hash;
        }

        public Int32 CompareTo(MatchLocation other)
        {
            return Beginning.CompareTo(other.Beginning);
        }

        public override String ToString() => Beginning + ", " + End;
    }
}
