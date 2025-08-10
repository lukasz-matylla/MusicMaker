using MusicCore;

namespace Composer.Melody.PitchFilters
{
    public interface IPitchFilter
    {
        void Setup(ScaleStep[] availableNotes, MusicalScale scale, Key key, Clef clef);
        double[] GetWeights(Chord chord, ScaleStep? previousNote, ScaleStep? nextNote, bool nextIsStrong, int measure, int startTime, int endTime);
    }
}
