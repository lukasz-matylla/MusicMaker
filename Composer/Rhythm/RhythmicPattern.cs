namespace Composer
{
    public class RhythmicPattern : IEquatable<RhythmicPattern>
    {
        public int[] Notes { get; }
        public double Energy { get; }
        public int Length => Notes.Sum();

        public RhythmicPattern(double energy, IEnumerable<int> notes)
        {
            Energy = energy;
            Notes = notes.ToArray();
        }

        public RhythmicPattern(double energy, params int[] notes)
        {
            Energy = energy;
            Notes = notes;
        }

        public bool Equals(RhythmicPattern? other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Notes.SequenceEqual(other.Notes);
        }

        public override string ToString()
        {
            return $"[{string.Join(", ", Notes)}]: {Energy}";
        }
    }
}
