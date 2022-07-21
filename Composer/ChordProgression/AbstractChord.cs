using MusicCore;

namespace Composer.ChordProgression
{
    internal class AbstractChord
    {
        private static IReadOnlyDictionary<ChordType, int[]> ChordIntervals = new Dictionary<ChordType, int[]>()
        {
            {ChordType.Major, new[] { 4, 3 }},
            {ChordType.Minor, new[] { 3, 4 }},
            {ChordType.Diminished, new[] { 3, 3 }},
            {ChordType.Augmented, new[] { 4, 4 }},
            {ChordType.Sus4, new[] { 5, 2 }},
            {ChordType.Sus2, new[] { 2, 5 }},
            {ChordType.Major7, new[] { 4, 3, 4 }},
            {ChordType.Minor7, new[] { 3, 4, 3 }},
            {ChordType.Dominant7, new[] { 4, 3, 3 }},
            {ChordType.HalfDiminished, new[] { 3, 3, 4 }},
            {ChordType.FullyDiminished, new[] { 3, 3, 3 }},
            {ChordType.MinMaj7, new[] { 3, 4, 4 }},
            {ChordType.DominantB5, new[] { 4, 2, 4 }},
        };

        public ChordType Type { get; }
        public ScaleStep Root { get; }
        public int? Inversion { get; }

        public AbstractChord(ScaleStep root, ChordType type = ChordType.Major, int? inversion = null)
        {
            Root = root;
            Type = type;
            Inversion = inversion;
        }

        public AbstractChord WithInversion(int inversion)
        {
            return new AbstractChord(Root, Type, inversion);
        }

        public AbstractChord WithRoot(ScaleStep root)
        {
            return new AbstractChord(root, Type, Inversion);
        }

        public AbstractChord WithType(ChordType type)
        {
            return new AbstractChord(Root, type, Inversion);
        }

        public static AbstractChord? Diatonic(MusicalScale scale, int root, bool is7th = false)
        {
            var numNotes = is7th ? 4 : 3;
            var pitches = Enumerable.Range(0, numNotes)
                .Select(i => scale.Steps[root + 2 * i])
                .Select(p => p < scale[root] ? p + MusicalScale.HalftonesInOctave : p)
                .ToArray();
            var intervals = Enumerable.Range(0, numNotes - 1)
                .Select(i => pitches[i+1] - pitches[i])
                .ToArray();

            var matching = ChordIntervals.Where(c => c.Value.SequenceEqual(intervals));
            if (matching.Any())
            {
                return new AbstractChord(root, matching.First().Key);
            }
            return null;
        }

        public bool IsTriad()
        {
            return Type == ChordType.Major ||
                Type == ChordType.Minor ||
                Type == ChordType.Diminished ||
                Type == ChordType.Augmented;
        }

        public bool Is7th()
        {
            return !IsTriad();
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() ^
                Root.GetHashCode() ^
                (Inversion?.GetHashCode() ?? 111);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is AbstractChord chord)
            {
                return Equals(chord);
            }

            return false;
        }

        public bool Equals(AbstractChord chord)
        {
            return Type == chord.Type
                && Root == chord.Root
                && Inversion == chord.Inversion;
        }
    }
}
