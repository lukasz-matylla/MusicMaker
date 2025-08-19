using MusicCore;

namespace Composer.Melody.PitchFilters
{
    public class FilterForbiddenTensions : PitchFilterBase
    {
        private readonly double softCutoff;

        public FilterForbiddenTensions(double cutoff = 0.0, double softCutoff = 0.1)
            : base(cutoff)
        {
            this.softCutoff = softCutoff;
        }

        protected override double GetWeight(ScaleStep thisNote, 
            Chord chord, 
            ScaleStep? previousNote, 
            ScaleStep? nextNote, 
            bool nextIsStrong, 
            int measure, 
            int startTime, 
            int endTime)
        {
            if (chord.Notes.Any(n => n.Step == thisNote.Step))
            {
                return 1.0; // Chord tones are always allowed
            }

            if (previousNote != null && nextNote != null)
            {
                var beforeInterval = Scale.StepInterval(previousNote, thisNote);
                var afterInterval = Scale.StepInterval(thisNote, nextNote);

                if (Math.Abs(beforeInterval) <= 1 && Math.Abs(afterInterval) <= 1)
                {
                    return 1.0;
                }
            }

            if (chord.Notes.Any(n => Scale.NormalizedHalftoneInterval(n, thisNote) == 1) &&
                (nextNote == null || !nextIsStrong || Scale.StepInterval(thisNote, nextNote) != -1))
            {
                return Cutoff;
            }

            if (!chord.Notes.Any(n => Scale.NormalizedHalftoneInterval(n, thisNote) == 0)
                && chord.Notes.Any(n => Scale.NormalizedHalftoneInterval(n, thisNote) == 6))
            {
                return softCutoff;
            }

            return 1.0;
        }
    }
}
