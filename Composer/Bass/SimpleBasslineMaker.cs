using MusicCore;

namespace Composer
{
    public class SimpleBasslineMaker : BassMakerBase, IBasslineMaker
    {
        protected override void FillBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int octaveWrapThreshold, bool octaveDown)
        {
            var bass = GetChordTone(chord, 0, octaveWrapThreshold, octaveDown);
            FillWithNote(result, measure, bass, beats, octaveWrapThreshold, octaveDown);
        }
    }
}
