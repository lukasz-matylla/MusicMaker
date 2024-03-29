﻿using Tools;

namespace MusicCore
{
    public static class ChordOperations
    {
        public static ScaleStep? PitchToStep(int pitch, MusicalScale scale, NoteIdentificationMode mode = NoteIdentificationMode.ClosestThenSharp)
        {
            var octave = pitch / 12;
            pitch %= 12;

            var upper = PitchToStepUpper(pitch, scale);
            var lower = PitchToStepLower(pitch, scale);

            ScaleStep? result = null;

            switch (mode)
            {
                case NoteIdentificationMode.SharpOnly:
                    result = lower;
                    break;
                case NoteIdentificationMode.FlatOnly:
                    result = upper;
                    break;
                case NoteIdentificationMode.PreferSharp:
                    result = lower ?? upper;
                    break;
                case NoteIdentificationMode.PreferFlat:
                    result = upper ?? lower;
                    break;
                case NoteIdentificationMode.ClosestThenSharp:
                    result = (new[] { lower, upper })
                        .OfType<ScaleStep>()
                        .OrderBy(n => Math.Abs((int)n.Accidental))
                        .ThenBy(n => -(int)n.Accidental)
                        .FirstOrDefault();
                    break;
                case NoteIdentificationMode.ClosestThanFlat:
                    result = (new[] { lower, upper })
                        .OfType<ScaleStep>()
                        .OrderBy(n => Math.Abs((int)n.Accidental))
                        .ThenBy(n => (int)n.Accidental)
                        .FirstOrDefault();
                    break;
            }

            return result != null ?
                scale.OctaveOffset(result, octave) :
                null;
        }

        public static bool IsChordEquivalent(Chord a, Chord b, MusicalScale scale)
        {
            if (a == null || b == null)
            {
                return false;
            }

            var aAsPitches = a.Notes.Select(n => scale.StepToPitch(n));
            var bAsPitches = b.Notes.Select(n => scale.StepToPitch(n));

            return aAsPitches.SequenceEqual(bAsPitches);
        }

        public static ScaleStep DiatonicMove(ScaleStep step, ScaleInterval interval, MusicalScale scale, bool strict = true)
        {
            var movedBySteps = scale.ChangeBySteps(step, interval.Steps);

            var halftoneDifference = scale.HalftoneInterval(step, movedBySteps) - interval.Halftones;
            var modifier = (int)step.Accidental - halftoneDifference;


            if (strict)
            {
                if (Math.Abs(modifier) <= 2)
                {
                    return new ScaleStep(movedBySteps.Step, (Accidental)modifier);
                }

                throw new InvalidOperationException($"Strict diatonic movement of {step} by {interval} is impossible in scale {scale}");
            }
            else
            {
                if (Math.Abs(modifier) <= 1)
                {
                    return new ScaleStep(movedBySteps.Step, (Accidental)modifier);
                }

                var result = PitchToStep(scale.StepToPitch(step) + interval.Halftones, scale)?.WithOctave(0);
                if (result == null)
                {
                    throw new InvalidOperationException($"Diatonic movement of {step} by {interval} is impossible in scale {scale} even with enharmonics");
                }

                return result;
            }
        }

        public static Chord ModifyChord(Chord chord, int note, int halftoneInterval, MusicalScale scale)
        {
            var n = chord.Notes[note];
            n = scale.ChangeByHalftones(n, halftoneInterval).WithOctave(0);
            return chord.WithChangedNote(note, n);
        }

        public static Chord StackedIntervals(ScaleStep root, MusicalScale scale, bool strict, params ScaleInterval[] intervals)
        {
            var notes = new List<ScaleStep>();
            notes.Add(root);
            var current = root;
            foreach (var interval in intervals)
            {
                current = DiatonicMove(current, interval, scale, strict);
                notes.Add(current);
            }

            return new Chord(notes.ToArray());
        }

