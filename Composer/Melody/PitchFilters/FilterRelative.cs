using MusicCore;
namespace Composer.Melody.PitchFilters
{
    public class FilterRelative : PitchFilterBase
    {
        private readonly Staff otherPart;
        private readonly bool above;
        private readonly bool allowEqual;

        public FilterRelative(Staff otherPart, bool above = false, bool allowEqual = true)
            : base(0.0)
        {
            this.otherPart = otherPart;
            this.above = above;
            this.allowEqual = allowEqual;
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
            var thisPitch = Scale.StepToPitch(thisNote) + (int)Key + 12*(int)Clef;

            var otherPitches = otherPart
                .NotesDuring(measure, startTime, endTime)
                .Select(n => otherPart.Scale.StepToPitch(n.Pitch) + (int)otherPart.Key + 12*(int)otherPart.Clef);
            
            if (above)
            {
                var threshold = otherPitches.Max();

                if (thisPitch > threshold ||
                    thisPitch == threshold && allowEqual)
                {
                    return 1.0;
                }
            }
            else
            {
                var threshold = otherPitches.Min();

                if (thisPitch < threshold ||
                    thisPitch == threshold && allowEqual)
                {
                    return 1.0;
                }
            }

            return Cutoff;
        }
    }
}
