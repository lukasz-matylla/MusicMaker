using MusicCore;
using System.Diagnostics;
using Tools;

namespace Composer
{
    public class SimpleMelodyMaker
    {
        public int RangeFrom { get; }
        public int RangeTo { get; }

        private readonly Random rand;

        private const double FirstNoteCloseness = 0.4;
        private const double NextNoteCloseness = 0.7;
        private const double MiddleOctaveCutoff = 0.9;
        private const double RepeatDowngrade = 0.3;

        public SimpleMelodyMaker(int rangeFrom = 60, int rangeTo = 79)
        {
            RangeFrom = rangeFrom;
            RangeTo = rangeTo;

            rand = new Random();
        }

        public Staff GenerateMelody(Chord[] chords, 
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

            var notes = AvailableNotes(scale, RangeFrom - tonic, RangeTo - tonic);         
            var result = new Staff(tonic, scale, rhythm.Meter, tempo, measuresCount);

            FillFirstMeasure(result, notes, chords[0], rhythm.Measures[0], scale);

            for (var measure = 1; measure < measuresCount - 1; measure++)
            {
                FillMeasure(result, notes, measure, chords[measure % chords.Length], rhythm.Measures[measure % rhythm.MeasureCount], scale);
            }

            FillLastMeasure(result, notes, measuresCount - 1, chords[(measuresCount - 1) % chords.Length], rhythm.Measures[(measuresCount - 1) % rhythm.MeasureCount], scale);

            return result;
        }

