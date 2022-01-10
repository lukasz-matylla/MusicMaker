using MusicCore;

namespace Composer
{
    public class WalkingBassMaker : SimpleBasslineMaker
    {
        public WalkingBassMaker(int bottom = -13)
            : base(bottom)
        { }

        protected override void FillBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int octaveOffset, int bassWrap)
        {
            for (var i = 0; i < result.Meter.Top; i++)
            {
                var start = i * result.Meter.BeatLength;

                if (IsStrongBeat(beats, start))
                {
                    var bass = GetChordTone(chord, 0, octaveOffset, bassWrap);
                    result.AddNote(new Note(bass, result.Meter.BeatLength, start), measure);
                }
                else
                {
                    for (var j = 1; j < chord.Notes.Count; j++)
                    {
                        var pitch = GetChordTone(chord, j, octaveOffset, bassWrap);
                        result.AddNote(new Note(pitch, result.Meter.BeatLength, start), measure);
                    }                    
                }
            }
        }
    }
}
