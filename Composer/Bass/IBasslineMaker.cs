using MusicCore;

namespace Composer
{
    public interface IBasslineMaker
    {
        Staff GenerateBass(
           Chord[] chords,
           Staff rhythm,
           Key key = Key.C,
           Clef clef = Clef.Bass,
           MusicalScale? scale = null,
           int tempo = 120,
           int measuresCount = -1);
    }
}
