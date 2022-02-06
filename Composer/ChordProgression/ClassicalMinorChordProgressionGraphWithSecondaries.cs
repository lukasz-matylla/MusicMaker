using MusicCore;

namespace Composer
{
    public class ClassicalMinorChordProgressionGraphWithSecondaries : ClassicalMinorChordProgressionGraph
    {
        protected readonly Chord V7ofIV = new Chord(0, new ScaleStep(2, Accidental.Sharp), 4, 6);
        protected readonly Chord V7ofVII = new Chord(3, new ScaleStep(5, Accidental.Sharp), 0, 2);
        protected readonly Chord V7ofIII = new Chord(6, 1, 3, 5);
        protected readonly Chord V7ofVI = new Chord(2, 4, 6, new ScaleStep(1, Accidental.Flat));
        protected readonly Chord VofV = new Chord(1, new ScaleStep(3, Accidental.Sharp), new ScaleStep(5, Accidental.Sharp));

        public ClassicalMinorChordProgressionGraphWithSecondaries()
            : base()
        {
            AddTransition(i, V7ofIV);

            AddTransition(III, V7ofVI);

            AddTransition(V7ofIV, iv);
            AddTransition(V7ofIV, V7ofVII);

            AddTransition(V7ofVII, VII);
            AddTransition(V7ofVII, V7ofIII);

            AddTransition(V7ofIII, III);
            AddTransition(V7ofIII, V7ofVI);

            AddTransition(V7ofVI, VI);
            AddTransition(V7ofVI, iv);
            AddTransition(V7ofVI, ii);
            AddTransition(V7ofVI, ii0);

            AddTransition(iv, VofV);
            AddTransition(ii0, VofV);
            AddTransition(ii, VofV);

            AddTransition(VofV, i.Inversion(2));
            AddTransition(VofV, V);
            AddTransition(VofV, vii00);

            AddTransition(vii00, V7ofVI);
        }
    }
}
