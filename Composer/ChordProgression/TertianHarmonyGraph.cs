using Composer.ChordProgression;
using MusicCore;

namespace Composer
{
    public class TertianHarmonyGraph : ChordTransitionGraph
    {
        protected readonly ScaleInterval[] Major = new[] { ScaleInterval.MajorThird, ScaleInterval.MinorThird };
        protected readonly ScaleInterval[] Minor = new[] { ScaleInterval.MinorThird, ScaleInterval.MajorThird };
        protected readonly ScaleInterval[] Augmented = new[] { ScaleInterval.MajorThird, ScaleInterval.MajorThird };
        protected readonly ScaleInterval[] Major7 = new[] { ScaleInterval.MajorThird, ScaleInterval.MinorThird, ScaleInterval.MajorThird };
        protected readonly ScaleInterval[] Dominant7 = new[] { ScaleInterval.MajorThird, ScaleInterval.MinorThird, ScaleInterval.MinorThird };
        protected readonly ScaleInterval[] Minor7 = new[] { ScaleInterval.MinorThird, ScaleInterval.MajorThird, ScaleInterval.MinorThird };
        protected readonly ScaleInterval[] MinorMajor7 = new[] { ScaleInterval.MinorThird, ScaleInterval.MajorThird, ScaleInterval.MajorThird };
        protected readonly ScaleInterval[] FullyDiminished7 = new[] { ScaleInterval.MinorThird, ScaleInterval.MinorThird, ScaleInterval.MinorThird };

        protected virtual ScaleInterval[][] ChordTypes => new[] { Major, Minor, Augmented, Major7, Dominant7, Minor7, MinorMajor7, FullyDiminished7 };

        protected readonly int[] LegalSteps = new[] { -2, -1, 1, 2 };

        protected readonly ChromaticApproach chromaticApproach;
        protected readonly int tonicPreference;

        public override MusicalScale Scale { get; }

        public TertianHarmonyGraph(MusicalScale scale, ChromaticApproach chromaticApproach = ChromaticApproach.MostlyDiatonic, int tonicPreference = 4)
        {
            Scale = scale;
            this.chromaticApproach = chromaticApproach;
            this.tonicPreference = tonicPreference;

            GenerateChords();

            AddTransitions();

            ApplyTonicPreference();
        }

        protected virtual void GenerateChords()
        {
            for (var rootPitch = 0; rootPitch < 12; ++rootPitch)
            {
                foreach (var chordType in ChordTypes)
                {
                    var chord = ChordOperations.StackedIntervalsMinimumAccidentals(rootPitch, Scale, chordType);
                    if (chord != null && IsSuitablyDiatonic(chord))
                    {
                        AddOrUpdateChordAllInversions(chord);
                    }
                }
            }
        }

        protected bool IsSuitablyDiatonic(Chord chord)
        {
            var diatonic = CountDiatonicNotes(chord);
            var nonDiatonic = chord.Notes.Count - diatonic;

            switch (chromaticApproach)
            {
                case ChromaticApproach.StrictlyDiatonic:
                    return nonDiatonic == 0;
                case ChromaticApproach.MostlyDiatonic:
                    return nonDiatonic <= 1;
                case ChromaticApproach.MostlyChromatic:
                    return diatonic > 0;
                case ChromaticApproach.Free:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(chromaticApproach));
            }
        }

        protected int CountDiatonicNotes(Chord chord)
        {
            return chord.Notes.Count(n => n.Accidental == Accidental.None);
        }

        protected virtual void AddTransitions()
        {
            foreach (var chord in Items)
            {
                AddDirectVoiceLeadingTransitions(chord);

                if (chord.Notes.Count == 3)
                {
                    AddSplittingTransitions(chord);
                }
            }
        }

        protected void AddTransition(int rootPitchFrom, ScaleInterval[] shapeFrom, int rootPitchTo, ScaleInterval[] shapeTo)
        {
            AddTransitionAllInversions(
                ChordOperations.StackedIntervalsMinimumAccidentals(rootPitchFrom, Scale, shapeFrom),
                ChordOperations.StackedIntervalsMinimumAccidentals(rootPitchTo, Scale, shapeTo));
        }

        private void AddDirectVoiceLeadingTransitions(Chord chord)
        {
            for (var i = 0; i < chord.Notes.Count - 1; i++)
            {
                foreach (var interval in LegalSteps)
                {
                    var newChord = ChordOperations.ModifyChord(chord, i, interval, Scale);

                    if (FindItemIndex(newChord) >= 0)
                    {
                        AddTransition(chord, newChord);
                    }
                }
            }
        }

        private void AddSplittingTransitions(Chord chord)
        {
            var rootIndex = ChordOperations.FindRootInversion(chord, Scale);
            var rootPosition = chord.Inversion(rootIndex);

            foreach (var interval in new[] { -2, -1 })
            {
                var newChord = new Chord(
                    rootPosition.Notes
                    .Concat(new[] { Scale.ChangeByHalftones(rootPosition.Notes[0], interval) })
                    .ToArray());

                if (rootIndex > 0)
                {
                    newChord = newChord.Inversion(3 - rootIndex);
                }
                
                if (FindItemIndex(newChord) >= 0)
                {
                    AddTransition(chord, newChord);
                    AddTransition(newChord, chord);
                }
            }
        }

        protected void ApplyTonicPreference()
        {
            var tonicChord = new Chord(0, 2, 4);
            var tonicIndex = FindItemIndex(tonicChord);

            foreach (var transition in transitions)
            {
                if (transition.To == tonicIndex)
                {
                    transition.Weight *= tonicPreference;
                }
            }
        }
    }
}
