using MusicCore;
using Tools;

namespace Composer
{
    public class ArpeggioBasslineMaker: BassMakerBase, IBasslineMaker
    {
        protected override int Cutoff => 2;

        protected readonly int[] standarPattern = new[] { 0, 2, 1, 2 };
        protected readonly int[] standarPatternSkip5 = new[] { 0, 3, 1, 3 };

        public ArpeggioPattern Pattern { get; }
        public int NotesPerBeat { get; }

        public ArpeggioBasslineMaker(int notesPerBeat = 1,
            ArpeggioPattern pattern = ArpeggioPattern.Down)
        {
            NotesPerBeat = notesPerBeat;
            Pattern = pattern;
        }

        protected override void FillBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int topOfStaff, int octaveOffset)
        {
            var noteLength = result.Meter.BeatLength / NotesPerBeat;
            var noteCount = result.Meter.MeasureLength / noteLength;

            for (var i = 0; i < noteCount; i++)
            {
                var pitch = GetPatternPitch(chord, Pattern, i, noteCount, topOfStaff, octaveOffset);
                result.AddNote(measure, new Note(pitch, noteLength, i * noteLength));
            }
        }

        protected ScaleStep GetPatternPitch(Chord chord, ArpeggioPattern pattern, int noteIndex, int noteCount, int topOfStaff, int octaveOffset)
        {
            var chordSize = chord.Notes.Count;

            switch (pattern)
            {
                case ArpeggioPattern.Alberti:
                    noteIndex %= 4;
                    if (chordSize >= 4)
                    {
                        noteIndex = standarPatternSkip5[noteIndex];
                    }
                    else if (chord.Notes.Count == 3)
                    {
                        noteIndex = standarPattern[noteIndex];
                    }
                    else
                    {
                        noteIndex %= chordSize;
                    }
                    break;

                case ArpeggioPattern.Down:
                default:
                    if (noteCount % chordSize == 0)
                    {
                        noteIndex = (chordSize - noteIndex).WrapTo(chordSize);
                    }
                    else if (noteCount % (chordSize + 1) == 0)
                    {
                        noteIndex = (chordSize - noteIndex).WrapTo(chordSize + 1);
                    }
                    else
                    {
                        noteIndex = (chordSize + 1 - noteIndex).WrapTo(chordSize + 2);
                        if (noteIndex > chordSize)
                        {
                            noteIndex = 0;
                        }
                    }
                    break;

                case ArpeggioPattern.Up:
                    if (noteCount % (chordSize + 1) == 0)
                    {
                        noteIndex %= chordSize + 1;
                    }
                    else
                    {
                        noteIndex %= chordSize;
                    }
                    break;

                case ArpeggioPattern.UpDown:
                    var len = GetSubdivision(noteCount, 8);
                    noteIndex %= len;
                    if (noteIndex >= len/2 + 1)
                    {
                        noteIndex = len - noteIndex;
                    }
                    break;
            }

            return GetChordTone(chord, noteIndex, topOfStaff, octaveOffset);
        }

        protected int GetSubdivision(int n, int max)
        {
            for (var i = 1; ; i++)
            {
                if (n / i <= max)
                {
                    return n / i;
                }
            }
        }
    } 

    public enum ArpeggioPattern
    {
        Alberti,
        Down,
        Up,
        UpDown,
    }
}
