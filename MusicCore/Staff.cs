using System.Text;

namespace MusicCore
{
    public class Staff
    {
        public Clef Clef { get; }
        public Key Key { get; }
        public int ReferencePitch { get; }
        public MusicalScale Scale { get; }
        public int StartTime { get; }
        public int Tempo { get; }
        public Meter Meter { get; }
        public Instrument Instrument { get; }
        
        public int MeasureCount => Measures.Count;
        public int MeasureLength => Meter.MeasureLength;
        public int TotalLength => MeasureCount * MeasureLength;

        public OverflowBehavior MeasureOverflow { get; }
        public OverflowBehavior StaffOverflow { get; }

        public IReadOnlyList<IReadOnlyList<Note>> Measures => measures;
        public List<List<Note>> measures { get; }

        public Staff(Clef clef = Clef.Treble,
            Key key = Key.C,
            MusicalScale? scale = null,
            Meter? meter = null,
            int tempo = 120,
            int measuresCount = 8,
            int startTime = 0,
            Instrument instrument = Instrument.AcousticGrandPiano,
            OverflowBehavior measureOverflow = OverflowBehavior.Clip,
            OverflowBehavior staffOverflow = OverflowBehavior.Clip)
        {
            Clef = clef;
            Key = key;
            Scale = scale ?? MusicalScale.Major;
            Meter = meter ?? Meter.CC;
            StartTime = startTime;
            Instrument = instrument;
            Tempo = tempo;

            ReferencePitch = 12 * (int)Clef + (int)Key;

            measures = CreateMeasures(measuresCount);

            MeasureOverflow = measureOverflow;
            StaffOverflow = staffOverflow;
        }

        public IReadOnlyList<Note> this[int measure]
        {
            get => Measures[measure];
        }

        #region Copying

        public Staff Empty()
        {
            return new Staff(Clef, Key, Scale, Meter, Tempo, MeasureCount, StartTime, Instrument, MeasureOverflow, StaffOverflow);
        }

        public Staff CopyTo(Staff target, int targetPosition = 0, int fromMeasure = 0, int measureCount = -1)
        {
            if (measureCount < 0)
            {
                measureCount = MeasureCount;
            }

            for (var measure = 0; measure < measureCount; measure++)
            {
                foreach (var note in Measures[fromMeasure + measure])
                {
                    target.AddNote(note, targetPosition + measure);
                }
            }

            return target;
        }

        public Staff Clone()
        {
            return CopyTo(Empty());
        }

        public Staff Append(Staff other)
        {
            var copy = new Staff(Clef, Key, Scale, Meter, Tempo, MeasureCount + other.MeasureCount, StartTime, Instrument, MeasureOverflow, StaffOverflow);

            CopyTo(copy);
            other.CopyTo(copy, MeasureCount);

            return copy;
        }

        public Staff Merge(Staff other)
        {
            var copy = new Staff(Clef, Key, Scale, Meter, Tempo, Math.Max(MeasureCount, other.MeasureCount), StartTime, Instrument, MeasureOverflow, StaffOverflow);

            CopyTo(copy);
            other.CopyTo(copy);

            return copy;
        }

        public Staff ToScale(Clef clef, Key key, MusicalScale scale)
        {
            var copy = new Staff(clef, key, scale, Meter, Tempo, MeasureCount, StartTime, Instrument, MeasureOverflow, StaffOverflow);
            return CopyTo(copy);
        }

        public Staff ToTempo(int tempo)
        {
            var copy = new Staff(Clef, Key, Scale, Meter, tempo, MeasureCount, StartTime, Instrument, MeasureOverflow, StaffOverflow);
            return CopyTo(copy);
        }

        public Staff ToStartTime(int startTime)
        {
            var copy = new Staff(Clef, Key, Scale, Meter, Tempo, MeasureCount, startTime, Instrument, MeasureOverflow, StaffOverflow);
            return CopyTo(copy);
        }

        #endregion

        #region Helpers

        public int ToAbsolutePitch(ScaleStep n)
        {
            return ReferencePitch + Scale.StepToPitch(n);
        }

        public Note? NoteBefore(int measure, int startTime)
        {
            var prev = Measures[measure].OrderBy(n => n.StartTime).LastOrDefault(n => n.StartTime < startTime);

            if (prev == null && measure > 0)
            {
                prev = Measures[measure - 1].OrderBy(n => n.StartTime).LastOrDefault();
            }

            return prev;
        }

        public Note? NoteAfter(int measure, int startTime)
        {
            var prev = Measures[measure].OrderBy(n => n.StartTime).FirstOrDefault(n => n.StartTime > startTime);

            if (prev == null && measure < MeasureCount - 1)
            {
                prev = Measures[measure + 1].OrderBy(n => n.StartTime).FirstOrDefault();
            }

            return prev;
        }

        #endregion
   
        public Note? AddNote(Note note, int measure)
        {
            if (measure < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(measure));
            }

            if (measure >= MeasureCount)
            {
                switch (StaffOverflow)
                {
                    case OverflowBehavior.Clip:
                        return null;
                    case OverflowBehavior.Extend:
                        EnsureMeasures(measure);
                        break;
                    case OverflowBehavior.Throw:
                        throw new ArgumentOutOfRangeException(nameof(measure));
                }
            }

            if (note.StartTime < 0 || note.StartTime >= MeasureLength)
            {
                throw new ArgumentOutOfRangeException(nameof(note.StartTime));
            }

            if (note.EndTime > MeasureLength)
            {
                switch (MeasureOverflow)
                {
                    case OverflowBehavior.Clip:
                        note = note.WithLength(MeasureLength - note.StartTime);
                        break;
                    case OverflowBehavior.Extend:
                        AddNote(note.WithLength(MeasureLength - note.StartTime), measure);
                        return AddNote(note.WithLength(note.EndTime - MeasureLength), measure + 1);
                    case OverflowBehavior.Throw:
                        throw new ArgumentOutOfRangeException(nameof(note.EndTime));
                }
            }

            var prevIndex = measures[measure].FindIndex(n => n.Pitch == note.Pitch && n.StartTime == note.StartTime);

            if (prevIndex >= 0)
            {
                measures[measure][prevIndex] = note;
            }
            else
            {
                prevIndex = measures[measure].FindLastIndex(n => n.StartTime <= note.StartTime);
                measures[measure].Insert(prevIndex + 1, note);
            }

            return note;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var measure = 0; measure < MeasureCount; measure++)
            {
                if (Measures[measure].Count == 0)
                {
                    sb.AppendLine($"M{measure} is empty");
                    sb.AppendLine();

                    continue;
                }

                sb.AppendLine($"M{measure}: ");

                sb.AppendLine(string.Join(" ", Measures[measure].Select(n => n.ToString())));
                sb.AppendLine();
            }

            return sb.ToString();
        }

        #region PrivateMethods

        private List<List<Note>> CreateMeasures(int measuresCount)
        {
            var result = new List<List<Note>>();

            for (var i = 0; i < measuresCount; i++)
            {
                result.Add(new List<Note>());
            }

            return result;
        }

        private void EnsureMeasures(int measuresCount)
        {
            if (MeasureCount >= measuresCount)
            {
                return;
            }

            for (var i = 0; i < measuresCount - MeasureCount; i++)
            {
                measures.Add(new List<Note>());
            }
        }

        #endregion
    }
}
