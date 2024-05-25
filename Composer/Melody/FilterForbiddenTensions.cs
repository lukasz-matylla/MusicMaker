using MusicCore;

namespace Composer.Melody
{
    public class FilterForbiddenTensions : PitchFilterBase
    {
        public FilterForbiddenTensions(double cutoff = 0.2)
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
            if (chord.Notes.Any(n => Scale.NormalizedHalftoneInterval(n, thisNote) == 1 || Scale.NormalizedHalftoneInterval(thisNote, n) == 1))
            {
                return Cutoff;
            }

            return 1.0;
        }
    }
}
