using MusicCore;

namespace Composer
{
    public class ClassicalMajorChordProgressionGraphWithSecondaries : ClassicalMajorChordProgressionGraph
    {
        protected readonly Chord VofIII = new Chord(6, new ScaleStep(1, Accidental.Sharp), new ScaleStep(3, Accidental.Sharp));
        protected readonly Chord VofVI = new Chord(2, new ScaleStep(4, Accidental.Sharp), 6);
        protected readonly Chord VofII = new Chord(5, new ScaleStep(0, Accidental.Sharp), 2);
        protected readonly Chord VofV = new Chord(1, new ScaleStep(3, Accidental.Sharp), 5);
        protected readonly Chord V7ofIV = new Chord(0, 2, 4, new ScaleStep(6, Accidental.Flat));

        protected readonly Chord Iaug = new Chord(0, 2, new ScaleStep(4, Accidental.Sharp));

        public ClassicalMajorChordProgressionGraphWithSecondaries()
            : base()
        {
            AddTransition(I, VofIII);
            AddTransition(I, VofVI);
            AddTransition(I, VofII);
            AddTransition(I, VofV);
            AddTransition(I, V7ofIV);

            AddTransition(iii, VofVI);
            AddTransition(vi, VofII);
            AddTransition(IV, VofV);
            AddTransition(ii, VofV);

            AddTransition(VofIII, iii);
            AddTransition(VofIII, VofVI);

            AddTransition(VofVI, vi);
            AddTransition(VofVI, VofII);

            AddTransition(VofII, ii);
            AddTransition(VofII, VofV);

            AddTransition(VofV, V);
            AddTransition(VofV, V7);
            AddTransition(VofV, vii0);
            AddTransition(VofV, I.Inversion(2));
            AddTransition(VofV, V7ofIV);

            AddTransition(V7ofIV, IV);
            AddTransition(V7ofIV, ii);

            AddTransition(vii0, VofVI);
            AddTransition(vii00, VofVI);
        }
    }
}