        public static Chord? StackedIntervalsMinimumAccidentals(int halftoneRoot, MusicalScale scale, params ScaleInterval[] intervals)
        {
            var root = PitchToStep(halftoneRoot, scale, NoteIdentificationMode.ClosestThenSharp);

            if (root == null)
            {
                return null;
            }

            if (root.Accidental == Accidental.None)
            {
                return StackedIntervals(root, scale, strict: false, intervals);
            }

            var sharpRoot = PitchToStep(halftoneRoot, scale, NoteIdentificationMode.SharpOnly);
            var flatRoot = PitchToStep(halftoneRoot, scale, NoteIdentificationMode.FlatOnly);

            if (sharpRoot == null)
            {
                if (flatRoot == null)
                {
                    throw new InvalidOperationException("This case should be impossible");
                }

                return StackedIntervals(flatRoot, scale, strict: false, intervals);
            }
            else
            {
                if (flatRoot == null)
                {
                    return StackedIntervals(sharpRoot, scale, strict: false, intervals);
                }
                else
                {
                    var chord1 = StackedIntervals(flatRoot, scale, strict: false, intervals);
                    var chord2 = StackedIntervals(sharpRoot, scale, strict: false, intervals);

                    var d = chord1.Notes.Sum(n => Math.Abs((int)n.Accidental)) - chord2.Notes.Sum(n => Math.Abs((int)n.Accidental));

                    return d < 0 ? 
                        chord1 : 
                        chord2;
                }
            }
        }

        public static ScaleStep NextToneAbove(Chord chord, MusicalScale scale, ScaleStep from)
        {
            bool IsBelow(ScaleStep note)
            {
                return (note.Step < from.Step) ||
                    (note.Step == from.Step && note.Accidental <= from.Accidental);
            }

            var result = chord.Notes
                .Select(n => IsBelow(n) ? n.WithOctave(from.Octave + 1) : n.WithOctave(from.Octave))
                .OrderBy(n => scale.StepToPitch(n))
                .First();

            return result;
        }

        public static ScaleStep NextToneBelow(Chord chord, MusicalScale scale, ScaleStep from)
        {
            bool IsAbove(ScaleStep note)
            {
                return (note.Step > from.Step) ||
                    (note.Step == from.Step && note.Accidental >= from.Accidental);
            }

            var result = chord.Notes
                .Select(n => IsAbove(n) ? n.WithOctave(from.Octave - 1) : n.WithOctave(from.Octave))
                .OrderByDescending(n => scale.StepToPitch(n))
                .First();

            return result;
        }

        public static int TotalAccidentals(Chord chord)
        {
            return chord.Notes.Sum(n => Math.Abs((int)n.Accidental));
        }

        public static bool HasLessAccidentals(Chord a, Chord b)
        {
            return TotalAccidentals(a) < TotalAccidentals(b);
        }

        public static bool IsTertianChordInRootPosition(Chord chord, MusicalScale scale)
        {
            var prev = chord.Notes[0];

            foreach (var n in chord.Notes.Skip(1))
            {
                var interval = scale.NormalizedHalftoneInterval(prev, n);
                if (interval < 3 || interval > 4)
                {
                    return false;
                }
                prev = n;
            }

            return true;
        }

        public static int FindRootInversion(Chord chord, MusicalScale scale)
        {
            for (var i = 0; i < chord.Notes.Count; i++)
            {
                if (IsTertianChordInRootPosition(chord.Inversion(i), scale))
                {
                    return i;
                }
            }

            return -1;
        }

        #region Private methods

        private static ScaleStep? PitchToStepLower(int pitch, MusicalScale scale)
        {
            var step = Enumerable.Range(0, scale.Steps.Count)
                .Where(i => scale.Steps[i] <= pitch)
                .Max();

            var diff = pitch - scale.Steps[step];

            if (diff <= 2)
            {
                return new ScaleStep(step, (Accidental)diff);
            }

            return null;
        }

        private static ScaleStep? PitchToStepUpper(int pitch, MusicalScale scale)
        {
            if (pitch > scale.Steps.Max())
            {
                var d = pitch - 12;
                if (d >= -2)
                {
                    return new ScaleStep(0, (Accidental)d, 1);
                }

                return null;
            }


            var step = Enumerable.Range(0, scale.Steps.Count)
                .Where(i => scale.Steps[i] >= pitch)
                .Min();

            var diff = pitch - scale.Steps[step];

            if (diff >= -2)
            {
                return new ScaleStep(step, (Accidental)diff);
            }

            return null;
        }

        #endregion
    }

    public enum NoteIdentificationMode
    {
        SharpOnly,
        FlatOnly,
        PreferSharp,
        PreferFlat,
        ClosestThenSharp,
        ClosestThanFlat
    }
}
