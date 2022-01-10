using MusicCore;

namespace Composer
{
    public class SimpleBasslineMaker
    {
        public int Bottom { get; }

        public SimpleBasslineMaker(int bottom = -11)
        {
            Bottom = bottom;
        }

        public Staff GenerateBass(Chord[] chords,
            Staff rhythm,
            int tonic = 60,
            MusicalScale? scale = null,
            int tempo = 120,
            int measuresCount = -1)
        {
            scale ??= MusicalScale.Major;

            if (measuresCount <= 0)
            {
                measuresCount = chords.Length;
            }

            var octaveOffset = Bottom / scale.Count;
            var bassWrap = Bottom - (octaveOffset - 1) * scale.Count;

            var result = new Staff(tonic, scale, rhythm.Meter, tempo, measuresCount);

            for (var measure = 0; measure < measuresCount; measure++)
            {
                var chord = chords[measure % chords.Length];
                var beats = rhythm[measure % rhythm.MeasureCount];

                if (measure % chords.Length == chords.Length - 1)
                {
                    FillLastBar(result, measure, chord, beats, octaveOffset, bassWrap);
                }
                else
                {
                    FillBar(result, measure, chord, beats, octaveOffset, bassWrap);
                }                
            }

            return result;
        }

        protected virtual void FillLastBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int octaveOffset, int bassWrap)
        {
            var bass = GetChordTone(chord, 0, octaveOffset, bassWrap);

            result.AddNote(new Note(bass, result.MeasureLength, 0), measure);
        }

        protected virtual void FillBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int octaveOffset, int bassWrap)
        {
            var bass = GetChordTone(chord, 0, octaveOffset, bassWrap);

            for (var i = 0; i < result.Meter.Top; i++)
            {
                var start = i * result.Meter.BeatLength;

                if (IsStrongBeat(beats, start))
                {
                    var length = TimeToStrongBeat(beats, start, result.MeasureLength);
                    result.AddNote(new Note(bass, length, start), measure);
                }
            }
        }

        protected ScaleStep GetChordTone(Chord chord, int index, int octaveOffset, int bassWrap)
        {
            var chordBass = chord.Notes[0];
            var chordNote = chord.Notes[index];

            if (chordBass.Step > bassWrap)
            {
                octaveOffset--;
            }

            if (chordNote.Step < chordBass.Step)
            {
                octaveOffset++;
            }

            return new ScaleStep(chordNote.Step, chordNote.Accidental, octaveOffset);
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
