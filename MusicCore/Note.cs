using Tools;

namespace MusicCore
{
    public class Note : IEquatable<Note>
    {
        public ScaleStep Pitch { get; }
        public int Length { get; }
        public int StartTime { get; }
        public int EndTime => StartTime + Length;

        public Note(ScaleStep pitch, int length, int start = 0)
        {
            Pitch = pitch;
            Length = length;
            StartTime = start;
        }

        #region Modifiers

        public Note AtPitch(ScaleStep newPitch)
        {
            return new Note(newPitch, Length, StartTime);
        }

        public Note WithLength(int newLength)
        {
            return new Note(Pitch, newLength, StartTime);
        }

        public Note WithStart(int newStart)
        {
            return new Note(Pitch, Length, newStart);
        }

        public Note Double()
        {
            return new Note(Pitch, Length * 2, StartTime);
        }

        public Note Half()
        {
            return new Note(Pitch, Length / 2, StartTime);
        }

        public Note AtTime(int time)
        {
            return new Note(Pitch, Length, time);
        }

        public Note ParallelWith(Note other)
        {
            return new Note(Pitch, Length, other.StartTime);
        }

        public Note After(Note other)
        {
            return new Note(Pitch, Length, other.EndTime);
        }

        #endregion

        public override string ToString()
        {
            var startPart = new Ratio(StartTime, (int)NoteValue.Whole).Simplify().ToString();

            var lengthPart = new Ratio(Length, (int)NoteValue.Whole).Simplify().ToString();

            return $"[{startPart}:{Pitch}:{lengthPart}]";
        }

        public bool Equals(Note? other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.Pitch == Pitch &&
                other.Length == Length &&
                other.StartTime == StartTime;
        }
    }
}
