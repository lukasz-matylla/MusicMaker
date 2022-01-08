using Tools;

namespace MusicCore
{
    public class MusicalScale : IEquatable<MusicalScale>
    {
        public IReadOnlyList<int> Steps { get; }

        public int Count => Steps.Count;

        public MusicalScale(params int[] steps)
        {
            Steps = steps;
        }

        #region Intervals

        public int StepToPitch(ScaleStep step)
        {
            return Steps[step.Step] + (int)step.Accidental + 12 * step.Octave;
        }

        public int HalftoneIterval(ScaleStep from, ScaleStep to)
        {
            return StepToPitch(to) - StepToPitch(from);
        }

        public int NormalizedHalftoneInterval(ScaleStep from, ScaleStep to)
        {
            return HalftoneIterval(from, to).WrapTo(12);
        }

        public int MinimumHalftoneDistance(ScaleStep from, ScaleStep to)
        {
            var interval = NormalizedHalftoneInterval(from, to);

            return Math.Min(interval, 12 - interval);
        }

        public int NoteInterval(ScaleStep from, ScaleStep to)
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

        public static readonly MusicalScale Major = new MusicalScale(0, 2, 4, 5, 7, 9, 11);
        public static readonly MusicalScale Minor = new MusicalScale(0, 2, 3, 5, 7, 8, 10);

        #endregion
    }
}
