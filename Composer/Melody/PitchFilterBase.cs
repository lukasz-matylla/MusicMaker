using MusicCore;

namespace Composer.Melody
{
    public abstract class PitchFilterBase : IPitchFilter
    {
        protected ScaleStep[] AvailableNotes { get; private set; }
        protected MusicalScale Scale { get; private set; }
        protected Key Key { get; private set; }
        protected Clef Clef { get; private set; }
        protected double Cutoff { get; }

        protected PitchFilterBase(double cutoff)
        {
            AvailableNotes = new ScaleStep[0];
            Scale = MusicalScale.Major;

            Cutoff = cutoff;
        }

        public virtual void Setup(ScaleStep[] availableNotes, MusicalScale scale, Key key, Clef clef)
        {
            AvailableNotes = availableNotes;
            Scale = scale;
            Key = key;
            Clef = clef;
        }

        public virtual double[] GetWeights(Chord chord, 
            ScaleStep? previousNote, 
            ScaleStep? nextNote, 
            bool nextIsStrong, 
            int measure, 
            int startTime, 
            int endTime)
        {
            return AvailableNotes
                .Select(note => GetWeight(note, chord, previousNote, nextNote, nextIsStrong, measure, startTime, endTime))
                .ToArray();
        }

        protected abstract double GetWeight(ScaleStep thisNote,
            Chord chord,
            ScaleStep? previousNote,
            ScaleStep? nextNote,
            bool nextIsStrong,
            int measure,
            int startTime,
            int endTime);
    }
}
