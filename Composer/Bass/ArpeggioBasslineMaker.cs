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

        protected override void FillBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int octaveWrapThreshold, bool octaveDown)
        {
            var noteLength = result.Meter.BeatLength / NotesPerBeat;
            var noteCount = result.Meter.MeasureLength / noteLength;

            for (var i = 0; i < noteCount; i++)
            {
                var pitch = GetPatternPitch(chord, Pattern, i, octaveWrapThreshold, octaveDown);
                result.AddNote(measure, new Note(pitch, noteLength, i * noteLength));
            }
        }

        protected ScaleStep GetPatternPitch(Chord chord, ArpeggioPattern pattern, int noteIndex, int octaveWrapThreshold, bool octaveDown)
        {
            var count = chord.Notes.Count;

            switch (pattern)
            {
                case ArpeggioPattern.Alberti:
                    noteIndex %= 4;
                    if (count >= 4)
                    {
                        noteIndex = standarPatternSkip5[noteIndex];
                    }
                    else if (chord.Notes.Count == 3)
                    {
                        noteIndex = standarPattern[noteIndex];
                    }
                    else
                    {
                        noteIndex %= count;
                    }
                    break;

                case ArpeggioPattern.Down:
                default:
                    noteIndex = (4 - noteIndex).WrapTo(4);
                    break;

                case ArpeggioPattern.Up:
                    noteIndex %= count;
                    break;

                case ArpeggioPattern.UpDown:
                    noteIndex %= 6;
                    if (noteIndex >= 4)
                    {
                        noteIndex = 6 - noteIndex;
                    }
                    break;
            }

            return GetChordTone(chord, noteIndex, octaveWrapThreshold, octaveDown);
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
