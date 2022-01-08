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
                measuresCount = Math.Min(chords.Length, rhythm.MeasureCount);
            }

            var result = new Staff(tonic, scale, rhythm.Meter, tempo, measuresCount);

            var octaveOffset = Bottom / scale.Count;
            var bassWrap = Bottom - (octaveOffset - 1) * scale.Count;

            for (var measure = 0; measure < measuresCount; measure++)
            {
                var chord = chords[measure % chords.Length];
                var bass = GetBassNote(chord, octaveOffset, bassWrap);

                var beats = rhythm.Measures[measure % rhythm.MeasureCount];

                foreach (var beat in beats.Where(n => n.Pitch.Step == 0))
                {
                    var length = TimeToStrongBeat(beats, beat.StartTime, result.MeasureLength);
                    result.AddNote(new Note(bass, length, beat.StartTime), measure);
                }
            }

            return result;
        }

        private ScaleStep GetBassNote(Chord chord, int octaveOffset, int bassWrap)
        {            
            var chordBass = chord.Notes[0];

            if (chord.Notes[0].Step <= bassWrap)
            {
                return new ScaleStep(chordBass.Step, chordBass.Accidental, octaveOffset);
            }

            return new ScaleStep(chordBass.Step, chordBass.Accidental, octaveOffset - 1);
        }

        private int TimeToStrongBeat(IReadOnlyList<Note> rhythm, int t, int measureLength)
        {
            return rhythm.FirstOrDefault(n => n.StartTime > t && n.Pitch.Step == 0)?.StartTime
                ?? (measureLength - t);
        }
    }
}
