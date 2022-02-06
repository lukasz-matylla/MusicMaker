using MusicCore;

namespace Composer.Melody
{
    public class FilterAvoidRepeats : PitchFilterBase
    {
        public FilterAvoidRepeats(double cutoff = 0.2) 
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
            var result = 1.0;

            if (previousNote != null && previousNote.Equals(thisNote))
            {
                result *= Cutoff;
            }

            if (nextNote != null && nextNote.Equals(thisNote))
            {
                result *= Cutoff;
            }

            return result;
        }
    }
}
