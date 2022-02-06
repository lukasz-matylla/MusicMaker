using MusicCore;

namespace Composer.Melody
{
    public class FilterSmoothTransition : PitchFilterBase
    {
        private double DistantCutoff { get; }

        public FilterSmoothTransition(double cutoff = 0.6, double distantCutoff = 0.8) 
            : base(cutoff)
        {
            DistantCutoff = distantCutoff;
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
            if (previousNote == null)
            {
                throw new ArgumentNullException(nameof(previousNote));
            }

            if (nextNote == null)
            {
                var interval = Scale.HalftoneIterval(previousNote, thisNote);
                return Math.Pow(Cutoff, Math.Abs(interval));
            }

            var beforeInterval = Scale.HalftoneIterval(previousNote, thisNote);
            var afterInterval = Scale.HalftoneIterval(thisNote, nextNote);

            if (!nextIsStrong)
            {
                return Math.Pow(Cutoff, Math.Abs(beforeInterval)) * Math.Pow(DistantCutoff, Math.Abs(afterInterval));
            }

            if (Math.Abs(Scale.NoteInterval(previousNote, nextNote)) == 0 &&
                Math.Abs(Scale.NoteInterval(thisNote, nextNote)) == 1)
            {
                return 1.0;
            }

            if (Math.Abs(Scale.NoteInterval(previousNote, nextNote)) == 1 &&
                Math.Abs(Scale.NoteInterval(thisNote, nextNote)) == 1)
            {
                return 1.0;
            }

            if (beforeInterval * afterInterval > 0)
            {
                return Math.Max(Math.Pow(Cutoff, Math.Abs(beforeInterval)), Math.Pow(Cutoff, Math.Abs(afterInterval)));
            }

            return 0.0;
        }
    }
}
