using MusicCore;

namespace Composer
{
    public class ClassicalMajorChordProgressionGraphWithSecondaries : ClassicalMajorChordProgressionGraph
    {
        protected readonly Chord VofII = new Chord(5, new ScaleStep(0, Accidental.Sharp), 2);
        protected readonly Chord VofV = new Chord(1, new ScaleStep(3, Accidental.Sharp), 5);
        protected readonly Chord VofVI = new Chord(2, new ScaleStep(4, Accidental.Sharp), 6);

        protected readonly Chord V7ofIV = new Chord(0, 2, 4, new ScaleStep(6, Accidental.Flat));
        protected readonly Chord V7ofV = new Chord(1, new ScaleStep(3, Accidental.Sharp), 5, 0);

        protected readonly Chord vii00ofV = new Chord(0, new ScaleStep(2, Accidental.Flat), new ScaleStep(4, Accidental.Flat), 5);
        protected readonly Chord vii00ofII = new Chord(2, 4, new ScaleStep(6, Accidental.Flat), new ScaleStep(1, Accidental.Flat));

        protected readonly Chord Iaug = new Chord(0, 2, new ScaleStep(4, Accidental.Sharp));
        protected readonly Chord IIaug = new Chord(1, new ScaleStep(3, Accidental.Sharp), new ScaleStep(5, Accidental.Sharp));
        protected readonly Chord IVaug = new Chord(3, 5, new ScaleStep(0, Accidental.Sharp));
        protected readonly Chord Vaug = new Chord(4, 6, new ScaleStep(1, Accidental.Sharp));

        public ClassicalMajorChordProgressionGraphWithSecondaries()
            : base()
        {
            AddTransition(I, VofII);
            AddTransition(I, V7ofIV);
            AddTransition(I, vii00ofV);
            AddTransition(I, Iaug);
            AddTransition(I, Vaug);

            AddTransition(ii, VofV);
            AddTransition(ii, V7ofV);
            AddTransition(ii, VofII);
            AddTransition(ii, IIaug);
            AddTransition(ii, IVaug);

            AddTransition(iii, VofVI);
            AddTransition(iii, V7ofIV);
            AddTransition(iii, vii00ofII);
            AddTransition(iii, Iaug);
            AddTransition(iii, Vaug);

            AddTransition(IV, VofV);
            AddTransition(IV, V7ofV);
            AddTransition(IV, V7ofIV);
            AddTransition(IV, IVaug);

            AddTransition(V, VofV);
            AddTransition(V, V7ofV);
            AddTransition(V, vii00ofII);
            AddTransition(V, Vaug);

            AddTransition(vi, VofII);
            AddTransition(vi, vii00ofV);
            AddTransition(vi, Iaug);
            AddTransition(vi, IVaug);


            AddTransition(bVII, IIaug);


            AddTransition(VofII, ii);
            AddTransition(VofII, VofV);
            AddTransition(VofII, V7ofV);

            AddTransition(VofVI, vi);
            AddTransition(VofVI, VofII);

            AddTransition(V7ofIV, IV);
            AddTransition(V7ofIV, ii);

            AddTransition(VofV, V);
            AddTransition(VofV, V7);
            AddTransition(VofV, vii0);
            AddTransition(VofV, I.Inversion(2));

            AddTransition(V7ofV, V);
            AddTransition(V7ofV, V7);
            AddTransition(V7ofV, vii0);
            AddTransition(V7ofV, I.Inversion(2));

            AddTransition(VofVI, vi);


            AddTransition(vii00ofV, V);
            
            AddTransition(vii00ofII, ii);


            AddTransition(Iaug, ii);
            AddTransition(Iaug, IV);
            AddTransition(Iaug, vi);

            AddTransition(IIaug, V);
            AddTransition(IIaug, VofV);

            AddTransition(IVaug, ii);
            AddTransition(IVaug, VofII);

            AddTransition(Vaug, I);
            AddTransition(Vaug, iii);
            AddTransition(Vaug, VofVI);
        }
    }
}
