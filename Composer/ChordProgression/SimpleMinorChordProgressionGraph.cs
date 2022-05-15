using MusicCore;

namespace Composer
{
    public class SimpleMinorChordProgressionGraph : ChordTransitionGraph
    {
        public override MusicalScale Scale => MusicalScale.Minor;

        protected readonly Chord i = new Chord(0, 2, 4);

        protected readonly Chord ii0 = new Chord(1, 3, 5);

        protected readonly Chord III = new Chord(2, 4, 6);

        protected readonly Chord iv = new Chord(3, 5, 0);

        protected readonly Chord v = new Chord(4, 6, 1);
        protected readonly Chord V = new Chord(4, new ScaleStep(6, Accidental.Sharp), 1);
        protected readonly Chord V7 = new Chord(4, new ScaleStep(6, Accidental.Sharp), 1, 3);

        protected readonly Chord VI = new Chord(5, 0, 2);

        protected readonly Chord VII = new Chord(6, 1, 3);

        public SimpleMinorChordProgressionGraph()
            : base()
        {
            AddTransition(i, i.Inversion(2));
            AddTransition(i, ii0);
            AddTransition(i, III);
            AddTransition(i, iv);
            AddTransition(i, v);
            AddTransition(i, V);
            AddTransition(i, V7);
            AddTransition(i, VI);
            AddTransition(i, VII);

            AddTransition(i.Inversion(2), V);
            AddTransition(i.Inversion(2), V7);

            AddTransition(ii0, i.Inversion(2));
            AddTransition(ii0, v);
            AddTransition(ii0, V);
            AddTransition(ii0, VI);

            AddTransition(III, v);
            AddTransition(III, V);
            AddTransition(III, V7);
            AddTransition(III, VI);

            AddTransition(iv, i, 3);
            AddTransition(iv, i.Inversion(2));
            AddTransition(iv, v);
            AddTransition(iv, V);
            AddTransition(iv, VI);
            AddTransition(iv, VII);

            AddTransition(v, i);
            AddTransition(v, V);
            AddTransition(v, VI);

            AddTransition(V, i, 3);
            AddTransition(V, VI);
            AddTransition(V, V7);

            AddTransition(V7, i, 3);
            AddTransition(V7, VI);

            AddTransition(VI, iv);
            AddTransition(VI, ii0);
            AddTransition(VI, v);
            AddTransition(VI, V);
            AddTransition(VI, V7);

            AddTransition(VII, i);
            AddTransition(VII, VI);
        }
    }
}
