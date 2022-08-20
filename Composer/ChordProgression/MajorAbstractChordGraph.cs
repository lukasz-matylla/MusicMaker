using MusicCore;

namespace Composer.ChordProgression
{
    internal class MajorAbstractChordGraph : AbstractChordGraph
    {

        #region I
        protected readonly FlaggedAbstractChord I = new FlaggedAbstractChord(0, ChordType.Major, 0)
        {
            Function = HarmonicFunction.TonicInitial,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord Ix = new FlaggedAbstractChord(0, ChordType.Major, null)
        {
            Function = HarmonicFunction.Tonic | HarmonicFunction.TonicFinal,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord I64 = new FlaggedAbstractChord(0, ChordType.Major, 2)
        {
            Function = HarmonicFunction.DominantInitial,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord I7 = new FlaggedAbstractChord(0, ChordType.Dominant7, null)
        {
            Function = HarmonicFunction.TonicFinal,
            Flags = ChordFlags.SecondaryDominant
        };

        protected readonly FlaggedAbstractChord Imaj7 = new FlaggedAbstractChord(0, ChordType.Major7, 0)
        {
            Function = HarmonicFunction.TonicInitial,
            Flags = ChordFlags.Extended
        };

        protected readonly FlaggedAbstractChord Imaj7x = new FlaggedAbstractChord(0, ChordType.Major7, null)
        {
            Function = HarmonicFunction.Tonic | HarmonicFunction.TonicFinal,
            Flags = ChordFlags.Extended
        };

        protected readonly FlaggedAbstractChord Iaug = new FlaggedAbstractChord(0, ChordType.Augmented, null)
        {
            Function = HarmonicFunction.Tonic | HarmonicFunction.TonicFinal,
        };
        #endregion

        #region bII
        protected readonly FlaggedAbstractChord bii = new FlaggedAbstractChord(new ScaleStep(1, Accidental.Flat), ChordType.Minor, null)
        {
            Function = HarmonicFunction.Tonic | HarmonicFunction.TonicFinal,
            Flags = ChordFlags.ChromaticMediant
        };

        protected readonly FlaggedAbstractChord N = new FlaggedAbstractChord(new ScaleStep(1, Accidental.Flat), ChordType.Major, 1)
        {
            Function = HarmonicFunction.PredominantInitial | HarmonicFunction.PredominantFinal,
            Flags = ChordFlags.Neapolitan
        };

        protected readonly FlaggedAbstractChord bII7 = new FlaggedAbstractChord(new ScaleStep(1, Accidental.Flat), ChordType.Dominant7, 0)
        {
            Function = HarmonicFunction.Dominant | HarmonicFunction.DominantFinal | HarmonicFunction.DominantSolo | HarmonicFunction.DominantStrong,
            Flags = ChordFlags.TritoneSubstitution
        };
        #endregion

        #region II
        protected readonly FlaggedAbstractChord ii = new FlaggedAbstractChord(1, ChordType.Minor, 0)
        {
            Function = HarmonicFunction.AnyPredominant,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord iix = new FlaggedAbstractChord(1, ChordType.Minor, null)
        {
            Function = HarmonicFunction.Predominant | HarmonicFunction.PredominantFinal,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord iimin7 = new FlaggedAbstractChord(1, ChordType.Minor7, 0)
        {
            Function = HarmonicFunction.AnyPredominant,
            Flags = ChordFlags.Extended
        };

        protected readonly FlaggedAbstractChord iimin7x = new FlaggedAbstractChord(1, ChordType.Minor7, null)
        {
            Function = HarmonicFunction.Predominant | HarmonicFunction.PredominantFinal,
            Flags = ChordFlags.Extended
        };

        protected readonly FlaggedAbstractChord iiminmaj7 = new FlaggedAbstractChord(1, ChordType.MinMaj7, 0)
        {
            Function = HarmonicFunction.AnyPredominant,
            Flags = ChordFlags.Altered
        };

        protected readonly FlaggedAbstractChord II7 = new FlaggedAbstractChord(1, ChordType.Dominant7, null)
        {
            Function = HarmonicFunction.PredominantFinal,
            Flags = ChordFlags.SecondaryDominant
        };
        #endregion

        #region bIII
        protected readonly FlaggedAbstractChord bIII = new FlaggedAbstractChord(new ScaleStep(2, Accidental.Flat), ChordType.Major, null)
        {
            Function = HarmonicFunction.Tonic,
            Flags = ChordFlags.ChromaticMediant
        };
        #endregion

        #region III
        protected readonly FlaggedAbstractChord iii = new FlaggedAbstractChord(2, ChordType.Minor, null)
        {
            Function = HarmonicFunction.Tonic | HarmonicFunction.TonicFinal,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord III = new FlaggedAbstractChord(2, ChordType.Major, null)
        {
            Function = HarmonicFunction.Tonic,
            Flags = ChordFlags.ChromaticMediant
        };

        protected readonly FlaggedAbstractChord III7 = new FlaggedAbstractChord(2, ChordType.Dominant7, null)
        {
            Function = HarmonicFunction.Tonic,
            Flags = ChordFlags.SecondaryDominant
        };
        #endregion

        #region IV
        protected readonly FlaggedAbstractChord iv = new FlaggedAbstractChord(3, ChordType.Minor, null)
        {
            Function = HarmonicFunction.Predominant | HarmonicFunction.PredominantFinal,
            Flags = ChordFlags.Borrowed
        };

        protected readonly FlaggedAbstractChord IV = new FlaggedAbstractChord(3, ChordType.Major, 0)
        {
            Function = HarmonicFunction.AnyPredominant,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord IVx = new FlaggedAbstractChord(3, ChordType.Major, null)
        {
            Function = HarmonicFunction.Predominant | HarmonicFunction.PredominantFinal,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord IV7 = new FlaggedAbstractChord(3, ChordType.Dominant7, null)
        {
            Function = HarmonicFunction.PredominantFinal,
            Flags = ChordFlags.SecondaryDominant
        };

        protected readonly FlaggedAbstractChord IVmaj7 = new FlaggedAbstractChord(3, ChordType.Major7, 0)
        {
            Function = HarmonicFunction.AnyPredominant,
            Flags = ChordFlags.Extended
        };

        protected readonly FlaggedAbstractChord IVmaj7x = new FlaggedAbstractChord(3, ChordType.Major7, null)
        {
            Function = HarmonicFunction.Predominant | HarmonicFunction.PredominantFinal,
            Flags = ChordFlags.Extended
        };

        protected readonly FlaggedAbstractChord IVsus4 = new FlaggedAbstractChord(3, ChordType.Sus4, 0)
        {
            Function = HarmonicFunction.AnyPredominant
        };
        #endregion

        #region V
        protected readonly FlaggedAbstractChord v = new FlaggedAbstractChord(4, ChordType.Minor, null)
        {
            Function = HarmonicFunction.Dominant | HarmonicFunction.DominantSolo | HarmonicFunction.DominantFinal,
            Flags = ChordFlags.Borrowed
        };

        protected readonly FlaggedAbstractChord V = new FlaggedAbstractChord(4, ChordType.Major, 0)
        {
            Function = HarmonicFunction.AnyDominant | HarmonicFunction.DominantSolo | HarmonicFunction.DominantStrong,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord Vx = new FlaggedAbstractChord(4, ChordType.Major, null)
        {
            Function = HarmonicFunction.Dominant | HarmonicFunction.DominantFinal,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord V7 = new FlaggedAbstractChord(4, ChordType.Dominant7, 0)
        {
            Function = HarmonicFunction.DominantFinal | HarmonicFunction.DominantSolo | HarmonicFunction.DominantStrong,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord Vaug = new FlaggedAbstractChord(4, ChordType.Augmented, null)
        {
            Function = HarmonicFunction.DominantFinal | HarmonicFunction.DominantSolo | HarmonicFunction.DominantStrong,
        };

        protected readonly FlaggedAbstractChord V7b5 = new FlaggedAbstractChord(4, ChordType.DominantB5, 0)
        {
            Function = HarmonicFunction.DominantFinal | HarmonicFunction.DominantSolo | HarmonicFunction.DominantStrong,
            Flags = ChordFlags.Altered
        };

        protected readonly FlaggedAbstractChord Vsus4 = new FlaggedAbstractChord(4, ChordType.Sus4, 0)
        {
            Function = HarmonicFunction.AnyDominant,
            Flags = ChordFlags.Diatonic
        };
        #endregion

        #region bVI
        protected readonly FlaggedAbstractChord bVI = new FlaggedAbstractChord(new ScaleStep(5, Accidental.Flat), ChordType.Major, null)
        {
            Function = HarmonicFunction.PredominantFinal,
            Flags = ChordFlags.Borrowed
        };

        protected readonly FlaggedAbstractChord bvi07 = new FlaggedAbstractChord(new ScaleStep(5, Accidental.Flat), ChordType.FullyDiminished, null)
        {
            Function = HarmonicFunction.PredominantFinal,
            Flags = ChordFlags.Extended
        };
        #endregion

        #region VI
        protected readonly FlaggedAbstractChord vi = new FlaggedAbstractChord(5, ChordType.Minor, null)
        {
            Function = HarmonicFunction.Tonic | HarmonicFunction.TonicFinal,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord viminmaj7 = new FlaggedAbstractChord(5, ChordType.MinMaj7, null)
        {
            Function = HarmonicFunction.Tonic | HarmonicFunction.TonicFinal,
            Flags = ChordFlags.Altered
        };

        protected readonly FlaggedAbstractChord VI = new FlaggedAbstractChord(5, ChordType.Major, null)
        {
            Function = HarmonicFunction.Tonic | HarmonicFunction.TonicFinal,
            Flags = ChordFlags.ChromaticMediant
        };

        protected readonly FlaggedAbstractChord VI7 = new FlaggedAbstractChord(5, ChordType.Dominant7, null)
        {
            Function = HarmonicFunction.TonicFinal,
            Flags = ChordFlags.SecondaryDominant
        };
        #endregion

        #region bVII
        protected readonly FlaggedAbstractChord bVII = new FlaggedAbstractChord(new ScaleStep(6, Accidental.Flat), ChordType.Major, 0)
        {
            Function = HarmonicFunction.PredominantInitial | HarmonicFunction.Predominant | HarmonicFunction.DominantFinal | HarmonicFunction.DominantSolo,
            Flags = ChordFlags.Borrowed
        };
        #endregion

        #region VII
        protected readonly FlaggedAbstractChord vii0 = new FlaggedAbstractChord(6, ChordType.Diminished, 1)
        {
            Function = HarmonicFunction.AnyDominant,
            Flags = ChordFlags.Diatonic
        };

        protected readonly FlaggedAbstractChord vii07 = new FlaggedAbstractChord(6, ChordType.FullyDiminished, null)
        {
            Function = HarmonicFunction.Dominant | HarmonicFunction.DominantFinal,
            Flags = ChordFlags.Extended
        };
        #endregion

        public MajorAbstractChordGraph() 
            : base()
        {
            AddTransition(I, Ix);
            AddTransition(I, I7);
            AddTransition(I, Imaj7x);
            AddTransition(I, Iaug);
            AddTransition(I, I64);
            AddTransition(I, bii);
            AddTransition(I, N);
            AddTransition(I, ii);
            AddTransition(I, iimin7);
            AddTransition(I, iiminmaj7);
            AddTransition(I, iii);
            AddTransition(I, III);
            AddTransition(I, iv);
            AddTransition(I, IV, 3);
            AddTransition(I, IVmaj7, 3);
            AddTransition(I, v);
            AddTransition(I, V);
            AddTransition(I, V7);
            AddTransition(I, V7b5);
            AddTransition(I, vi);
            AddTransition(I, viminmaj7);
            AddTransition(I, bVII);
            AddTransition(I, vii0, 0.2);

            AddTransition(Ix, Ix);
            AddTransition(Ix, Imaj7x);
            AddTransition(Ix, Iaug);
            AddTransition(Ix, I7);
            AddTransition(Ix, I64);
            AddTransition(Ix, bii);
            AddTransition(Ix, N);
            AddTransition(Ix, ii);
            AddTransition(Ix, iimin7);
            AddTransition(Ix, iiminmaj7);
            AddTransition(Ix, iii);
            AddTransition(Ix, III);
            AddTransition(Ix, iv);
            AddTransition(Ix, IV, 3);
            AddTransition(Ix, IVmaj7, 3);
            AddTransition(Ix, v);
            AddTransition(Ix, V);
            AddTransition(Ix, V7);
            AddTransition(Ix, V7b5);
            AddTransition(Ix, vi);
            AddTransition(Ix, viminmaj7);
            AddTransition(Ix, bVII);
            AddTransition(Ix, vii0, 0.2);

            AddTransition(Imaj7, Ix, 0.5);
            AddTransition(Imaj7, I7, 0.5);
            AddTransition(Imaj7, Imaj7x, 0.5);
            AddTransition(Imaj7, I64);
            AddTransition(Imaj7, bii, 0.5);
            AddTransition(Imaj7, N, 0.5);
            AddTransition(Imaj7, ii, 0.5);
            AddTransition(Imaj7, iimin7, 0.5);
            AddTransition(Imaj7, iiminmaj7, 0.5);
            AddTransition(Imaj7, iii, 0.5);
            AddTransition(Imaj7, III);
            AddTransition(Imaj7, iv, 0.5);
            AddTransition(Imaj7, IV);
            AddTransition(Imaj7, IVmaj7);
            AddTransition(Imaj7, IVsus4, 3);
            AddTransition(Imaj7, v);
            AddTransition(Imaj7, V, 0.5);
            AddTransition(Imaj7, V7, 0.5);
            AddTransition(Imaj7, V7b5, 0.5);
            AddTransition(Imaj7, vi, 0.5);
            AddTransition(Imaj7, viminmaj7, 0.5);
            AddTransition(Imaj7, bVII, 0.5);
            AddTransition(Imaj7, vii0, 0.2);
                           
            AddTransition(Imaj7x, Ix, 0.5);
            AddTransition(Imaj7x, Imaj7x, 0.5);
            AddTransition(Imaj7x, I7, 0.5);
            AddTransition(Imaj7x, I64);
            AddTransition(Imaj7x, bii, 0.5);
            AddTransition(Imaj7x, N, 0.5);
            AddTransition(Imaj7x, ii, 0.5);
            AddTransition(Imaj7x, iimin7, 0.5);
            AddTransition(Imaj7x, iiminmaj7, 0.5);
            AddTransition(Imaj7x, iii, 0.5);
            AddTransition(Imaj7x, III, 0.5); 
            AddTransition(Imaj7x, iv, 0.5);
            AddTransition(Imaj7x, IV);
            AddTransition(Imaj7x, IVmaj7);
            AddTransition(Imaj7x, IVsus4, 3);
            AddTransition(Imaj7x, v, 0.5);
            AddTransition(Imaj7x, V, 0.5);
            AddTransition(Imaj7x, V7, 0.5);
            AddTransition(Imaj7x, V7b5, 0.5);
            AddTransition(Imaj7x, vi, 0.5);
            AddTransition(Imaj7x, viminmaj7, 0.5);
            AddTransition(Imaj7x, bVII, 0.5);
            AddTransition(Imaj7x, vii0, 0.2);

            AddTransition(I7, N);
            AddTransition(I7, IV, 3);
            AddTransition(I7, IVmaj7, 3);
            AddTransition(I7, IVsus4);
            AddTransition(I7, iv);

            AddTransition(I64, V, 3);
            AddTransition(I64, Vx);
            AddTransition(I64, V7, 3);
            AddTransition(I64, v);
            AddTransition(I64, Vsus4, 3);

            AddTransition(Iaug, Ix);
            AddTransition(Iaug, Iaug);
            AddTransition(Iaug, bii);
            AddTransition(Iaug, bII7);
            AddTransition(Iaug, III);
            AddTransition(Iaug, IV);
            AddTransition(Iaug, vi);
            AddTransition(Iaug, viminmaj7);

            AddTransition(bii, I);
            AddTransition(bii, Ix);
            AddTransition(bii, N);
            AddTransition(bii, ii);
            AddTransition(bii, bII7);
            AddTransition(bii, VI);

            AddTransition(N, I64);
            AddTransition(N, v);
            AddTransition(N, V);
            AddTransition(N, V7);
            AddTransition(N, V7b5);

            AddTransition(bII7, I, 3);
            AddTransition(bII7, Imaj7, 3);

            AddTransition(ii, I64, 3);
            AddTransition(ii, N);
            AddTransition(ii, bII7, 3);
            AddTransition(ii, ii);
            AddTransition(ii, iix);
            AddTransition(ii, iimin7);
            AddTransition(ii, iimin7x);
            AddTransition(ii, iiminmaj7);
            AddTransition(ii, II7);
            AddTransition(ii, v);
            AddTransition(ii, V, 3);
            AddTransition(ii, V7, 3);
            AddTransition(ii, V7b5, 3);
            AddTransition(ii, bVII);
            AddTransition(ii, vii0, 3);
            AddTransition(ii, vii07);

            AddTransition(iix, I64, 3);
            AddTransition(iix, N);
            AddTransition(iix, bII7, 3);
            AddTransition(iix, iix);
            AddTransition(iix, iimin7x);
            AddTransition(iix, II7);
            AddTransition(iix, v);
            AddTransition(iix, V, 3);
            AddTransition(iix, V7, 3);
            AddTransition(iix, V7b5, 3);
            AddTransition(iix, bVII);
            AddTransition(iix, vii0, 3);
            AddTransition(iix, vii07);

            AddTransition(iimin7, I64, 3);
            AddTransition(iimin7, N);
            AddTransition(iimin7, bII7, 3);
            AddTransition(iimin7, iix);
            AddTransition(iimin7, iimin7);
            AddTransition(iimin7, iimin7x);
            AddTransition(iimin7, iiminmaj7);
            AddTransition(iimin7, II7);
            AddTransition(iimin7, v);
            AddTransition(iimin7, V, 3);
            AddTransition(iimin7, V7, 3);
            AddTransition(iimin7, V7b5, 3);
            AddTransition(iimin7, bVII);
            AddTransition(iimin7, vii0);
            AddTransition(iimin7, vii07);

            AddTransition(iimin7x, I64);
            AddTransition(iimin7x, N, 0.5);
            AddTransition(iimin7x, bII7);
            AddTransition(iimin7x, iix, 0.5);
            AddTransition(iimin7x, iimin7x, 0.5);
            AddTransition(iimin7x, II7, 0.5);
            AddTransition(iimin7x, v, 0.5);
            AddTransition(iimin7x, V);
            AddTransition(iimin7x, V7);
            AddTransition(iimin7x, V7b5);
            AddTransition(iimin7x, bVII, 0.5);
            AddTransition(iimin7x, vii0, 0.5);
            AddTransition(iimin7x, vii07, 0.5);

            AddTransition(iiminmaj7, I64, 3);
            AddTransition(iiminmaj7, N);
            AddTransition(iiminmaj7, bII7, 3);
            AddTransition(iiminmaj7, iix);
            AddTransition(iiminmaj7, iimin7);
            AddTransition(iiminmaj7, iimin7x);
            AddTransition(iiminmaj7, II7);
            AddTransition(iiminmaj7, v);
            AddTransition(iiminmaj7, V, 3);
            AddTransition(iiminmaj7, V7, 3);
            AddTransition(iiminmaj7, V7b5, 3);
            AddTransition(iiminmaj7, bVII);
            AddTransition(iiminmaj7, vii0);
            AddTransition(iiminmaj7, vii07);

            AddTransition(II7, I64);
            AddTransition(II7, bII7, 3);
            AddTransition(II7, V, 3);
            AddTransition(II7, V7, 3);
            AddTransition(II7, V7b5, 3);

            AddTransition(bIII, iii);

            AddTransition(iii, Ix);
            AddTransition(iii, Imaj7x);
            AddTransition(iii, I7);
            AddTransition(iii, bIII);
            AddTransition(iii, iii);
            AddTransition(iii, III);
            AddTransition(iii, III7);
            AddTransition(iii, vi, 3);
            AddTransition(iii, viminmaj7, 3);
            AddTransition(iii, vii0, 3);
            AddTransition(iii, vii07);

            AddTransition(III, Iaug);
            AddTransition(III, vi);
            AddTransition(III, viminmaj7);

            AddTransition(III7, vi);
            AddTransition(III7, viminmaj7);

            AddTransition(iv, I, 5);
            AddTransition(iv, Imaj7, 3);
            AddTransition(iv, I64);
            AddTransition(iv, N);
            AddTransition(iv, iix);
            AddTransition(iv, iimin7x);
            AddTransition(iv, iv);
            AddTransition(iv, IVx);
            AddTransition(iv, IV7);
            AddTransition(iv, IVmaj7x);
            AddTransition(iv, v, 3);
            AddTransition(iv, V, 3);
            AddTransition(iv, V7, 3);
            AddTransition(iv, V7b5, 3);
            AddTransition(iv, vii0, 0.2);

            AddTransition(IV, I, 3);
            AddTransition(IV, I64, 3);
            AddTransition(IV, Imaj7, 3);
            AddTransition(IV, N);
            AddTransition(IV, iix);
            AddTransition(IV, iimin7x);
            AddTransition(IV, iv);
            AddTransition(IV, IV);
            AddTransition(IV, IVx, 3);
            AddTransition(IV, IVmaj7, 3);
            AddTransition(IV, IVmaj7x, 3);
            AddTransition(IV, IV7);
            AddTransition(IV, v);
            AddTransition(IV, V, 3);
            AddTransition(IV, V7, 3);
            AddTransition(IV, V7b5, 3);

            AddTransition(IVx, N);
            AddTransition(IVx, iix);
            AddTransition(IVx, iimin7x);
            AddTransition(IVx, iv);
            AddTransition(IVx, IV);
            AddTransition(IVx, IVx);
            AddTransition(IVx, IVmaj7x);
            AddTransition(IVx, IV7);
            AddTransition(IVx, bVII);

            AddTransition(IVmaj7, I);
            AddTransition(IVmaj7, Imaj7);
            AddTransition(IVmaj7, I64);
            AddTransition(IVmaj7, N, 0.5);
            AddTransition(IVmaj7, iix, 0.5);
            AddTransition(IVmaj7, iimin7x, 0.5);
            AddTransition(IVmaj7, iv, 0.5);
            AddTransition(IVmaj7, IV, 0.5);
            AddTransition(IVmaj7, IVx, 0.5);
            AddTransition(IVmaj7, IVmaj7, 0.5);
            AddTransition(IVmaj7, IVmaj7x, 0.5);
            AddTransition(IVmaj7, IV7, 0.5);
            AddTransition(IVmaj7, v, 0.5);
            AddTransition(IVmaj7, V);
            AddTransition(IVmaj7, V7);
            AddTransition(IVmaj7, V7b5);

            AddTransition(IVmaj7x, N, 0.5);
            AddTransition(IVmaj7x, iix, 0.5);
            AddTransition(IVmaj7x, iimin7x, 0.5);
            AddTransition(IVmaj7x, iv, 0.5);
            AddTransition(IVmaj7x, IV, 0.5);
            AddTransition(IVmaj7x, IVx, 0.5);
            AddTransition(IVmaj7x, IVmaj7x, 0.5);
            AddTransition(IVmaj7x, IV7, 0.5);
            AddTransition(IVmaj7x, bVII, 0.5);

            AddTransition(IV7, bVII);

            AddTransition(IVsus4, IV, 3);
            AddTransition(IVsus4, iv);
            AddTransition(IVsus4, V, 3);
            AddTransition(IVsus4, V7, 3);

            AddTransition(v, I, 3);
            AddTransition(v, Imaj7, 3);
            AddTransition(v, v);
            AddTransition(v, V);
            AddTransition(v, V7, 3);
            AddTransition(v, V7b5, 3);
            AddTransition(v, Vaug);
            AddTransition(v, vi);
            AddTransition(v, bVII);
            AddTransition(v, vii0, 0.2);
            AddTransition(v, vii07, 0.2);

            AddTransition(V, I, 3);
            AddTransition(V, Imaj7, 3);
            AddTransition(V, v);
            AddTransition(V, V);
            AddTransition(V, Vx, 3);
            AddTransition(V, V7, 3);
            AddTransition(V, V7b5);
            AddTransition(V, Vaug);
            AddTransition(V, vi);
            AddTransition(V, vii0, 0.2);
            AddTransition(V, vii07, 0.2);

            AddTransition(Vx, I);
            AddTransition(Vx, Imaj7);
            AddTransition(Vx, v);
            AddTransition(Vx, V);
            AddTransition(Vx, Vx);
            AddTransition(Vx, V7, 3);
            AddTransition(Vx, V7b5);
            AddTransition(Vx, Vaug);
            AddTransition(Vx, bVII, 0.2);
            AddTransition(Vx, vii0, 0.2);
            AddTransition(Vx, vii07, 0.2);

            AddTransition(V7, I, 3);
            AddTransition(V7, Imaj7, 3);
            AddTransition(V7, V7);
            AddTransition(V7, V7b5);
            AddTransition(V7, vi);

            AddTransition(Vaug, I);
            AddTransition(Vaug, Imaj7);
            AddTransition(Vaug, V);
            AddTransition(Vaug, Vx);
            AddTransition(Vaug, Vaug);

            AddTransition(V7b5, I);

            AddTransition(Vsus4, V, 3);

            AddTransition(bVI, V, 3);
            AddTransition(bVI, V7, 3);
            AddTransition(bVI, bVI);
            AddTransition(bVI, bvi07);
            AddTransition(bVI, bVII);

            AddTransition(bvi07, V);
            AddTransition(bvi07, V7);

            AddTransition(vi, Ix);
            AddTransition(vi, Imaj7x);
            AddTransition(vi, ii, 3);
            AddTransition(vi, iimin7, 3);
            AddTransition(vi, iiminmaj7);
            AddTransition(vi, iv);
            AddTransition(vi, IV, 3);
            AddTransition(vi, IVmaj7, 3);
            AddTransition(vi, V);
            AddTransition(vi, bVI);
            AddTransition(vi, bvi07);
            AddTransition(vi, vi);
            AddTransition(vi, viminmaj7);
            AddTransition(vi, VI);
            AddTransition(vi, VI7);

            AddTransition(viminmaj7, Ix);
            AddTransition(viminmaj7, Imaj7x);
            AddTransition(viminmaj7, ii, 3);
            AddTransition(viminmaj7, iimin7, 3);
            AddTransition(viminmaj7, iiminmaj7);
            AddTransition(viminmaj7, iv);
            AddTransition(viminmaj7, IV, 3);
            AddTransition(viminmaj7, IVmaj7, 3);
            AddTransition(viminmaj7, V);
            AddTransition(viminmaj7, bVI);
            AddTransition(viminmaj7, bvi07);
            AddTransition(viminmaj7, vi);
            AddTransition(viminmaj7, viminmaj7);
            AddTransition(viminmaj7, VI);
            AddTransition(viminmaj7, VI7);

            AddTransition(VI, ii);
            AddTransition(VI, iimin7);
            AddTransition(VI, iiminmaj7);
            AddTransition(VI, vi);

            AddTransition(VI7, ii);
            AddTransition(VI7, iimin7);
            AddTransition(VI7, iiminmaj7);

            AddTransition(bVII, I);
            AddTransition(bVII, Imaj7);
            AddTransition(bVII, IVsus4);
            AddTransition(bVII, bVI);
            AddTransition(bVII, bVII);

            AddTransition(vii0, I, 0.2);
            AddTransition(vii0, Imaj7, 0.2);
            AddTransition(vii0, bII7, 0.2);
            AddTransition(vii0, v);
            AddTransition(vii0, V);
            AddTransition(vii0, Vx);
            AddTransition(vii0, V7);
            AddTransition(vii0, bVII);
            AddTransition(vii0, vii0, 0.2);
            AddTransition(vii0, vii07);

            AddTransition(vii07, I, 0.2);
            AddTransition(vii07, Imaj7, 0.2);
            AddTransition(vii07, bII7);
            AddTransition(vii07, vii07);
        }
    }
}
