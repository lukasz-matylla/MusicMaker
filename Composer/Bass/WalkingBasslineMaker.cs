using MusicCore;

namespace Composer
{
    public class WalkingBasslineMaker : BassMakerBase, IBasslineMaker
    {
        protected override int Cutoff => 11;

        protected override void FillBar(Staff result, int measure, Chord chord, Chord nextChord, IReadOnlyList<Note> beats, int topOfStaff, int octaveOffset)
        {
            var scale = result.Scale;

            var current = GetChordTone(chord, 0, topOfStaff, octaveOffset);
            var previousRoot = result.NoteAt(measure - 1, 0)?.Pitch;
            var previousLast = result.NoteBefore(measure, 0)?.Pitch;
            var nextRoot = GetChordTone(nextChord, 0, topOfStaff, octaveOffset);

            bool directionUp;
            if (previousRoot == null || previousLast == null)
            {
                directionUp = GetDirectionByPosition(scale, current, topOfStaff);
            }
            else
            {
                directionUp = GetDirectionByPrevious(scale, previousRoot, previousLast);
            }
            if (directionUp)
            {
                current = current.WithOctave(current.Octave - 1);
            }

            var numBeats = result.Meter.Top;
            if (numBeats < 3)
            {
                numBeats *= 2;
            }
            var noteLength = result.MeasureLength / numBeats;

            result.AddNote(measure, new Note(current, noteLength));

            for (var i = 1; i < numBeats - 1; i++)
            {
                current = directionUp ?
                    ChordOperations.NextToneAbove(chord, scale, current) :
                    ChordOperations.NextToneBelow(chord, scale, current);

                result.AddNext(new Note(current, noteLength));
            }

            current = GetFinalNote(scale, chord, current, nextRoot, directionUp);

            result.AddNext(new Note(current, noteLength));
        }

        protected override void FillLastBar(Staff result, int measure, Chord chord, IReadOnlyList<Note> beats, int topOfStaff, int octaveOffset)
        {
            var bass = GetChordTone(chord, 0, topOfStaff, octaveOffset);

            var previousRoot = result.NoteAt(measure - 1, 0)?.Pitch;
            var previousLast = result.NoteBefore(measure, 0)?.Pitch;
            if (previousRoot != null && previousLast != null)
            {
                var directionUp = GetDirectionByPrevious(result.Scale, previousRoot, previousLast);
                if (!directionUp)
                {
                    bass = bass.WithOctave(bass.Octave - 1);
                }
            }

            FillWithNote(result, measure, bass, beats, topOfStaff, octaveOffset);
        }

        private ScaleStep GetFinalNote(MusicalScale scale, Chord chord, ScaleStep current, ScaleStep nextRoot, bool directionUp)
        {
            if (!directionUp)
            {
                nextRoot = nextRoot.WithOctave(nextRoot.Octave - 1);
            }

            var nextChordTone = directionUp ?
                    ChordOperations.NextToneAbove(chord, scale, current) :
                    ChordOperations.NextToneBelow(chord, scale, current);

            var stepsFromChordTone = scale.StepInterval(nextChordTone, nextRoot);
            var halftonesFromChordTone = scale.HalftoneInterval(nextChordTone, nextRoot);

            if (Math.Abs(stepsFromChordTone) == 1 ||
                Math.Abs(halftonesFromChordTone) == 1)
            {
                // next chord tone approaches by step or half-step
                return nextChordTone;
            }

            if (halftonesFromChordTone == 5 ||
                halftonesFromChordTone == -7)
            {
                // next chord tone approaches by a fifth
                return nextChordTone;
            }

            // next chord tone is too far, try subsequent one
            nextChordTone = directionUp ?
                    ChordOperations.NextToneAbove(chord, scale, nextChordTone) :
                    ChordOperations.NextToneBelow(chord, scale, nextChordTone);

            if (Math.Abs(stepsFromChordTone) == 1 ||
                Math.Abs(halftonesFromChordTone) == 1)
            {
                // next chord tone approaches by step or half-step
                return nextChordTone;
            }

            var stepsFromCurrent = scale.StepInterval(current, nextRoot);
            var halftonesFromCurrent = scale.HalftoneInterval(current, nextRoot);

            if (Math.Abs(halftonesFromCurrent) == 2)
            {
                // chromatic passing tone
                return scale.ChangeByHalftones(current, halftonesFromCurrent / 2);
            }

            if (Math.Abs(stepsFromCurrent) == 2)
            {
                // passing tone
                return scale.ChangeBySteps(current, stepsFromCurrent / 2);
            }

            if (Math.Abs(stepsFromCurrent) > 2)
            {
                // too far, try to approach by step
                var target = scale.ChangeBySteps(nextRoot, directionUp ? -1 : 1);
                if (scale.Steps.Any(s => s == scale.StepToPitch(target.WithOctave(0))))
                {
                    return target;
                }
                target = scale.ChangeByHalftones(target, directionUp ? 1 : -1);
                return target;
            }

            // half tone from or at the target root - overshoot
            if (directionUp)
            {
                var target = scale.ChangeByHalftones(nextRoot, 2);
                if (scale.Steps.Any(s => s == scale.StepToPitch(target.WithOctave(0))))
                {
                    return target;
                }
                target = scale.ChangeByHalftones(target, -1);
                return target;
            }
            else
            {
                var target = scale.ChangeByHalftones(nextRoot, -2);
                if (scale.Steps.Any(s => s == scale.StepToPitch(target.WithOctave(0))))
                {
                    return target;
                }
                target = scale.ChangeByHalftones(target, 1);
                return target;
            }
        }

        private bool GetDirectionByPrevious(MusicalScale scale, ScaleStep previousRoot, ScaleStep previousLast)
        {
            var interval = scale.StepInterval(previousRoot, previousLast);
            return interval < 0;
        }

        private bool GetDirectionByPosition(MusicalScale scale, ScaleStep current, int topOfStaff)
        {
            return current.Step + (current.Octave + 1)*scale.Count <= topOfStaff;

        }
    }
}
