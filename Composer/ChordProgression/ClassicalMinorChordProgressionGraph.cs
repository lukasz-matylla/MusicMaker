using MusicCore;

namespace Composer
{
    public class ClassicalMinorChordProgressionGraph : SimpleMinorChordProgressionGraph
    {
        public override MusicalScale Scale => MusicalScale.Minor;

        protected readonly Chord ii = new Chord(1, 3, new ScaleStep(5, Accidental.Sharp));
        protected readonly Chord N = new Chord(3, 5, new ScaleStep(1, Accidental.Flat));
        protected readonly Chord Fr = new Chord(5, 0, 1, new ScaleStep(3, Accidental.Sharp));
        protected readonly Chord Ger = new Chord(5, 0, 2, new ScaleStep(3, Accidental.Sharp));
        protected readonly Chord vii00 = new Chord(6, new ScaleStep(1, Accidental.Flat), new ScaleStep(3, Accidental.Flat), new ScaleStep(5, Accidental.Flat));

        public ClassicalMinorChordProgressionGraph() 
            : base()
        {
            AddTransition(i, ii);
            AddTransition(i, vii00);

            AddTransition(ii, i.Inversion(2));
            AddTransition(ii, v);
            AddTransition(ii, V);
            AddTransition(ii, vii00);
            AddTransition(ii, VI);

            AddTransition(ii0, vii00);

            AddTransition(iv, vii00);
 
            AddTransition(N, V);
            AddTransition(N, V7);

            AddTransition(v, N);
            AddTransition(v, Fr);
            AddTransition(v, Ger);

            AddTransition(VI, ii);
            AddTransition(VI, N);
            AddTransition(VI, Fr);
            AddTransition(VI, Ger);

            AddTransition(Fr, V);
            AddTransition(Fr, V7);
            AddTransition(Ger, V);
            AddTransition(Ger, V7);

            AddTransition(vii00, i);
            AddTransition(vii00, V);
        }
    }
}
