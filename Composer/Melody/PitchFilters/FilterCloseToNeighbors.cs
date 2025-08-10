using MusicCore;

namespace Composer.Melody.PitchFilters
{
    public class FilterCloseToNeighbors : PitchFilterBase
    {
        private readonly bool chromatic;

        public FilterCloseToNeighbors(double cutoff = 0.7, bool chromatic = false)
            : base(cutoff)
        {
            this.chromatic = chromatic;
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
            var result = 1.0;

            if (previousNote != null)
            {
                var interval = chromatic ?
                0.5 * Scale.HalftoneInterval(previousNote, thisNote) :
                Scale.StepInterval(previousNote, thisNote);

                result *= Math.Pow(Cutoff, Math.Abs(interval));
            }

            if (nextNote != null)
            {
                var interval = chromatic ?
                0.5 * Scale.HalftoneInterval(thisNote, nextNote) :
                Scale.StepInterval(thisNote, nextNote);

                result *= Math.Pow(Cutoff, Math.Abs(interval));
            }

            return result;
        }
    }
}
