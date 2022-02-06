using MusicCore;

namespace Composer
{
    public interface IChordProgressionGenerator
    {
        Chord[] GenerateProgression(int length, CadenceType cadence = CadenceType.Strong);
    }
}
