using MusicCore;

namespace Composer.Melody.PitchFilters
{
    public class FilterAvoidRepeats : PitchFilterBase
    {
        public double CutoffBeforeStrong { get; }

        public FilterAvoidRepeats(double cutoff = 0.7, double cutoffBeforeStrong = 0.05)
            : base(cutoff)
        {
            CutoffBeforeStrong = cutoffBeforeStrong;
        }

        protected override double GetWeight(ScaleStep thisNote, 
            Chord chord, 
            Chord? nextChord,
            ScaleStep? previousNote, 
            ScaleStep? nextNote, 
            bool nextIsStrong, 
            int measure, 
            int startTime, 
            int endTime)
        {
            var result = 1.0;

            if (previousNote != null && previousNote.Equals(thisNote))
            {
                result *= Cutoff;
            }

            if (nextNote != null && nextNote.Equals(thisNote))
            {
                if (nextIsStrong)
                {
                    result *= CutoffBeforeStrong;
                }
                else
                {
                    result *= Cutoff;
                }
            }

            return result;
        }
    }
}
