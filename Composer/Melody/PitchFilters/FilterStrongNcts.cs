using MusicCore;

namespace Composer.Melody.PitchFilters
{
    public class FilterStrongNcts : PitchFilterBase
    {
        private double suspension;
        private double retardation;
        private double appoggiatura;
        private int minLeap;
        private int maxLeap;

        public FilterStrongNcts(double suspension = 1.0, double retardation = 0.0, double appoggiatura = 1.0, int minLeap = 3, int maxLeap = 7)
            : base(0)
        { 
            this.suspension = suspension; 
            this.retardation = retardation;
            this.appoggiatura = appoggiatura;

            this.minLeap = minLeap;
            this.maxLeap = maxLeap;
        }

        protected override double GetWeight(
            ScaleStep thisNote, 
            Chord chord,
            Chord? nextChord,
            ScaleStep? previousNote, 
            ScaleStep? nextNote, 
            bool nextIsStrong, 
            int measure, 
            int startTime, 
            int endTime)
        {
            // chord tones
            if (chord.Notes.Any(n => n.Step == thisNote.Step))
            {
                return 1.0;
            }

            // suspension - keep previous note, resolve down on next beat
            if (thisNote.Equals(previousNote) && nextChord != null && nextChord.Equals(chord) 
                && nextChord.Notes.Select(n => Scale.NormalizedHalftoneInterval(n, thisNote)).Any(i => i == 1 || i == 2))
            {
                return suspension;
            }

            // retardation - keep previous note, resolve up on next beat
            if (thisNote.Equals(previousNote) && nextChord != null && nextChord.Equals(chord)
                && nextChord.Notes.Select(n => Scale.NormalizedHalftoneInterval(thisNote, n)).Any(i => i == 1 || i == 2))
            {
                return retardation;
            }

            // appoggiatura - leap up, resolve down on next beat
            if (previousNote != null && nextChord != null && nextChord.Equals(chord)
                && Scale.NormalizedHalftoneInterval(previousNote, thisNote) >= minLeap && Scale.NormalizedHalftoneInterval(previousNote, thisNote) <= maxLeap
                && nextChord.Notes.Select(n => Scale.NormalizedHalftoneInterval(n, thisNote)).Any(i => i == 1 || i == 2))
            {
                return appoggiatura;
            }

            return 0;
        }
    }
}