        private ScaleStep[] AvailableNotes(MusicalScale scale, int rangeFrom, int rangeTo)
        {
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

        private double[] FirstOctave(ScaleStep[] notes)
        {
            return Enumerable.Range(0, notes.Length)
                .Select(i => notes[i].Octave == 0 ? 1.0 : 0.0)
                .ToArray();
        }

        private double[] NearMiddle(ScaleStep[] notes, MusicalScale scale, double alpha = FirstNoteCloseness)
        {
            return Enumerable.Range(0, notes.Length)
                .Select(i => notes[i].Octave < 0 ?
                    scale.NoteInterval(notes[i], new ScaleStep(0)) : 
                    notes[i].Octave > 0 ? scale.NoteInterval(new ScaleStep(0, octave: 1), notes[i]) : 0)
                .Select(x => Math.Pow(alpha, x))
                .ToArray();
        }

        private double[] ChordTones(ScaleStep[] notes, Chord chord)
        {
            return Enumerable.Range(0, notes.Length)
                .Select(i => chord.Notes.Any(cn => cn.Step == notes[i].Step) ? 1.0 : 0.0)
                .ToArray();
        }

        private double[] NearTone(ScaleStep[] notes, MusicalScale scale, ScaleStep note, double alpha = NextNoteCloseness)
        {
            return Enumerable.Range(0, notes.Length)
                .Select(i => Math.Abs(scale.NoteInterval(notes[i], note)))
                .Select(v => Math.Pow(alpha, v))
                .ToArray();
        }

        private double[] StrongNearTone(ScaleStep[] notes, MusicalScale scale, ScaleStep note, double alpha = NextNoteCloseness)
        {
            return Enumerable.Range(0, notes.Length)
                .Select(i => Math.Abs(scale.NoteInterval(notes[i], note)))
                .Select(v => Math.Pow(alpha, v * v))
                .ToArray();
        }

        private double[] AvoidRepeat(ScaleStep[] notes, ScaleStep note, double alpha = RepeatDowngrade)
        {
            return Enumerable.Range(0, notes.Length)
                .Select(i => notes[i].Equals(note) ? alpha : 1.0)
                .ToArray();
        }

        private double[] AvoidForbiddenTensions(ScaleStep[] notes, Chord chord, MusicalScale scale, double alpha = RepeatDowngrade)
        {
            return Enumerable.Range(0, notes.Length)
                .Select(i => chord.Notes.Any(n => scale.NormalizedHalftoneInterval(n, notes[i]) == 1) ? alpha : 1.0)
                .ToArray();
        }

        private double[] AllEqual(ScaleStep[] notes, double v = 1.0)
        {
            return Enumerable.Repeat(v, notes.Length)
                .ToArray();
        }

        private ScaleStep ApplyAccidental(ScaleStep note, Chord chord, MusicalScale scale)
        {
            var chordTone = chord.Notes.FirstOrDefault(n => n.Step == note.Step);
            if (chordTone != null)
            {
                Debug.WriteLine($"For note {note} over {chord}, applying {chordTone.Accidental}");
                return new ScaleStep(note.Step, chordTone.Accidental, note.Octave);
            }

            return note;
        }

        private void FillFirstMeasure(Staff target, ScaleStep[] notes, Chord chord, IReadOnlyList<Note> rhythm, MusicalScale scale)
        {
            var weights = ChordTones(notes, chord).Mult(FirstOctave(notes));

            var currentPitch = notes.SelectRandomly(weights, rand);
            currentPitch = ApplyAccidental(currentPitch, chord, scale);
            target.AddNote(rhythm[0].AtPitch(currentPitch), 0);

            foreach (var note in rhythm.Skip(1).Where(n => n.Pitch.Step == 0))
            {
                weights = ChordTones(notes, chord)
                    .Mult(NearTone(notes, target.Scale, currentPitch, NextNoteCloseness))
                    .Mult(AvoidRepeat(notes, currentPitch, RepeatDowngrade))
                    .Mult(NearMiddle(notes, target.Scale, MiddleOctaveCutoff));
                currentPitch = notes.SelectRandomly(weights, rand);
                currentPitch = ApplyAccidental(currentPitch, chord, scale);
                target.AddNote(note.AtPitch(currentPitch), 0);
            }

            FillWeakBeats(target, 0, target.Scale, notes, chord, rhythm);
        }

        private void FillMeasure(Staff target, ScaleStep[] notes, int measure, Chord chord, IReadOnlyList<Note> rhythm, MusicalScale scale)
        {
            var currentPitch = target.NoteBefore(measure, 0)?.Pitch ?? new ScaleStep(0);

            var weights = ChordTones(notes, chord)
                    .Mult(NearTone(notes, target.Scale, currentPitch, FirstNoteCloseness))
                    .Mult(NearMiddle(notes, target.Scale, MiddleOctaveCutoff));
            currentPitch = notes.SelectRandomly(weights, rand);
            currentPitch = ApplyAccidental(currentPitch, chord, scale);
            target.AddNote(rhythm[0].AtPitch(currentPitch), measure);

            foreach (var note in rhythm.Skip(1).Where(n => n.Pitch.Step == 0))
            {
                weights = ChordTones(notes, chord)
                    .Mult(NearTone(notes, target.Scale, currentPitch, NextNoteCloseness))
                    .Mult(AvoidRepeat(notes, currentPitch, RepeatDowngrade))
                    .Mult(NearMiddle(notes, target.Scale, MiddleOctaveCutoff));
                currentPitch = notes.SelectRandomly(weights, rand);
                currentPitch = ApplyAccidental(currentPitch, chord, scale);
                target.AddNote(note.AtPitch(currentPitch), measure);
            }

            FillWeakBeats(target, measure, target.Scale, notes, chord, rhythm);
        }

        private void FillLastMeasure(Staff target, ScaleStep[] notes, int measure, Chord chord, IReadOnlyList<Note> rhythm, MusicalScale scale)
        {
            var currentPitch = target.NoteBefore(measure, 0)?.Pitch ?? new ScaleStep(0);

            if (rhythm.Count(n => n.Pitch.Step == 0) > 1)
            {
                var weights = ChordTones(notes, chord)
                .Mult(NearTone(notes, target.Scale, currentPitch, FirstNoteCloseness))
                .Mult(NearMiddle(notes, target.Scale, MiddleOctaveCutoff));
                currentPitch = notes.SelectRandomly(weights, rand);
                currentPitch = ApplyAccidental(currentPitch, chord, scale);
                target.AddNote(rhythm[0].AtPitch(currentPitch), measure);

                var lastBeat = rhythm.Last().StartTime;
                foreach (var note in rhythm.Skip(1).Where(n => n.Pitch.Step == 0 && n.StartTime < lastBeat))
                {
                    weights = ChordTones(notes, chord)
                        .Mult(NearTone(notes, target.Scale, currentPitch, NextNoteCloseness))
                        .Mult(NearMiddle(notes, target.Scale, MiddleOctaveCutoff));
                    currentPitch = notes.SelectRandomly(weights, rand);
                    currentPitch = ApplyAccidental(currentPitch, chord, scale);
                    target.AddNote(note.AtPitch(currentPitch), measure);
                }
            }

            currentPitch = NearestTonic(notes, target.Scale, currentPitch);
            target.AddNote(rhythm.Last().AtPitch(currentPitch), measure);

            FillWeakBeats(target, measure, target.Scale, notes, chord, rhythm);
        }

        private void FillWeakBeats(Staff target, int measure, MusicalScale scale, ScaleStep[] notes, Chord chord, IReadOnlyList<Note> rhythm)
        {
            foreach (var note in rhythm.Where(n => n.Pitch.Step > 0))
            {
                var nextStrong = rhythm.FirstOrDefault(n => n.Pitch.Step == 0 && n.StartTime > note.StartTime);
                var nextWeak = rhythm.FirstOrDefault(n => n.Pitch.Step > 0 && n.StartTime > note.StartTime);
                var nextIsStrong = nextWeak == null || 
                    (nextStrong != null && nextStrong.StartTime < nextWeak.StartTime);

                var weights = AvoidForbiddenTensions(notes, chord, scale);

                var before = target.NoteBefore(measure, note.StartTime);
                var after = target.NoteAfter(measure, note.StartTime);

                if (before != null && after != null && nextIsStrong && Math.Abs(scale.NoteInterval(before.Pitch, after.Pitch)) >= 3)
                {
                    var currentPitch = EscapeTone(notes, scale, before.Pitch, after.Pitch);
                    target.AddNote(note.AtPitch(currentPitch), measure);
                }
                else
                {
                    if (before != null)
                    {
                        weights = weights.Mult(StrongNearTone(notes, scale, before.Pitch))
                            .Mult(AvoidRepeat(notes, before.Pitch));
                    }


                    if (after != null)
                    {
                        if (nextIsStrong)
                        {
                            weights = weights.Mult(StrongNearTone(notes, scale, after.Pitch))
                                .Mult(AvoidRepeat(notes, after.Pitch));
                        }
                        else
                        {
                            weights = weights.Mult(NearTone(notes, scale, after.Pitch))
                                .Mult(AvoidRepeat(notes, after.Pitch));
                        }
                    }

                    weights = weights.Mult(ChordTones(notes, chord).Add(AllEqual(notes, 1.0)));

                    var currentPitch = notes.SelectRandomly(weights, rand);
                    currentPitch = ApplyAccidental(currentPitch, chord, scale);
                    target.AddNote(note.AtPitch(currentPitch), measure);
                }
            }
        }

        private ScaleStep EscapeTone(ScaleStep[] notes, MusicalScale scale, ScaleStep before, ScaleStep after)
        {
            if (scale.NoteInterval(before, after) > 0)
            {
                return scale.ChangeBySteps(before, 1);
            }

            return scale.ChangeBySteps(before, -1);
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
