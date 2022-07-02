using MusicCore;

namespace Composer
{
    public class SimpleBasslineMaker : BassMakerBase, IBasslineMaker
    {
        protected override void FillBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int topOfStaff, int octaveOffset)
        {
            var bass = GetChordTone(chord, 0, topOfStaff, octaveOffset);
            FillWithNote(result, measure, bass, beats, topOfStaff, octaveOffset);
        }
    }
}
