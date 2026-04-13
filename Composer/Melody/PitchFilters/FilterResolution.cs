using MusicCore;

namespace Composer.Melody.PitchFilters
{
    public class FilterResolution : PitchFilterBase
    {
        private bool strong;

        public FilterResolution(bool strong = false, double cutoff = 0.3)
            : base(cutoff)
        {
            this.strong = strong;
        }

        protected override double GetWeight(ScaleStep thisNote, Chord chord, Chord? nextChord, ScaleStep? previousNote, ScaleStep? nextNote, 
            bool nextIsStrong, int measure, int startTime, int endTime)
        {
            if (previousNote == null)
            {
                return 1;
            }

            var previousIsChordTone = chord.Notes.Any(n => n.Step == previousNote.Step);
            var thisIsChordTone = chord.Notes.Any(n => n.Step == thisNote.Step);
            var stepsBefore = Scale.StepInterval(previousNote, thisNote);

            // nothing to resolve
            if (previousIsChordTone)
            {
                return 1;
            }

            // beginning of new chord - a strong NCT may be here
            if (startTime == 0)
            {
                return 1;
            }

            // next note resolves
            if (!strong && nextIsStrong && nextNote != null && chord.Notes.Any(n => n.Step == nextNote.Step && n.Accidental == nextNote.Accidental)
                && Scale.StepInterval(previousNote, nextNote) == 1)
            {
                // suspension
                if (thisNote.Equals(previousNote))
                {
                    return 1;
                }

                // surround
                var stepsAfter = Scale.StepInterval(thisNote, nextNote);
                if (stepsBefore * stepsAfter < 0 && Math.Abs(stepsAfter) == 1)
                {
                    return 1;
                }
            }

            // resolve NCT by step down to a chord tone
            if (thisIsChordTone && Math.Abs(stepsBefore) == -1)
            {
                return 1;
            }

            // resolve NCT by step up to a chord tone
            if (thisIsChordTone && Math.Abs(stepsBefore) == 1)
            {
                return Cutoff;
            }

            return 0;
        }
    }
}
