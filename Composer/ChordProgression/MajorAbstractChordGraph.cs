namespace Composer.ChordProgression
{
    internal class MajorAbstractChordGraph : AbstractChordGraph
    {
        protected readonly FlaggedAbstractChord I = new FlaggedAbstractChord(0, ChordType.Major, 0)
        {
            Function = HarmonicFunction.AnyTonic,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord ii = new FlaggedAbstractChord(1, ChordType.Minor, 0)
        {
            Function = HarmonicFunction.AnyPredominant,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord iii = new FlaggedAbstractChord(2, ChordType.Minor, null)
        {
            Function = HarmonicFunction.Tonic,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord IV = new FlaggedAbstractChord(3, ChordType.Major, 0)
        {
            Function = HarmonicFunction.AnyPredominant,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord V = new FlaggedAbstractChord(4, ChordType.Major, 0)
        {
            Function = HarmonicFunction.AnyDominant | HarmonicFunction.DominantStrong,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord V7 = new FlaggedAbstractChord(4, ChordType.Dominant7, 0)
        {
            Function = HarmonicFunction.DominantFinal | HarmonicFunction.DominantStrong,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord vi = new FlaggedAbstractChord(5, ChordType.Minor, null)
        {
            Function = HarmonicFunction.Tonic | HarmonicFunction.TonicFinal,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord vii0 = new FlaggedAbstractChord(6, ChordType.Diminished, 1)
        {
            Function = HarmonicFunction.DominantFinal,
            Flags = ChordFlags.Diatonic
        };

        public MajorAbstractChordGraph() 
            : base()
        {
            AddTransition(I, I);
            AddTransition(I, ii);
            AddTransition(I, iii);
            AddTransition(I, IV, 3);
            AddTransition(I, V);
            AddTransition(I, V7);
            AddTransition(I, vi);
            AddTransition(I, vii0);

            AddTransition(ii, ii);
            AddTransition(ii, V, 3);
            AddTransition(ii, V7, 3);
            AddTransition(ii, vii0);

            AddTransition(iii, I);
            AddTransition(iii, iii);
            AddTransition(iii, vi, 3);

            AddTransition(IV, I, 3);
            AddTransition(IV, ii);
            AddTransition(IV, IV);
            AddTransition(IV, V, 3);
            AddTransition(IV, V7, 3);
            AddTransition(IV, vii0);

            AddTransition(V, I, 3);
            AddTransition(V, V);
            AddTransition(V, V7, 3);
            AddTransition(V, vi);
            AddTransition(V, vii0);

            AddTransition(V7, I, 3);
            AddTransition(V7, V7);
            AddTransition(V7, vi);

            AddTransition(vi, ii, 3);
            AddTransition(vi, IV, 3);
            AddTransition(vi, V);
            AddTransition(vi, vi);

            AddTransition(vii0, I);
            AddTransition(vii0, vii0);
        }
    }
}
