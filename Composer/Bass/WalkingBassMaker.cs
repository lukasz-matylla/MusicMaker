using MusicCore;

namespace Composer
{
    public class WalkingBassMaker : SimpleBasslineMaker
    {
        protected override int Cutoff => 2;

        protected override void FillBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int wrapAbove)
        {
            for (var i = 0; i < result.Meter.Top; i++)
            {
                var start = i * result.Meter.BeatLength;

                if (IsStrongBeat(beats, start))
                {
                    var bass = GetChordTone(chord, 0, wrapAbove);
                    result.AddNote(measure, new Note(bass, result.Meter.BeatLength, start));
                }
                else
                {
                    for (var j = 1; j < chord.Notes.Count; j++)
                    {
                        var pitch = GetChordTone(chord, j, wrapAbove);
                        result.AddNote(measure, new Note(pitch, result.Meter.BeatLength, start));
                    }                    
                }
            }
        }
    }
}
