using Composer.Melody;
using MusicCore;
using System.Diagnostics;
using Tools;

namespace Composer
{
    public class SimpleMelodyMaker
    {
        private readonly Random rand;

        private const double StrongBeatNonChordTonesRatio = 0.2;

        private Staff rhythm;
        private Chord[] chords;
        private Key key;
        private MusicalScale scale;
        private Clef clef;
        private int range;
        private int tempo;
        private Instrument instrument;

        private readonly List<IPitchFilter> strongNoteFilters;
        private readonly List<IPitchFilter> weakNoteFilters;

        bool useChromaticTransitions;

        public SimpleMelodyMaker()
        {
            rand = new Random();

            rhythm = CreateDefaultRhythm();
            chords = new[] { new Chord(0, 2, 4) };
            key = Key.C;
            scale = MusicalScale.Major;
            clef = Clef.Treble;
            range = 12;
            tempo = 120;
            instrument = Instrument.AcousticGrandPiano;

            useChromaticTransitions = false;

            strongNoteFilters = new List<IPitchFilter>()
            {
                new FilterInsideStaff(),
                new FilterChordTones(),
                new FilterCloseToNeighbors(),
                new FilterAvoidRepeats()
            };

            weakNoteFilters = new List<IPitchFilter>()
            {
                new FilterInsideStaff(),
                new FilterAvoidRepeats(),
                new FilterForbiddenTensions(),
                new FilterSmoothTransition()
            };
        }

        public SimpleMelodyMaker WithRhythm(Staff rhythm)
        {
            this.rhythm = rhythm;
            return this;
        }

        public SimpleMelodyMaker OverChords(Chord[] chords)
        {
            this.chords = chords;
            return this;
        }

        public SimpleMelodyMaker InKey(Key key, MusicalScale scale)
        {
            this.key = key;
            this.scale = scale;
            return this;
        }

        public SimpleMelodyMaker InTempo(int tempo)
        {
            this.tempo = tempo;
            return this;
        }

        public SimpleMelodyMaker InClef(Clef clef)
        {
            this.clef = clef;
            return this;
        }

        public SimpleMelodyMaker InRange(int halftoneRange)
        {
            range = halftoneRange;
            return this;
        }

        public SimpleMelodyMaker OnInstrument(Instrument instrument)
        {
            this.instrument = instrument;
            return this;
        }

        public SimpleMelodyMaker WithChromaticTransitions()
        {
            useChromaticTransitions = true;
            return this;
        }

        public SimpleMelodyMaker Above(Staff other, bool allowEqual = true)
        {
            var filter = new FilterRelative(other, true, allowEqual);
            strongNoteFilters.Add(filter);
            weakNoteFilters.Add(filter);
            return this;
        }

        public SimpleMelodyMaker Below(Staff other, bool allowEqual = true)
        {
            var filter = new FilterRelative(other, false, allowEqual);
            strongNoteFilters.Add(filter);
            weakNoteFilters.Add(filter);
            return this;
        }

        public SimpleMelodyMaker WithNctStrongBeats(double nctRatio = StrongBeatNonChordTonesRatio)
        {
            strongNoteFilters.RemoveAll(filter => filter is FilterChordTones);
            var filter = new FilterChordTones(nctRatio);
            strongNoteFilters.Add(filter);
            return this;
        }

        public SimpleMelodyMaker OnlyChordStrongBeats()
        {
            strongNoteFilters.RemoveAll(filter => filter is FilterChordTones);
            var filter = new FilterChordTones();
            strongNoteFilters.Add(filter);
            return this;
        }

        public Staff GenerateMelody(int measuresCount = -1)
        {
            if (measuresCount <= 0)
            {
                measuresCount = Math.Min(chords.Length, rhythm.MeasureCount);
            }

            var result = new Staff(clef, key, scale, rhythm.Meter, tempo, measuresCount, instrument: instrument);

            var notes = AvailableNotes();
            InitializeFilters(notes);

            FillFirstMeasure(result, notes, chords[0], rhythm.Measures[0]);

            for (var measure = 1; measure < measuresCount - 1; measure++)
            {
                FillMeasure(result, notes, measure, chords[measure % chords.Length], rhythm.Measures[measure % rhythm.MeasureCount]);
            }

            FillLastMeasure(result, notes, measuresCount - 1, chords[(measuresCount - 1) % chords.Length], 
                rhythm.Measures[(measuresCount - 1) % rhythm.MeasureCount]);

            return result;
        }

