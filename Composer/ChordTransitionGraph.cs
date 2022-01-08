using MusicCore;

namespace Composer
{
    public abstract class ChordTransitionGraph : IChordTransitionGraph
    {
        public IReadOnlyList<Chord> Chords => chords;
        public abstract MusicalScale Scale { get; }

        private readonly List<Chord> chords;
        private readonly List<GraphTransition> transitions;

        protected ChordTransitionGraph()
        {
            chords = new List<Chord>();
            transitions = new List<GraphTransition>();
        }

        protected void AddTransition(Chord from, Chord to, double weight = 1)
        {
            var fromIndex = EnsureChord(from);
            var toIndex = EnsureChord(to);

            var transitionIndex = transitions.FindIndex(t => t.From == fromIndex && t.To == toIndex);

            if (transitionIndex >= 0)
            {
                transitions[transitionIndex].Weight = weight;
            }
            else
            {
                transitions.Add(new GraphTransition(fromIndex, toIndex, weight));
            }
        }

        protected int FindChordIndex(Chord chord)
        {
            return chords.FindIndex(c => ChordOperations.IsChordEquivalent(c, chord, Scale));
        }

        protected int AddOrUpdateChord(Chord chord)
        {
            var index = FindChordIndex(chord);

            if (index == -1)
            {
                index = chords.Count;
                chords.Add(chord);
            }
            else if (ChordOperations.HasLessAccidentals(chord, Chords[index]))
            {
                chords[index] = chord;
            }

            return index;
        }

        protected int EnsureChord(Chord chord)
        {
            var index = FindChordIndex(chord);

            if (index == -1)
            {
                index = chords.Count;
                chords.Add(chord);
            }

            return index;
        }

        private double GetWeight(int from, int to)
        {
            var transition = transitions.FirstOrDefault(t => t.From == from && t.To == to);
            return transition?.Weight ?? 0;
        }

        #region Interface implementation

        public double[] WeightsFrom(int chord)
        {
            return Enumerable.Range(0, chords.Count)
                .Select(i => GetWeight(chord, i))
                .ToArray();
        }

        public double[] WeightsFrom(Chord chord)
        {
            return WeightsFrom(FindChordIndex(chord));
        }

        public double[] WeightsTo(int chord)
        {
            return Enumerable.Range(0, chords.Count)
                .Select(i => GetWeight(i, chord))
                .ToArray();
        }

        public double[] WeightsTo(Chord chord)
        {
            return WeightsTo(FindChordIndex(chord));
        }

        #endregion

        protected class GraphTransition
        {
            public readonly int From;
            public readonly int To;
            public double Weight;

            public GraphTransition(int from, int to, double weight = 1)
            {
                From = from;
                To = to;
                Weight = weight;
            }
        }
    }
}
