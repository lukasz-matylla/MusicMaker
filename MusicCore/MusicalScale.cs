using Tools;

namespace MusicCore
{
    public class MusicalScale : IEquatable<MusicalScale>
    {
        public const int HalftonesInOctave = 12;

        public IReadOnlyList<int> Steps { get; }

        public int Count => Steps.Count;

        public int this[int n] => Steps[n];

        public MusicalScale(params int[] steps)
        {
            Steps = (new[] { 0 }).Concat(steps.OrderBy(i => i)).ToArray();
        }

        #region Intervals

        public int StepToPitch(ScaleStep step)
        {
            return Steps[step.Step] + (int)step.Accidental + HalftonesInOctave * step.Octave;
        }

        public int HalftoneInterval(ScaleStep from, ScaleStep to)
        {
            return StepToPitch(to) - StepToPitch(from);
        }

        public int NormalizedHalftoneInterval(ScaleStep from, ScaleStep to)
        {
            return HalftoneInterval(from, to).WrapTo(HalftonesInOctave);
        }

        public int MinimumHalftoneDistance(ScaleStep from, ScaleStep to)
        {
            var interval = NormalizedHalftoneInterval(from, to);

            return Math.Min(interval, HalftonesInOctave - interval);
        }

        public int StepInterval(ScaleStep from, ScaleStep to)
        {
            return to.Step - from.Step + Count * (to.Octave - from.Octave);
        }

        public int NormalizeNoteInterval(int interval)
        {
            return interval.WrapTo(Count);
        }

        #endregion

        #region NoteOperations

        public ScaleStep ChangeBySteps(ScaleStep pitch, int offset)
        {
            var targetStep = pitch.Step + offset;
            var octaveOffset = targetStep >= 0 ?
                targetStep / Count :
                targetStep / Count - 1;
            targetStep = NormalizeNoteInterval(targetStep);
            return new ScaleStep(targetStep, pitch.Accidental, pitch.Octave + octaveOffset);
        }

        public ScaleStep StepUp(ScaleStep pitch)
        {
            return ChangeBySteps(pitch, 1);
        }

        public ScaleStep StepDown(ScaleStep pitch)
        {
            return ChangeBySteps(pitch, -1);
        }

        public ScaleStep ChangeByHalftones(ScaleStep pitch, int offset)
        {
            var targetPitch = StepToPitch(pitch) + offset;
            var targetNormalized = targetPitch % HalftonesInOctave;

            // If it is a scale step with no accidentals, it takes preference
            if (Steps.Contains(targetNormalized))
            {
                var step = Enumerable.Range(0, Count).Single(i => Steps[i] == targetPitch % HalftonesInOctave);
                var octaveOffset = targetPitch > 0 ?
                    targetPitch / 12 :
                    targetPitch / 12 - 1;
                return new ScaleStep(step, Accidental.None, octaveOffset);
            }

            // Prefer to modify current tone with an accidental
            if (Math.Abs((int)pitch.Accidental + offset) < 2)
            {
                return pitch.WithAccidental((Accidental)((int)pitch.Accidental + offset));
            }

            var closestStep = Enumerable.Range(0, Count)
                .OrderBy(i => Math.Abs(Steps[i] - targetNormalized))
                .First();
            var accidental = targetNormalized - Steps[closestStep];
            if ((HalftonesInOctave - targetNormalized) < Math.Abs(Steps[closestStep] - targetNormalized))
            {
                closestStep = 0;
                accidental = targetNormalized - HalftonesInOctave;
            }

            var stepPitch = Steps[closestStep] + accidental;
            var remainder = targetPitch - stepPitch;
            var octave = remainder > 0 ?
                remainder / HalftonesInOctave :
                remainder / HalftonesInOctave - 1;
            return new ScaleStep(closestStep, (Accidental)accidental, octave);
        }

        public ScaleStep OctaveUp(ScaleStep pitch)
        {
            return new ScaleStep(pitch.Step, pitch.Accidental, pitch.Octave + 1);
        }

        public ScaleStep OctaveDown(ScaleStep pitch)
        {
            return new ScaleStep(pitch.Step, pitch.Accidental, pitch.Octave - 1);
        }

        public ScaleStep OctaveOffset(ScaleStep pitch, int offset)
        {
            return new ScaleStep(pitch.Step, pitch.Accidental, pitch.Octave + offset);
        }

        #endregion

        public bool Equals(MusicalScale? other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Steps.SequenceEqual(other.Steps);
        }

        #region ReadyScales

        public static readonly MusicalScale Major = new MusicalScale(2, 4, 5, 7, 9, 11);
        public static readonly MusicalScale Minor = new MusicalScale(2, 3, 5, 7, 8, 10);

        #endregion
    }
}
