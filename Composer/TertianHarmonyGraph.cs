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

        public override MusicalScale Scale { get; }

        public TertianHarmonyGraph(MusicalScale scale)
        {
            Scale = scale;

            GenerateChords();
            AddTransitions();
        }

        protected virtual void GenerateChords()
        {
            for (var rootPitch = 0; rootPitch < 12; ++rootPitch)
            {
                foreach (var chordType in ChordTypes)
                {
                    var chord = ChordOperations.StackedIntervalsMinimumAccidentals(rootPitch, Scale, chordType);
                    if (chord != null)
                    {
                        AddOrUpdateChordAllInversions(chord);
                    }
                }
            }
        }

        protected virtual void AddTransitions()
        {
            foreach (var chord in Chords)
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

                    if (FindChordIndex(newChord) >= 0)
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
                    .Concat(new[] { ChordOperations.HalftoneMove(rootPosition.Notes[0], interval, Scale) })
                    .ToArray());

                if (rootIndex > 0)
                {
                    newChord = newChord.Inversion(3 - rootIndex);
                }
                
                if (FindChordIndex(newChord) >= 0)
                {
                    AddTransition(chord, newChord);
                    AddTransition(newChord, chord);
                }
            }
        }
    }
}
