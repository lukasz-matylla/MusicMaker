namespace MusicCore
{
    public class Chord : IEquatable<Chord>
    {
        public IReadOnlyList<ScaleStep> Notes { get; }

        public Chord(params ScaleStep[] notes)
        {
            Notes = notes;
        }

        public Chord(params int[] notes)
        {
            Notes = notes.Select(n => new ScaleStep(n)).ToArray();
        }

        public Chord Inversion(int n)
        {
            n %= Notes.Count;

            return new Chord(Notes.Skip(n).Concat(Notes.Take(n)).ToArray());
        }

        public Chord WithChangedNote(int n, ScaleStep newValue)
        {
            var newNotes = Notes.ToArray();
            newNotes[n] = newValue;
            return new Chord(newNotes);
        }

        public override string ToString()
        {
            return $"({string.Join(", ", Notes)})";
        }

        public bool Equals(Chord? other)
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
    }
}
