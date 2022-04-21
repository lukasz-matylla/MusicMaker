using MusicCore;

namespace Composer
{
    public class RhythmicBassMaker : SimpleBasslineMaker, IBasslineMaker
    {
        protected override int Cutoff => 2;

        protected override void FillBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int octaveWrapThreshold, bool octaveDown)
        {
            for (var i = 0; i < result.Meter.Top; i++)
            {
                var start = i * result.Meter.BeatLength;

                if (IsStrongBeat(beats, start))
                {
                    var bass = GetChordTone(chord, 0, octaveWrapThreshold, octaveDown);
                    result.AddNote(measure, new Note(bass, result.Meter.BeatLength, start));
                }
                else
                {
                    for (var j = 1; j < chord.Notes.Count; j++)
                    {
                        var pitch = GetChordTone(chord, j, octaveWrapThreshold, octaveDown);
                        result.AddNote(measure, new Note(pitch, result.Meter.BeatLength, start));
                    }                    
                }
            }
        }

        protected override void FillLastBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int octaveWrapThreshold, bool octaveDown)
        {
            for (var i = 0; i < chord.Notes.Count; i++)
            {
                var pitch = GetChordTone(chord, i, octaveWrapThreshold, octaveDown);
                FillWithNote(result, measure, pitch, beats, octaveWrapThreshold, octaveDown);
            }           
        }
    }
}
