using Tools;

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
            var aAsPitches = a.Notes.Select(n => scale.StepToPitch(n));
            var bAsPitches = b.Notes.Select(n => scale.StepToPitch(n));

            return aAsPitches.SequenceEqual(bAsPitches);
        }

        public static ScaleStep DiatonicMove(ScaleStep step, ScaleInterval interval, MusicalScale scale)
        {
            var movedBySteps = scale.ChangeBySteps(step, interval.Steps);

            var halftoneDifference = scale.HalftoneIterval(step, movedBySteps) - interval.Halftones;
            var modifier = (int)step.Accidental - halftoneDifference;

            if (Math.Abs(modifier) <= 2)
            {
                return new ScaleStep(movedBySteps.Step, (Accidental)modifier);
            }

            throw new InvalidOperationException($"Diatonic movement of {step} by {interval} is impossible in scale {scale}");
        }

        public static ScaleStep HalftoneMove(ScaleStep step, int interval, MusicalScale scale)
        {
            var pitch = scale.StepToPitch(step);
            pitch = (pitch + interval).WrapTo(12);
            var newNote = PitchToStep(pitch, scale, NoteIdentificationMode.ClosestThenSharp);

            if (newNote == null)
            {
                throw new InvalidOperationException($"Pitch {pitch} can't be expressed in scale {scale}");
            }
            return newNote;
        }

        public static Chord ModifyChord(Chord chord, int note, int halftoneInterval, MusicalScale scale)
        {
            var n = chord.Notes[note];
            n = HalftoneMove(n, halftoneInterval, scale);
            return chord.WithChangedNote(note, n);
        }

        public static Chord StackedIntervals(ScaleStep root, MusicalScale scale, params ScaleInterval[] intervals)
        {
            var notes = new List<ScaleStep>();
            notes.Add(root);
            var current = root;
            foreach (var interval in intervals)
            {
                current = DiatonicMove(current, interval, scale);
                notes.Add(current);
            }

            return new Chord(notes.ToArray());
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
