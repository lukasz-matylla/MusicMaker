using MusicCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        protected readonly int[] LegalSteps = new[] { -2, -1, 1, 2 };

        public override MusicalScale Scale { get; }

        public TertianHarmonyGraph(MusicalScale scale)
        {
            Scale = scale;

            GenerateChords();
            AddTransitions();
        }

        private void GenerateChords()
        {
            var chordTypes = new[] { Major, Minor, Augmented, Major7, Dominant7, Minor7, MinorMajor7, FullyDiminished7 };

            for (var rootPitch = 0; rootPitch < 12; ++rootPitch)
            {
                var root = ChordOperations.PitchToStep(rootPitch, Scale, NoteIdentificationMode.ClosestThenSharp);

                if (root == null)
                {
                    continue;
                }

                foreach (var chordType in chordTypes)
                {
                    if (root.Accidental == Accidental.None)
                    {
                        var chord = ChordOperations.StackedIntervals(root, Scale, chordType);
                        AddAllInversions(chord);
                    }
                    else
                    {
                        var sharpRoot = ChordOperations.PitchToStep(rootPitch, Scale, NoteIdentificationMode.SharpOnly);
                        var flatRoot = ChordOperations.PitchToStep(rootPitch, Scale, NoteIdentificationMode.FlatOnly);

                        if (sharpRoot == null)
                        {
                            if (flatRoot == null)
                            {
                                throw new InvalidOperationException("This case should be impossible");
                            }

                            var chord = ChordOperations.StackedIntervals(flatRoot, Scale, chordType);
                            AddAllInversions(chord);
                        }
                        else
                        {
                            if (flatRoot == null)
                            {
                                var chord = ChordOperations.StackedIntervals(sharpRoot, Scale, chordType);
                                AddAllInversions(chord);
                            }
                            else
                            {
                                var chord1 = ChordOperations.StackedIntervals(flatRoot, Scale, chordType);
                                var chord2 = ChordOperations.StackedIntervals(sharpRoot, Scale, chordType);

                                var d = chord1.Notes.Sum(n => Math.Abs((int)n.Accidental)) - chord2.Notes.Sum(n => Math.Abs((int)n.Accidental));

                                if (d < 0)
                                {
                                    AddAllInversions(chord1);
                                }
                                else
                                {
                                    AddAllInversions(chord2);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AddAllInversions(Chord chord)
        {
            for (var i = 0; i < chord.Notes.Count; i++)
            {
                AddOrUpdateChord(chord.Inversion(i));
            }
        }

        private void AddTransitions()
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
