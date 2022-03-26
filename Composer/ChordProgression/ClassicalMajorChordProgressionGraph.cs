using MusicCore;

namespace Composer
{
    public class ClassicalMajorChordProgressionGraph : SimpleMajorChordProgressionGraph
    {
        public override MusicalScale Scale => MusicalScale.Major;

        protected readonly Chord IVsus2 = new Chord(3, 4, 0);
        protected readonly Chord N = new Chord(3, new ScaleStep(5, Accidental.Flat), new ScaleStep(1, Accidental.Flat));
        protected readonly Chord Vsus4 = new Chord(4, 0, 1);
        protected readonly Chord Fr = new Chord(new ScaleStep(5, Accidental.Flat), 0, 1, new ScaleStep(3, Accidental.Sharp));
        protected readonly Chord Ger = new Chord(new ScaleStep(5, Accidental.Flat), 0, new ScaleStep(2, Accidental.Flat), new ScaleStep(3, Accidental.Sharp));
        protected readonly Chord vii00 = new Chord(6, 1, 3, new ScaleStep(5, Accidental.Flat));

        public ClassicalMajorChordProgressionGraph()
            : base()
        {
            AddTransition(I, V.Inversion(2));
            AddTransition(I, Vsus4);
            AddTransition(I, vi);

            AddTransition(I.Inversion(1), Vsus4);

            AddTransition(ii, N);
            AddTransition(ii, Fr);
            AddTransition(ii, Ger);
            AddTransition(ii, vii00.Inversion(1));

            AddTransition(V.Inversion(2), I.Inversion(1));
            AddTransition(V.Inversion(2), iii);

            AddTransition(IV, N);
            AddTransition(IV, Fr);
            AddTransition(IV, Ger);
            AddTransition(IV, V);
            AddTransition(IV, vii00.Inversion(1));

            AddTransition(IVsus2, IV);

            AddTransition(N, V);
            AddTransition(N, V7);

            AddTransition(V, I);
            AddTransition(V, V);
            AddTransition(V, V7);
            AddTransition(V, vi);

            AddTransition(Vsus4, V);
            AddTransition(Vsus4, V7);

            AddTransition(I.Inversion(2), Vsus4);

            AddTransition(Fr, V);
            AddTransition(Fr, V7);

            AddTransition(Ger, V);
            AddTransition(Ger, V7);

            AddTransition(vii00.Inversion(1), I);
            AddTransition(vii00.Inversion(1), I.Inversion(2));
        }
    }
}
