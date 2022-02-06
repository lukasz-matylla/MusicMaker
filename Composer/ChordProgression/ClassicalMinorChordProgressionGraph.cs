using MusicCore;

namespace Composer
{
    public class ClassicalMinorChordProgressionGraph : ChordTransitionGraph
    {
        public override MusicalScale Scale => MusicalScale.Minor;

        protected readonly Chord i = new Chord(0, 2, 4);

        protected readonly Chord ii0 = new Chord(1, 3, 5);
        protected readonly Chord ii = new Chord(1, 3, new ScaleStep(5, Accidental.Sharp));

        protected readonly Chord III = new Chord(2, 4, 6);

        protected readonly Chord iv = new Chord(3, 5, 0);
        protected readonly Chord N = new Chord(3, 5, new ScaleStep(1, Accidental.Flat));

        protected readonly Chord v = new Chord(4, 6, 1);
        protected readonly Chord V = new Chord(4, new ScaleStep(6, Accidental.Sharp), 1);
        protected readonly Chord V7 = new Chord(4, new ScaleStep(6, Accidental.Sharp), 1, 3);

        protected readonly Chord VI = new Chord(5, 0, 2);
        protected readonly Chord Fr = new Chord(5, 0, 1, new ScaleStep(3, Accidental.Sharp));
        protected readonly Chord Ger = new Chord(5, 0, 2, new ScaleStep(3, Accidental.Sharp));

        protected readonly Chord VII = new Chord(6, 1, 3);
        protected readonly Chord vii00 = new Chord(6, new ScaleStep(1, Accidental.Flat), new ScaleStep(3, Accidental.Flat), new ScaleStep(5, Accidental.Flat));

        public ClassicalMinorChordProgressionGraph() 
            :base()
        {
            AddTransition(i, i.Inversion(2));
            AddTransition(i, ii);
            AddTransition(i, ii0);
            AddTransition(i, III);
            AddTransition(i, iv);
            AddTransition(i, v);
            AddTransition(i, V);
            AddTransition(i, V7);
            AddTransition(i, VI);
            AddTransition(i, VII);
            AddTransition(i, vii00);

            AddTransition(i.Inversion(2), V);
            AddTransition(i.Inversion(2), V7);

            AddTransition(ii, i.Inversion(2));
            AddTransition(ii, v);
            AddTransition(ii, V);
            AddTransition(ii, vii00);
            AddTransition(ii, VI);

            AddTransition(ii0, i.Inversion(2));
            AddTransition(ii0, v);
            AddTransition(ii0, V);
            AddTransition(ii0, vii00);
            AddTransition(ii0, VI);

            AddTransition(III, VI);

            AddTransition(iv, i.Inversion(2));
            AddTransition(iv, v);
            AddTransition(iv, V);
            AddTransition(iv, vii00);
            AddTransition(iv, VI);
            AddTransition(iv, VII);

            AddTransition(N, V);
            AddTransition(N, V7);

            AddTransition(v, i);
            AddTransition(v, N);
            AddTransition(v, Fr);
            AddTransition(v, Ger);
            AddTransition(v, VI);

            AddTransition(V, i);
            AddTransition(V, N);
            AddTransition(V, Fr);
            AddTransition(V, Ger);
            AddTransition(V, VI);
            AddTransition(V, V7);

            AddTransition(V7, i);
            AddTransition(V7, VI);

            AddTransition(VI, iv);
            AddTransition(VI, ii);
            AddTransition(VI, ii0);
            AddTransition(VI, N);
            AddTransition(VI, Fr);
            AddTransition(VI, Ger);
            AddTransition(VI, v);
            AddTransition(VI, V);
            AddTransition(VI, V7);

            AddTransition(Fr, V);
            AddTransition(Fr, V7);
            AddTransition(Ger, V);
            AddTransition(Ger, V7);

            AddTransition(VII, i);
            AddTransition(VII, VI);

            AddTransition(vii00, i);
            AddTransition(vii00, V);
        }
    }
}
