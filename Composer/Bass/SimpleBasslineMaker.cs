using MusicCore;

namespace Composer
{
    public class SimpleBasslineMaker : BassMakerBase, IBasslineMaker
    {
        protected override void FillBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int wrapAbove)
        {
            var bass = GetChordTone(chord, 0, wrapAbove);

            for (var i = 0; i < result.Meter.Top; i++)
            {
                var start = i * result.Meter.BeatLength;

                if (IsStrongBeat(beats, start))
                {
                    var length = TimeToStrongBeat(beats, start, result.MeasureLength);
                    result.AddNote(measure, new Note(bass, length, start));
                }
            }
        }

        protected bool IsStrongBeat(IReadOnlyList<Note> beats, int t)
        {
            return beats.FirstOrDefault(b => b.StartTime == t)?.Pitch?.Step == 0;
        }

        protected int TimeToStrongBeat(IReadOnlyList<Note> rhythm, int t, int measureLength)
        {
            return rhythm.FirstOrDefault(n => n.StartTime > t && n.Pitch.Step == 0)?.StartTime
                ?? (measureLength - t);
        }  
    }
}
