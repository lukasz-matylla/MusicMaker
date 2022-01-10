using MusicCore;
using Tools;

namespace Composer
{
    public class AlbertiBassMaker: SimpleBasslineMaker
    {
        public AlbertiPattern Pattern { get; }
        public int NotesPerBeat { get; }
        public int ForcedLength { get; }

        public AlbertiBassMaker(int bottom = -13,
            int notesPerBeat = 1,
            AlbertiPattern pattern = AlbertiPattern.Down,
            int forcedLength = -1)
            : base(bottom)
        {
            NotesPerBeat = notesPerBeat;
            Pattern = pattern;
            ForcedLength = forcedLength;
        }

        protected override void FillBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int octaveOffset, int bassWrap)
        {
            var noteLength = result.Meter.BeatLength / NotesPerBeat;
            var noteCount = result.Meter.MeasureLength / noteLength;

            for (var i = 0; i < noteCount; i++)
            {
                var pitch = GetAlbertiPitch(chord, Pattern, i, octaveOffset, bassWrap);
                result.AddNote(new Note(pitch, noteLength, i * noteLength), measure);
            }
        }

        protected ScaleStep GetAlbertiPitch(Chord chord, AlbertiPattern pattern, int noteIndex, int octaveOffset, int bassWrap)
        {
            var count = ForcedLength > 0 ?
                ForcedLength :
                chord.Notes.Count;

            switch (pattern)
            {
                case AlbertiPattern.Down:
                default:
                    noteIndex = (count - noteIndex).WrapTo(count);
                    break;
                case AlbertiPattern.Up:
                    noteIndex %= count;
                    break;
                case AlbertiPattern.UpDown:
                    noteIndex %= 2 * count - 2;
                    if (noteIndex >= count)
                    {
                        noteIndex = 2 * count - 2 - noteIndex;
                    }
                    break;
                case AlbertiPattern.ZigZag:
                    if (noteIndex % 2 == 0)
                    {
                        noteIndex = 0;
                    }
                    else
                    {
                        noteIndex /= 2;
                        noteIndex %= count - 1;
                        noteIndex = count - noteIndex - 1;
                    }
                    break;
            }

            return GetChordTone(chord, noteIndex, octaveOffset, bassWrap);
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