        private Staff CreateDefaultRhythm()
        {
            var result = new Staff(meter: Meter.CC, measuresCount: 2);
            result.AddNote(0, new Note(0, (int)NoteValue.Quarter))
                .AddNext(new Note(0, (int)NoteValue.Quarter))
                .AddNext(new Note(0, (int)NoteValue.Quarter))
                .AddNext(new Note(0, (int)NoteValue.Quarter))
                .AddNext(new Note(0, (int)NoteValue.Quarter))
                .AddNext(new Note(0, (int)NoteValue.Quarter))
                .AddNext(new Note(0, (int)NoteValue.Half));
            return result;
        }

        private ScaleStep[] AvailableNotes()
        {
            var rangeFrom = -range - (int)key;
            var rangeTo = range - (int)key;

            var bottomOctave = rangeFrom < 0 ?
                rangeFrom / 12 - 1 :
                rangeFrom / 12;

            var topOctave = rangeTo > 0 ?
                rangeTo / 12 + 1 :
                rangeTo / 12;

            var notes = new List<ScaleStep>();

            for (var octave = bottomOctave; octave <= topOctave; octave++)
            {
                for (var step = 0; step < scale.Count; step++)
                {
                    var note = new ScaleStep(step, Accidental.None, octave);
                    var absolutePitch = scale.StepToPitch(note);
                    if (rangeFrom <= absolutePitch && absolutePitch <= rangeTo)
                    {
                        notes.Add(note);
                    }
                }
            }

            return notes.ToArray();
        }

        private void InitializeFilters(ScaleStep[] availableNotes)
        {
            foreach (var filter in strongNoteFilters.Concat(weakNoteFilters))
            {
                filter.Setup(availableNotes, scale, key, clef);
            }
        }

        private double[] ApplyFilters(
            ScaleStep[] notes,
            IEnumerable<IPitchFilter> filters,
            Chord chord,
            ScaleStep? previousNote,
            ScaleStep? nextNote,
            bool nextIsStrong,
            int measure,
            int startTime,
            int endTime)
        {
            var weights = Enumerable.Repeat(1.0, notes.Length).ToArray();

            foreach (var filter in filters)
            {
                var fromFilter = filter.GetWeights(chord, previousNote, nextNote, nextIsStrong, measure, startTime, endTime);
                weights = weights.Mult(fromFilter);
            }

            return weights;
        }

        private ScaleStep ApplyAccidental(ScaleStep note, Chord chord)
        {
            var chordTone = chord.Notes.FirstOrDefault(n => n.Step == note.Step);
            if (chordTone is not null)
            {
                Debug.WriteLine($"For note {note} over {chord}, applying {chordTone.Accidental}");
                return new ScaleStep(note.Step, chordTone.Accidental, note.Octave);
            }

            return note;
        }

        private void FillFirstMeasure(Staff target, ScaleStep[] notes, Chord chord, IReadOnlyList<Note> rhythm)
        {
            var weights = ApplyFilters(notes, strongNoteFilters, chord, null, null, false, 0, rhythm[0].StartTime, rhythm[0].EndTime);
            var currentPitch = notes.SelectRandomly(weights, rand);
            currentPitch = ApplyAccidental(currentPitch, chord);
            target.AddNote(0, rhythm[0].AtPitch(currentPitch));

            foreach (var note in rhythm.Skip(1).Where(n => n.Pitch.Step == 0))
            {
                weights = ApplyFilters(notes, strongNoteFilters, chord, currentPitch, null, false, 0, note.StartTime, note.EndTime);
                currentPitch = notes.SelectRandomly(weights, rand);
                currentPitch = ApplyAccidental(currentPitch, chord);
                target.AddNote(0, note.AtPitch(currentPitch));
            }

            FillWeakBeats(target, 0, notes, chord, rhythm);
        }

        private void FillMeasure(Staff target, ScaleStep[] notes, int measure, Chord chord, IReadOnlyList<Note> rhythm)
        {
            var currentPitch = target.NoteBefore(measure, 0)?.Pitch ?? new ScaleStep(0);

            foreach (var note in rhythm.Where(n => n.Pitch.Step == 0))
            {
                var weights = ApplyFilters(notes, strongNoteFilters, chord, currentPitch, null, false, measure, note.StartTime, note.EndTime);
                currentPitch = notes.SelectRandomly(weights, rand);
                currentPitch = ApplyAccidental(currentPitch, chord);
                target.AddNote(measure, note.AtPitch(currentPitch));
            }

            FillWeakBeats(target, measure, notes, chord, rhythm);
        }

        private bool UnifySamePitch(Staff target, int measure, ScaleStep currentPitch)
        {
            if (target.Measures[measure].Count > 1 &&
                target.Measures[measure].All(note => note.Pitch.Equals(currentPitch)) &&
                Enum.GetValues<NoteValue>().Cast<int>().Contains(target.MeasureLength))
            {
                target.ClearMeasure(measure);
                target.AddNote(measure, new Note(currentPitch, target.MeasureLength));
                return true;
            }

            return false;
        }

