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

            var octaveWrapThreshold = CalculateTopStaffTone(scale, key);
            bool octaveDown = (int)key > Cutoff;

            var result = new Staff(clef, key, scale, rhythm.Meter, tempo, measuresCount);

            for (var measure = 0; measure < measuresCount; measure++)
            {
                var chord = chords[measure % chords.Length];
                var nextChord = chords[(measure + 1) % chords.Length];
                var beats = rhythm[measure % rhythm.MeasureCount];

                if (measure % chords.Length == chords.Length - 1)
                {
                    FillLastBar(result, measure, chord, beats, octaveWrapThreshold, octaveDown);
                }
                else
                {
                    FillBar(result, measure, chord, nextChord, beats, octaveWrapThreshold, octaveDown);
                }
            }

            return result;
        }

        protected int CalculateTopStaffTone(MusicalScale scale, Key key)
        {
            if ((int)key > Cutoff)
            {
                return Enumerable.Range(0, scale.Count)
                    .Where(i => scale[i] + (int)key - 12 <= Cutoff)
                    .Last();
            }
            return Enumerable.Range(0, scale.Count)
                .Where(i => scale[i] + (int)key <= Cutoff)
                .Last();
        }

        protected virtual ScaleStep GetChordTone(Chord chord, int index, int octaveWrapThreshold, bool octaveDown, bool upwards = true)
        {
            var chordBass = chord.Notes[0];
            var chordNote = chord.Notes[index % chord.Notes.Count];
            var octave = 0;

            if (octaveDown)
            {
                octave--;
            }

            if (chordBass.Step > octaveWrapThreshold)
            {
                octave--;
            }

            if (upwards)
            {
                octave += index / chord.Notes.Count;

                if (chordNote.Step < chordBass.Step)
                {
                    octave++;
                }
            }

            return new ScaleStep(chordNote.Step, chordNote.Accidental, octave);
        }

        protected virtual void FillLastBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int octaveWrapThreshold, bool octaveDown)
        {
            var bass = GetChordTone(chord, 0, octaveWrapThreshold, octaveDown);

            result.AddNote(measure, new Note(bass, result.MeasureLength, 0));
        }

        protected virtual void FillBar(Staff result, int measure, Chord chord, Chord nextChord, IReadOnlyList<Note> beats, int octaveWrapThreshold, bool octaveDown)
        {
            FillBar(result, measure, chord, beats, octaveWrapThreshold, octaveDown);
        }

        protected virtual void FillBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int octaveWrapThreshold, bool octaveDown) { }
    }
}
