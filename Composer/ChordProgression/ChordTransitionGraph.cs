using MusicCore;
using Tools;

namespace Composer.ChordProgression
{
    public abstract class ChordTransitionGraph : TransitionGraphBase<Chord>, IChordTransitionGraph
    {
        public abstract MusicalScale Scale { get; }

        protected ChordTransitionGraph() :
            base()
        { }

        protected void AddTransitionAllInversions(Chord? from, Chord? to, double weight = 1)
        {
            if (from == null || to == null)
            {
                return;
            }

            for (var i = 0; i < from.Notes.Count; i++)
            {
                for (var j = 0; j < to.Notes.Count; j++)
                {
                    AddTransition(from.Inversion(i), to.Inversion(j), weight);
                }
            }
        }

        protected int AddOrUpdateChord(Chord chord)
        {
            var index = FindItemIndex(chord);

            if (index == -1)
            {
                index = items.Count;
                items.Add(chord);
            }
            else if (ChordOperations.HasLessAccidentals(chord, Items[index]))
            {
                items[index] = chord;
            }

            return index;
        }

        protected void AddOrUpdateChordAllInversions(Chord chord)
        {
            for (var i = 0; i <= chord.Notes.Count; i++)
            {
                AddOrUpdateChord(chord.Inversion(i));
            }
        }

        protected override bool IsEquivalent(Chord x, Chord y)
        {
            return ChordOperations.IsChordEquivalent(x, y, Scale);
        }
    }
}