        private void FillLastMeasure(Staff target, ScaleStep[] notes, int measure, Chord chord, IReadOnlyList<Note> rhythm)
        {
            var currentPitch = target.NoteBefore(measure, 0)?.Pitch ?? new ScaleStep(0);

            var lastBeat = rhythm.Last(n => n.Pitch.Step == 0).StartTime;
            var finalPitch = NearestTonic(notes, target.Scale, currentPitch);
            target.AddNote(measure, new Note(finalPitch, target.MeasureLength - lastBeat, lastBeat));

            if (rhythm.Count(n => n.Pitch.Step == 0) > 1)
            {              
                foreach (var note in rhythm.Where(n => n.Pitch.Step == 0 && n.StartTime < lastBeat))
                {
                    var weights = ApplyFilters(notes, strongNoteFilters, chord, currentPitch, finalPitch, true, measure, note.StartTime, note.EndTime);
                    currentPitch = notes.SelectRandomly(weights, rand);
                    currentPitch = ApplyAccidental(currentPitch, chord);
                    target.AddNote(measure, note.AtPitch(currentPitch));
                }
            }

            if (!UnifySamePitch(target, measure, currentPitch))
            {
                FillWeakBeats(target, measure, notes, chord, rhythm, lastBeat);
            }
        }

        private void FillWeakBeats(Staff target, int measure, ScaleStep[] notes, Chord chord, IReadOnlyList<Note> rhythm, int end = int.MaxValue)
        {
            foreach (var note in rhythm.Where(n => n.Pitch.Step > 0 && n.EndTime <= end))
            {
                var nextStrong = rhythm.FirstOrDefault(n => n.Pitch.Step == 0 && n.StartTime > note.StartTime);
                var nextWeak = rhythm.FirstOrDefault(n => n.Pitch.Step > 0 && n.StartTime > note.StartTime);
                var nextIsStrong = 
                    nextWeak == null || (nextStrong != null && nextStrong.StartTime < nextWeak.StartTime);

                var before = target.NoteBefore(measure, note.StartTime);
                var after = target.NoteAfter(measure, note.StartTime);

                var weights = ApplyFilters(notes, weakNoteFilters, chord, before?.Pitch, after?.Pitch, nextIsStrong, measure, note.StartTime, note.EndTime);
                var currentPitch = notes.SelectRandomly(weights, rand);
                currentPitch = ApplyAccidental(currentPitch, chord);

                target.AddNote(measure, note.AtPitch(currentPitch));
            }

            if (useChromaticTransitions)
            {
                ApplyChromaticTransitions(target, measure, rhythm);
            }
        }

        private void ApplyChromaticTransitions(Staff target, int measure, IReadOnlyList<Note> rhythm)
        {
            foreach (var note in rhythm.Where(n => n.Pitch.Step > 0))
            {
                var before = target.NoteBefore(measure, note.StartTime);
                var current = target.NoteAt(measure, note.StartTime);
                var after = target.NoteAfter(measure, note.StartTime);

                if (before != null && current != null && after != null)
                {
                    var newPitch = ApplyChromaticModification(before.Pitch, current.Pitch, after.Pitch);
                    if (!newPitch.Equals(current.Pitch))
                    {
                        target.ReplaceNote(measure, current, current.AtPitch(newPitch));
                    }
                }
            }
        }

        private ScaleStep ApplyChromaticModification(ScaleStep before, ScaleStep current, ScaleStep after)
        {
            // Chromatic neighbor
            if (before.Equals(current) && after.Equals(current))
            {
                var mod = rand.Next(2) * 2 - 1;
                var newAccidental = (int)current.Accidental + mod;
                newAccidental = newAccidental.Limit(-2, 2);
                return current.WithAccidental((Accidental)newAccidental);
            }

            // Chromatic passing tone
            var stepToNext = scale.HalftoneIterval(before, after);
            if (Math.Abs(stepToNext) == 2)
            {
                var change = stepToNext / 2;
                if (before.Equals(current))
                {
                    return scale.ChangeByHalftones(current, change);
                }
                if (after.Equals(current))
                {
                    return scale.ChangeByHalftones(current, -change);
                }
            }

            return current;
        }

        private ScaleStep NearestTonic(ScaleStep[] notes, MusicalScale scale, ScaleStep pitch)
        {
            ScaleStep result = new ScaleStep(0);
            int dist = int.MaxValue;

            foreach (var note in notes.Where(n => n.Step == 0))
            {
                var d = Math.Abs(scale.HalftoneIterval(note, pitch));

                if (d < dist)
                {
                    dist = d;
                    result = note;
                }
            }

            return result;
        }
    }
}
