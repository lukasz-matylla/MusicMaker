using MusicCore;

namespace Composer.ChordProgression
{
    public interface IChordTransitionGraph : IFilteredTransitionGraph<Chord>
    {
        MusicalScale Scale { get; }
    }
}
