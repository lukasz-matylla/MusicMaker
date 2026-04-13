using MusicCore;

namespace Composer.Melody.PitchFilters
{
    public class FilterWeakNcts : PitchFilterBase
    {
        public FilterWeakNcts(double cutoff = 0.6) 
            : base(cutoff)
        { }

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
            if (previousNote == null)
            {
                throw new ArgumentNullException(nameof(previousNote));
            }

            if (nextNote == null)
            {
                throw new ArgumentNullException(nameof(nextNote));
            }

            var beforeInterval = Scale.HalftoneInterval(previousNote, thisNote);
            var afterInterval = Scale.HalftoneInterval(thisNote, nextNote);

            var beforeSteps = Scale.StepInterval(previousNote, thisNote);
            var afterSteps = Scale.StepInterval(thisNote, nextNote);

            var isChordTone = chord.Notes.Any(n => n.Step == thisNote.Step);

            // passing note
            if (beforeInterval * afterInterval > 0 && Math.Abs(beforeInterval) <= 2 && Math.Abs(afterInterval) <= 2)
            {
                return 1;
            }

            // neighbor tone
            if (previousNote.Equals(nextNote) && beforeSteps * afterSteps == -1)
            {
                return 1;
            }

            // appoggiatura
            if (beforeInterval * afterInterval < 0 && Math.Abs(afterSteps) == 1)
            {
                return 1;
            }

            // passing chord tone over a skip
            if (beforeInterval * afterInterval > 0 && isChordTone)
            {
                return 1;
            }

            // approach, then surround
            if (!nextIsStrong && Math.Abs(afterSteps) == 1)
            {
                return 1;
            }

            // any chord tone
            if (isChordTone)
            {
                return Math.Max(Math.Pow(Cutoff, Math.Abs(beforeInterval)), Math.Pow(Cutoff, Math.Abs(afterInterval)));
            }

            // escape tone
            if (beforeInterval * afterInterval < 0 && Math.Abs(beforeSteps) == 1)
            {
                return Math.Pow(Cutoff, Math.Abs(afterInterval));
            }

            // approach with a skip
            if (Math.Abs(afterSteps) == 1)
            {
                return Math.Pow(Cutoff, Math.Abs(beforeInterval));
            }

            return 0.0;
        }
    }
}
