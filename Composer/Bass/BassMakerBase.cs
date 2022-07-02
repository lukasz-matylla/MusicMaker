using MusicCore;

namespace Composer
{
    public abstract class BassMakerBase : IBasslineMaker
    {
        protected virtual int Cutoff => 6;

        public virtual Staff GenerateBass(
            Chord[] chords,
            Staff rhythm,
            Key key,
            Clef clef,
            MusicalScale? scale,
            int tempo,
            int measuresCount)
        {
            scale ??= MusicalScale.Major;

            if (measuresCount <= 0)
            {
                measuresCount = chords.Length;
            }

            var octaveOffset = (int)key > Cutoff ?
                -1 :
                (Cutoff - (int)key) / MusicalScale.HalftonesInOctave;
            var topOfStaff = Enumerable.Range(0, scale.Count)
                    .Where(i => scale[i] + (int)key + MusicalScale.HalftonesInOctave * octaveOffset <= Cutoff)
                    .Last();

            var result = new Staff(clef, key, scale, rhythm.Meter, tempo, measuresCount);

            for (var measure = 0; measure < measuresCount; measure++)
            {
                var chord = chords[measure % chords.Length];
                var nextChord = chords[(measure + 1) % chords.Length];
                var beats = rhythm[measure % rhythm.MeasureCount];

                if (measure % chords.Length == chords.Length - 1)
                {
                    FillLastBar(result, measure, chord, beats, topOfStaff, octaveOffset);
                }
                else
                {
                    FillBar(result, measure, chord, nextChord, beats, topOfStaff, octaveOffset);
                }
            }

            return result;
        }

        protected virtual ScaleStep GetChordTone(Chord chord, int index, int topOfStaff, int octaveOffset, bool upwards = true)
        {
            var chordBass = chord.Notes[0];
            var chordNote = chord.Notes[index % chord.Notes.Count];

            if (chordBass.Step > topOfStaff)
            {
                octaveOffset--;
            }

            if (upwards)
            {
                octaveOffset += index / chord.Notes.Count;

                if (chordNote.Step < chordBass.Step)
                {
                    octaveOffset++;
                }
            }

            return new ScaleStep(chordNote.Step, chordNote.Accidental, octaveOffset);
        }

        protected virtual void FillLastBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int topOfStaff, int octaveOffset)
        {
            var bass = GetChordTone(chord, 0, topOfStaff, octaveOffset);
            FillWithNote(result, measure, bass, beats, topOfStaff, octaveOffset);
        }

        protected virtual void FillBar(Staff result, int measure, Chord chord, Chord nextChord, IReadOnlyList<Note> beats, int topOfStaff, int octaveOffset)
        {
            FillBar(result, measure, chord, beats, topOfStaff, octaveOffset);
        }

        protected virtual void FillBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int topOfStaff, int octaveOffset) { }

        protected void FillWithNote(Staff result, int measure, ScaleStep note, IReadOnlyList<Note> beats, int topOfStaff, int octaveOffset)
        {
            if (Enum.GetValues<NoteValue>().Cast<int>().Contains(result.MeasureLength))
            {
                result.AddNote(measure, new Note(note, result.MeasureLength));
                return;
            }

            for (var i = 0; i < result.Meter.Top; i++)
            {
                var start = i * result.Meter.BeatLength;

                if (IsStrongBeat(beats, start))
                {
                    var length = TimeToStrongBeat(beats, start, result.MeasureLength);
                    result.AddNote(measure, new Note(note, length, start));
                }
            }
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
