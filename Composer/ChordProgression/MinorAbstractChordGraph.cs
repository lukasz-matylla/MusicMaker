namespace Composer.ChordProgression
{
    internal class MinorAbstractChordGraph : AbstractChordGraph
    {
        #region I
        protected readonly FlaggedAbstractChord i = new FlaggedAbstractChord(0, ChordType.Minor, 0)
        {
            Function = HarmonicFunction.TonicInitial,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord ix = new FlaggedAbstractChord(0, ChordType.Minor, null)
        {
            Function = HarmonicFunction.Tonic | HarmonicFunction.TonicFinal,
            Flags = ChordFlags.Diatonic
        };
        #endregion

        #region II
        protected readonly FlaggedAbstractChord ii0 = new FlaggedAbstractChord(1, ChordType.Diminished, 1)
        {
            Function = HarmonicFunction.Tonic | HarmonicFunction.AnyPredominant,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord ii = new FlaggedAbstractChord(1, ChordType.Minor, 0)
        {
            Function = HarmonicFunction.AnyPredominant,
            Flags = ChordFlags.MelodicMinor
        };

        protected readonly FlaggedAbstractChord iix = new FlaggedAbstractChord(1, ChordType.Minor, null)
        {
            Function = HarmonicFunction.Predominant | HarmonicFunction.PredominantFinal,
            Flags = ChordFlags.MelodicMinor
        };
        #endregion

        #region III
        protected readonly FlaggedAbstractChord III = new FlaggedAbstractChord(2, ChordType.Major, null)
        {
            Function = HarmonicFunction.Tonic | HarmonicFunction.TonicFinal,
            Flags = ChordFlags.Diatonic
        };
        #endregion

        #region IV
        protected readonly FlaggedAbstractChord iv = new FlaggedAbstractChord(3, ChordType.Minor, 0)
        {
            Function = HarmonicFunction.AnyPredominant,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord ivx = new FlaggedAbstractChord(3, ChordType.Minor, null)
        {
            Function = HarmonicFunction.Predominant | HarmonicFunction.PredominantFinal,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord IV = new FlaggedAbstractChord(3, ChordType.Major, 0)
        {
            Function = HarmonicFunction.AnyPredominant,
            Flags = ChordFlags.MelodicMinor
        };

        protected readonly FlaggedAbstractChord IVx = new FlaggedAbstractChord(3, ChordType.Major, null)
        {
            Function = HarmonicFunction.Predominant | HarmonicFunction.PredominantFinal,
            Flags = ChordFlags.MelodicMinor
        };
        #endregion

        #region V
        protected readonly FlaggedAbstractChord v = new FlaggedAbstractChord(4, ChordType.Minor, 0)
        {
            Function = HarmonicFunction.AnyDominant | HarmonicFunction.DominantSolo,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord vx = new FlaggedAbstractChord(4, ChordType.Minor, null)
        {
            Function = HarmonicFunction.Dominant | HarmonicFunction.DominantFinal,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord V = new FlaggedAbstractChord(4, ChordType.Major, 0)
        {
            Function = HarmonicFunction.AnyDominant | HarmonicFunction.DominantSolo | HarmonicFunction.DominantStrong,
            Flags = ChordFlags.MelodicMinor
        };

        protected readonly FlaggedAbstractChord Vx = new FlaggedAbstractChord(4, ChordType.Major, null)
        {
            Function = HarmonicFunction.Dominant | HarmonicFunction.DominantFinal,
            Flags = ChordFlags.MelodicMinor
        };

        protected readonly FlaggedAbstractChord V7 = new FlaggedAbstractChord(4, ChordType.Dominant7, 0)
        {
            Function = HarmonicFunction.DominantFinal | HarmonicFunction.DominantSolo | HarmonicFunction.DominantStrong,
            Flags = ChordFlags.MelodicMinor
        };
        #endregion

        #region VI
        protected readonly FlaggedAbstractChord VI = new FlaggedAbstractChord(5, ChordType.Major, null)
        {
            Function = HarmonicFunction.Tonic | HarmonicFunction.TonicFinal,
            Flags = ChordFlags.Diatonic
        };
        #endregion

        #region VII
        protected readonly FlaggedAbstractChord VII = new FlaggedAbstractChord(6, ChordType.Major, 0)
        {
            Function = HarmonicFunction.DominantFinal | HarmonicFunction.DominantSolo,
            Flags = ChordFlags.Diatonic
        };
        #endregion

        public MinorAbstractChordGraph()
            : base()
        {
            AddTransition(i, ix);
            AddTransition(i, ii0);
            AddTransition(i, ii);
            AddTransition(i, III);
            AddTransition(i, iv);
            AddTransition(i, IV);
            AddTransition(i, v);
            AddTransition(i, V);
            AddTransition(i, V7);
            AddTransition(i, VI);
            AddTransition(i, VII);

            AddTransition(ix, ix);
            AddTransition(ix, ii0);
            AddTransition(ix, III);
            AddTransition(ix, ii);
            AddTransition(ix, iv);
            AddTransition(ix, IV);
            AddTransition(ix, v);
            AddTransition(ix, V);
            AddTransition(ix, V7);
            AddTransition(ix, VI);
            AddTransition(ix, VII);

            AddTransition(ii0, i);
            AddTransition(ii0, ii0);
            AddTransition(ii0, ii);
            AddTransition(ii0, iix);
            AddTransition(ii0, III, 3);
            AddTransition(ii0, iv);
            AddTransition(ii0, ivx);
            AddTransition(ii0, IV);
            AddTransition(ii0, IVx);
            AddTransition(ii0, v);
            AddTransition(ii0, V);
            AddTransition(ii0, V7);
            AddTransition(ii0, VII);

            AddTransition(ii, ii0);
            AddTransition(ii, ii);
            AddTransition(ii, iix);
            AddTransition(ii, iv);
            AddTransition(ii, ivx);
            AddTransition(ii, IV);
            AddTransition(ii, IVx);
            AddTransition(ii, v);
            AddTransition(ii, V);
            AddTransition(ii, V7);
            AddTransition(ii, VII);

            AddTransition(iix, ii0);
            AddTransition(iix, ii);
            AddTransition(iix, iix);
            AddTransition(iix, iv);
            AddTransition(iix, ivx);
            AddTransition(iix, IV);
            AddTransition(iix, IVx);
            AddTransition(iix, v);
            AddTransition(iix, V);
            AddTransition(iix, V7);
            AddTransition(iix, VII);

            AddTransition(III, i);
            AddTransition(III, ix);
            AddTransition(III, III);
            AddTransition(III, VI, 3);
            AddTransition(III, v);
            AddTransition(III, V);
            AddTransition(III, V7);

            AddTransition(iv, i);
            AddTransition(iv, ii0);
            AddTransition(iv, ii);
            AddTransition(iv, iix);
            AddTransition(iv, iv);
            AddTransition(iv, ivx);
            AddTransition(iv, IV);
            AddTransition(iv, IVx);
            AddTransition(iv, v);
            AddTransition(iv, V);
            AddTransition(iv, V7);
            AddTransition(iv, VII);

            AddTransition(ivx, i);
            AddTransition(ivx, ii0);
            AddTransition(ivx, ii);
            AddTransition(ivx, iix);
            AddTransition(ivx, iv);
            AddTransition(ivx, ivx);
            AddTransition(ivx, IV);
            AddTransition(ivx, IVx);
            AddTransition(ivx, v);
            AddTransition(ivx, V);
            AddTransition(ivx, V7);
            AddTransition(ivx, VII);

            AddTransition(IV, i);
            AddTransition(IV, ii0);
            AddTransition(IV, ii);
            AddTransition(IV, iix);
            AddTransition(IV, iv);
            AddTransition(IV, ivx);
            AddTransition(IV, IV);
            AddTransition(IV, IVx);
            AddTransition(IV, v);
            AddTransition(IV, V);
            AddTransition(IV, V7);
            AddTransition(IV, VII);

            AddTransition(IVx, i);
            AddTransition(IVx, ii0);
            AddTransition(IVx, ii);
            AddTransition(IVx, iix);
            AddTransition(IVx, iv);
            AddTransition(IVx, ivx);
            AddTransition(IVx, IV);
            AddTransition(IVx, IVx);
            AddTransition(IVx, v);
            AddTransition(IVx, V);
            AddTransition(IVx, V7);
            AddTransition(IVx, VII);

            AddTransition(v, i);
            AddTransition(v, v);
            AddTransition(v, vx);
            AddTransition(v, V);
            AddTransition(v, Vx);
            AddTransition(v, V7);
            AddTransition(v, VII);

            AddTransition(vx, i);
            AddTransition(vx, v);
            AddTransition(vx, vx);
            AddTransition(vx, V);
            AddTransition(vx, Vx);
            AddTransition(vx, V7);
            AddTransition(vx, VII);

            AddTransition(V, i, 3);
            AddTransition(V, v, 0.5);
            AddTransition(V, vx, 0.5);
            AddTransition(V, V);
            AddTransition(V, Vx);
            AddTransition(V, V7);
            AddTransition(V, VII, 0.5);

            AddTransition(Vx, i, 3);
            AddTransition(Vx, v, 0.5);
            AddTransition(Vx, vx, 0.5);
            AddTransition(Vx, V);
            AddTransition(Vx, Vx);
            AddTransition(Vx, V7);
            AddTransition(Vx, VII, 0.5);

            AddTransition(V7, i, 3);
            AddTransition(V7, V7);

            AddTransition(VI, i);
            AddTransition(VI, ix);
            AddTransition(VI, ii0);
            AddTransition(VI, ii);
            AddTransition(VI, III);
            AddTransition(VI, iv);
            AddTransition(VI, IV);
            AddTransition(VI, v, 0.5);
            AddTransition(VI, VI);
            AddTransition(VI, VII, 3);

            AddTransition(VII, i);
            AddTransition(VII, v); 
            AddTransition(VII, VII);
        }
    }
}
