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

        protected readonly Chord vii0 = new Chord(6, 1, 3).Inversion(1);

        public SimpleMajorChordProgressionGraph()
            : base()
        {
            AddTransition(I, I);
            AddTransition(I, ii);
            AddTransition(I, iii);
            AddTransition(I, IV);
            AddTransition(I, V);
            AddTransition(I, V7);
            AddTransition(I, vi);

            AddTransition(ii, V, 3);
            AddTransition(ii, V7, 3);
            AddTransition(ii, I.Inversion(2));
            AddTransition(ii, vii0);

            AddTransition(iii, vi);

            AddTransition(IV, I, 3);
            AddTransition(IV, ii);
            AddTransition(IV, IV);
            AddTransition(IV, V, 3);
            AddTransition(IV, V7, 3);
            AddTransition(IV, I.Inversion(2));
            AddTransition(IV, vii0);

            AddTransition(V, I, 3);
            AddTransition(V, V);
            AddTransition(V, V7);
            AddTransition(V, vi);

            AddTransition(V7, I, 3);
            AddTransition(V7, V7);
            AddTransition(V7, vi);

            AddTransition(I.Inversion(2), V);
            AddTransition(I.Inversion(2), V7);

            AddTransition(vi, ii);
            AddTransition(vi, IV, 3);
            AddTransition(vi, V);

            AddTransition(vii0, I);
            AddTransition(vii0, I.Inversion(2));
        }
    }
}
