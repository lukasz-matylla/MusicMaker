using MusicCore;

namespace Composer.Melody.PitchFilters
{
    public class FilterLimitSkip  : PitchFilterBase
    {
        public int Size { get; private set; }

        public FilterLimitSkip(int size = 8, double cutoff = 0)
            : base(cutoff)
        {
            Size = size;
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
            if (previousNote != null)
            {
                var interval = Scale.StepInterval(previousNote, thisNote);

                if (Math.Abs(interval) > Size)
                {
                    return Cutoff; // Avoid if the interval is larger than the limit
                }
            }

            if (nextNote != null)
            {
                var interval = Scale.StepInterval(thisNote, nextNote);

                if (Math.Abs(interval) > Size)
                {
                    return Cutoff; // Avoid if the interval is larger than the limit
                }
            }

            return 1.0;
        }
    }
}
