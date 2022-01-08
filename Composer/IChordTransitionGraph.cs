using MusicCore;

namespace Composer
{
    public interface IChordTransitionGraph
    {
        IReadOnlyList<Chord> Chords { get; }
        double[] WeightsFrom(int chord);
        double[] WeightsFrom(Chord chord);
        double[] WeightsTo(int chord);
        double[] WeightsTo(Chord chord);
        MusicalScale Scale { get; }
    }
}
