using MusicCore;

namespace Composer
{
    public class SimpleBasslineMaker
    {
        protected virtual int Cutoff => 6; 

        public Staff GenerateBass(
            Chord[] chords,
            Staff rhythm,
            Key key = Key.C,
            Clef clef = Clef.Bass,
            MusicalScale? scale = null,
            int tempo = 120,
            int measuresCount = -1)
        {
            scale ??= MusicalScale.Major;

            if (measuresCount <= 0)
            {
                measuresCount = chords.Length;
            }

            var wrapAbove = CalculateTopStaffTone(scale, key);

            var result = new Staff(clef, key, scale, rhythm.Meter, tempo, measuresCount);

            for (var measure = 0; measure < measuresCount; measure++)
            {
                var chord = chords[measure % chords.Length];
                var beats = rhythm[measure % rhythm.MeasureCount];

                if (measure % chords.Length == chords.Length - 1)
                {
                    FillLastBar(result, measure, chord, beats, wrapAbove);
                }
                else
                {
                    FillBar(result, measure, chord, beats, wrapAbove);
                }                
            }

            return result;
        }

        private int CalculateTopStaffTone(MusicalScale scale, Key key)
        {
            return Enumerable.Range(0, scale.Count).Where(i => scale[i] + (int)key <= Cutoff).Last();
        }

        protected virtual void FillLastBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int wrapAbove)
        {
            var bass = GetChordTone(chord, 0, wrapAbove);

            result.AddNote(new Note(bass, result.MeasureLength, 0), measure);
        }

        protected virtual void FillBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int wrapAbove)
        {
            var bass = GetChordTone(chord, 0, wrapAbove);

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

        protected ScaleStep GetChordTone(Chord chord, int index, int wrapAbove)
        {
            var chordBass = chord.Notes[0];
            var chordNote = chord.Notes[index];
            var octave = 0;

            if (chordBass.Step > wrapAbove)
            {
                octave--;
            }

            if (chordNote.Step < chordBass.Step)
            {
                octave++;
            }

            return new ScaleStep(chordNote.Step, chordNote.Accidental, octave);
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
