using MusicCore;

namespace Composer
{
    public class SimpleMajorChordProgressionGraph : ChordTransitionGraph
    {
        public override MusicalScale Scale => MusicalScale.Major;

        protected readonly Chord I = new Chord(0, 2, 4);

        protected readonly Chord ii = new Chord(1, 3, 5);

        protected readonly Chord iii = new Chord(2, 4, 6);

        protected readonly Chord IV = new Chord(3, 5, 0);

        protected readonly Chord V = new Chord(4, 6, 1);
        protected readonly Chord V7 = new Chord(4, 6, 1, 3);

        protected readonly Chord vi = new Chord(5, 0, 2);

        protected readonly Chord vii0 = new Chord(6, 1, 3);

        public SimpleMajorChordProgressionGraph()
            : base()
        {
            AddTransition(I, I.Inversion(1));
            AddTransition(I, V.Inversion(2));
            AddTransition(I, ii);
            AddTransition(I, iii);
            AddTransition(I, IV);
            AddTransition(I, V);
            AddTransition(I, V7);
            AddTransition(I, vi);

            AddTransition(I.Inversion(1), ii);
            AddTransition(I.Inversion(1), iii);
            AddTransition(I.Inversion(1), IV);
            AddTransition(I.Inversion(1), V);
            AddTransition(I.Inversion(1), V7);

            AddTransition(ii, V);
            AddTransition(ii, V7);
            AddTransition(ii, I.Inversion(2));
            AddTransition(ii, vii0.Inversion(1));

            AddTransition(V.Inversion(2), I.Inversion(1));
            AddTransition(V.Inversion(2), iii);

            AddTransition(iii, vi);
            AddTransition(iii, IV);
            AddTransition(iii, ii);
            AddTransition(iii, I.Inversion(1));

            AddTransition(IV, I);
            AddTransition(IV, ii);
            AddTransition(IV, IV);
            AddTransition(IV, V);
            AddTransition(IV, V7);
            AddTransition(IV, I.Inversion(2));
            AddTransition(IV, vii0.Inversion(1));

            AddTransition(V, I);
            AddTransition(V, V);
            AddTransition(V, V7);
            AddTransition(V, vi);

            AddTransition(V7, I);
            AddTransition(V7, V7);
            AddTransition(V7, vi);

            AddTransition(I.Inversion(2), I.Inversion(2));
            AddTransition(I.Inversion(2), V);
            AddTransition(I.Inversion(2), V7);

            AddTransition(vi, IV);
            AddTransition(vi, ii);

            AddTransition(vii0.Inversion(1), I);
            AddTransition(vii0.Inversion(1), I.Inversion(2));
        }
    }
}
