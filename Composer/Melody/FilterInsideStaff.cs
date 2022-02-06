using MusicCore;

namespace Composer.Melody
{
    public class FilterInsideStaff : PitchFilterBase
    {
        private readonly int allowedLedgers;

        private int topPitch;
        private int bottomPitch;
        
        public FilterInsideStaff(int allowedLedgers = 0, double cutoff = 0.5)
            : base(cutoff)
        {
            this.allowedLedgers = allowedLedgers;
        }

        public override void Setup(ScaleStep[] availableNotes, MusicalScale scale, Key key, Clef clef)
        {
            base.Setup(availableNotes, scale, key, clef);

            var clefOffset = clef switch
            {
                Clef.Treble8up => 2,
                Clef.Treble => 2,
                Clef.Alto => 0,
                Clef.Bass => -2,
                Clef.Bass8down => -2,
                _ => throw new ArgumentOutOfRangeException(nameof(clef))
            };

            var top = clefOffset + 5 + 2*allowedLedgers;
            var bottom = clefOffset - 5 - 2*allowedLedgers;

            var C = new ScaleStep(0);
            var major = MusicalScale.Major;

            topPitch = major.StepToPitch(major.ChangeBySteps(C, top));
            bottomPitch = major.StepToPitch(major.ChangeBySteps(C, bottom));
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
            var pitch = Scale.StepToPitch(thisNote) - (int)Key;

            if (pitch > topPitch)
            {
                return Math.Pow(Cutoff, pitch - topPitch);
            }
            if (pitch < bottomPitch)
            {
                return Math.Pow(Cutoff, bottomPitch - pitch);
            }
            return 1.0;
        }
    }
}
