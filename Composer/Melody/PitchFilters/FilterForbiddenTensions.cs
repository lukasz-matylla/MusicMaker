using MusicCore;

namespace Composer.Melody.PitchFilters
{
    public class FilterForbiddenTensions : PitchFilterBase
    {
        public FilterForbiddenTensions(double cutoff = 0.0)
            : base(cutoff)
        { }

        protected override double GetWeight(ScaleStep thisNote, 
            Chord chord, 
            ScaleStep? previousNote, 
            ScaleStep? nextNote, 
            bool nextIsStrong, 
            int measure, 
            int startTime, 
            int endTime)
        {
            if (previousNote != null && nextNote != null)
            {
                var beforeInterval = Scale.StepInterval(previousNote, thisNote);
                var afterInterval = Scale.StepInterval(thisNote, nextNote);

                if (Math.Abs(beforeInterval) <= 1 && Math.Abs(afterInterval) <= 1)
                {
                    return 1.0;
                }
            }

            if (chord.Notes.Any(n => Scale.NormalizedHalftoneInterval(n, thisNote) == 1))
            {
                return Cutoff;
            }

            if (!chord.Notes.Any(n => Scale.NormalizedHalftoneInterval(n, thisNote) == 0)
                && chord.Notes.Any(n => Scale.NormalizedHalftoneInterval(n, thisNote) == 6))
            {
                return Cutoff;
            }

            return 1.0;
        }
    }
}
