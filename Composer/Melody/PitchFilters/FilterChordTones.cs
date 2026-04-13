using MusicCore;

namespace Composer.Melody.PitchFilters
{
    public class FilterChordTones : PitchFilterBase
    {
        public FilterChordTones(double cutoff = 0.0)
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
            if (chord.Notes.Any(n => n.Step == thisNote.Step))
            {
                return 1.0;
            }

            return Cutoff;
        }
    }
}
