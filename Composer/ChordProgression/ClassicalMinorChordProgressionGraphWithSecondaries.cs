using MusicCore;

namespace Composer
{
    public class ClassicalMinorChordProgressionGraphWithSecondaries : ClassicalMinorChordProgressionGraph
    {
        protected readonly Chord V7ofIII = new Chord(6, 1, 3, 5);
        protected readonly Chord V7ofIV = new Chord(0, new ScaleStep(2, Accidental.Sharp), 4, 6);
        protected readonly Chord V7ofV = new Chord(1, new ScaleStep(3, Accidental.Sharp), new ScaleStep(5, Accidental.Sharp), 0);
        protected readonly Chord V7ofVI = new Chord(2, 4, 6, new ScaleStep(1, Accidental.Flat));
        protected readonly Chord V7ofVII = new Chord(3, new ScaleStep(5, Accidental.Sharp), 0, 2);

        public ClassicalMinorChordProgressionGraphWithSecondaries()
            : base()
        {
            AddTransition(i, V7ofIV);

            AddTransition(ii0, V7ofV);
            AddTransition(ii, V7ofV);

            AddTransition(III, V7ofIII);
            AddTransition(III, V7ofVI);

            AddTransition(iv, V7ofIV);
            AddTransition(iv, V7ofVII);

            AddTransition(v, V7ofV);
            AddTransition(V, V7ofV);

            AddTransition(VI, V7ofVI);

            AddTransition(VII, V7ofIII);
            AddTransition(VII, V7ofVII);


            AddTransition(V7ofIII, III);

            AddTransition(V7ofIV, iv);

            AddTransition(V7ofV, v);
            AddTransition(V7ofV, V);
            AddTransition(V7ofV, V7);
            AddTransition(V7ofV, i.Inversion(2));

            AddTransition(V7ofVI, VI);

            AddTransition(V7ofVII, VII);
        }
    }
}
