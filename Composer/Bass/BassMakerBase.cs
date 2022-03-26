using MusicCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Composer
{
    public abstract class BassMakerBase : IBasslineMaker
    {
        protected virtual int Cutoff => 6;

        public Staff GenerateBass(
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

            var wrapAbove = CalculateTopStaffTone(scale, key);

            var result = new Staff(clef, key, scale, rhythm.Meter, tempo, measuresCount);

            for (var measure = 0; measure < measuresCount; measure++)
            {
                var chord = chords[measure % chords.Length];
                var nextChord = chords[(measure + 1) % chords.Length];
                var beats = rhythm[measure % rhythm.MeasureCount];

                if (measure % chords.Length == chords.Length - 1)
                {
                    FillLastBar(result, measure, chord, beats, wrapAbove);
                }
                else
                {
                    FillBar(result, measure, chord, nextChord, beats, wrapAbove);
                }
            }

            return result;
        }

        private int CalculateTopStaffTone(MusicalScale scale, Key key)
        {
            return Enumerable.Range(0, scale.Count).Where(i => scale[i] + (int)key <= Cutoff).Last();
        }

        protected ScaleStep GetChordTone(Chord chord, int index, int wrapAbove)
        {
            var chordBass = chord.Notes[0];
            var chordNote = chord.Notes[index];
            var octave = 0;

            if (chordBass.Step > wrapAbove)
            {
                octave--;
            }

            if (chordNote.Step < chordBass.Step)
            {
                octave++;
            }

            return new ScaleStep(chordNote.Step, chordNote.Accidental, octave);
        }

        protected virtual void FillLastBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int wrapAbove)
        {
            var bass = GetChordTone(chord, 0, wrapAbove);

            result.AddNote(measure, new Note(bass, result.MeasureLength, 0));
        }

        protected virtual void FillBar(Staff result, int measure, Chord chord, Chord nextChord, IReadOnlyList<Note> beats, int wrapAbove)
        {
            FillBar(result, measure, chord, beats, wrapAbove);
        }

        protected abstract void FillBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int wrapAbove);
    }
}
