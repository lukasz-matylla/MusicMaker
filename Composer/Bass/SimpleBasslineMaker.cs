using MusicCore;

namespace Composer
{
    public class SimpleBasslineMaker : BassMakerBase, IBasslineMaker
    {
        protected override void FillBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int octaveWrapThreshold, bool octaveDown)
        {
            FillWithBassNote(result, measure, chord, beats, octaveWrapThreshold, octaveDown);
        }
    }
}
