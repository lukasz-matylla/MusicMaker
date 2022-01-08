using MusicCore;
using Tools;

namespace Composer
{
    public class AlbertiBassMaker
    {
        public int Bottom { get; }

        public AlbertiBassMaker(int bottom = -13)
        {
            Bottom = bottom;
        }

        public Staff GenerateBass(Chord[] chords,
            int tonic = 60,
            MusicalScale? scale = null,
            Meter? meter = null,
            int tempo = 120,
            int measuresCount = -1,
            int notesPerBeat = 1,
            AlbertiPattern pattern = AlbertiPattern.Down,
            int forcedLength = -1)
        {
            scale ??= MusicalScale.Major;
            meter ??= Meter.CC;

            if (measuresCount <= 0)
            {
                measuresCount = chords.Length;
            }

            var octaveOffset = Bottom / scale.Count;
            var bassWrap = Bottom - (octaveOffset - 1) * scale.Count;

            var result = new Staff(tonic, scale, meter, tempo, measuresCount);

            var noteLength = meter.BeatLength / notesPerBeat;
            var noteCount = meter.MeasureLength / noteLength;

            for (var measure = 0; measure < measuresCount; measure++)
            {
                var chordIndex = measure % chords.Length;
                var chord = chords[chordIndex];

                if (chordIndex == chords.Length - 1)
                {
                    var pitch = chord.Notes[0];
                    result.AddNote(new Note(new ScaleStep(pitch.Step, pitch.Accidental, octaveOffset), meter.MeasureLength, 0), measure);
                    continue;
                }

                for (var i = 0; i < noteCount; i++)
                {
                    var pitch = GetPitch(chord, pattern, i, forcedLength, octaveOffset, bassWrap);
                    result.AddNote(new Note(pitch, noteLength, i * noteLength), measure);
                }
            }

            return result;
        }

        private ScaleStep GetPitch(Chord chord, AlbertiPattern pattern, int i, int forcedLength, int octaveOffset, int bassWrap)
        {
            var count = forcedLength > 0 ?
                forcedLength :
                chord.Notes.Count;

            switch (pattern)
            {
                case AlbertiPattern.Down:
                default:
                    i = (count - i).WrapTo(count);
                    break;
                case AlbertiPattern.Up:
                    i %= count;
                    break;
                case AlbertiPattern.UpDown:
                    i %= 2 * count - 2;
                    if (i >= count)
                    {
                        i = 2 * count - 2 - i;
                    }
                    break;
                case AlbertiPattern.ZigZag:
                    if (i % 2 == 0)
                    {
                        i = 0;
                    }
                    else
                    {
                        i /= 2;
                        i %= count - 1;
                        i = count - i - 1;
                    }
                    break;
            }

            var octave = octaveOffset;
            if (chord.Notes[0].Step > bassWrap)
            {
                octave--;
            }

            if (i >= chord.Notes.Count)
            {
                i %= chord.Notes.Count;
                octave++;
            }

            var chordNote = chord.Notes[i];
            var bassNote = chord.Notes[0];
            
            if (chordNote.Step < bassNote.Step)
            {
                octave++;
            }

            return new ScaleStep(chordNote.Step, chordNote.Accidental, octave);
        }
    }

    public enum AlbertiPattern
    {
        Down,
        Up,
        UpDown,
        ZigZag
    }
}
