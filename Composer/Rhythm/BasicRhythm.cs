﻿using MusicCore;
using Tools;

namespace Composer
{
    public class BasicRhythm : IRhythmicPatternGenerator
    {
        private const int CutIntoSectionsThreshold = 8;

        private readonly Random rand;

        private readonly Pattern[] DuplePatterns = new[]
        {
            new Pattern(MeasureType.Any, 8),

            new Pattern(MeasureType.Any, 4, 4),
            new Pattern(MeasureType.Any, 4, 4),

            new Pattern(MeasureType.Any, 6, 2),
            new Pattern(MeasureType.Any, 6, 2),

            new Pattern(MeasureType.Any, 4, 2, 2),
            new Pattern(MeasureType.Any, 4, 2, 2),

            new Pattern(MeasureType.Middle, 2, 2, 2, 2),

            new Pattern(MeasureType.Middle, 2, 6),

            new Pattern(MeasureType.Middle, 2, 2, 4),

            new Pattern(MeasureType.Middle, 2, 4, 2),
        };

        private readonly Pattern[] TriplePatterns = new[]
        {
            new Pattern(MeasureType.Any, 12),

            new Pattern(MeasureType.Any, 8, 4),
            new Pattern(MeasureType.Any, 8, 4),

            new Pattern(MeasureType.Any, 8, 2, 2),
            new Pattern(MeasureType.Any, 8, 2, 2),
            new Pattern(MeasureType.Any, 8, 2, 2),

            new Pattern(MeasureType.Any, 4, 4, 2, 2),
            new Pattern(MeasureType.Any, 4, 4, 2, 2),

            new Pattern(MeasureType.Middle, 4, 2, 2, 4),

            new Pattern(MeasureType.Middle, 4, 2, 2, 2, 2),
            new Pattern(MeasureType.Middle, 4, 2, 2, 2, 2),

            new Pattern(MeasureType.Middle, 4, 8),
            new Pattern(MeasureType.Middle, 4, 8),

            new Pattern(MeasureType.Middle, 2, 2, 8),

            new Pattern(MeasureType.Middle, 2, 2, 4, 4),

            new Pattern(MeasureType.Middle, 2, 2, 4, 2, 2),

            new Pattern(MeasureType.Middle, 2, 2, 2, 2, 2, 2),
        };

        public BasicRhythm()
        {
            rand = new Random();
        }

        public Staff CreateRhythm(int measures, Meter meter, Staff? ignore = null)
        {
            var result = new Staff(meter: meter, measuresCount: measures);

            var strongBeats = RhythmTools.GetStrongBeats(meter);
            var phraseLength = measures > CutIntoSectionsThreshold ?
                RhythmTools.SectionLength(measures, CutIntoSectionsThreshold) :
                measures;

            var step = meter.BeatLength;
            if (meter.Top < 3)
            {
                step /= 2;
            }

            var rhythms = new List<int[]>();
            rhythms.Add(MakeRhythm(meter.MeasureLength, step, strongBeats, MeasureType.Opening));
            for (var i = 1; i < phraseLength - 1; i++)
            {
                rhythms.Add(MakeRhythm(meter.MeasureLength, step, strongBeats, MeasureType.Middle));
            }
            rhythms.Add(MakeRhythm(meter.MeasureLength, step, strongBeats, MeasureType.Closing));

            for (var i = 0; i < measures; i++)
            {
                var rhythm = rhythms[i % phraseLength];
                var pos = 0;

                for (var j = 0; j < rhythm.Length; j++)
                {
                    var value = strongBeats.Contains(pos) ? 0 : 1;

                    result.AddNote(i, new Note(value, rhythm[j], pos));

                    pos += rhythm[j];
                }
            }

            return result;
        }

        private int[] MakeRhythm(int measureLength, int step, int[] strongBeats, MeasureType where)
        {
            var result = new List<int>();

            for (var i = 0; i < strongBeats.Length; i++)
            {
                var t = strongBeats[i];
                var stepsToNext = RhythmTools.BeatsInStrongBeat(i, strongBeats, measureLength, step);

                if (where == MeasureType.Closing && i == strongBeats.Length - 1)
                {
                    result.Add(stepsToNext * step);
                    continue;
                }

                switch (stepsToNext)
                {
                    case 2:
                        var patterns2 = DuplePatterns.Where(p => (p.Type & where) > 0).ToArray();
                        AddRandomPattern(result, rand.SelectRandomly(patterns2), step / 4);
                        break;
                    case 3:
                        var patterns3 = TriplePatterns.Where(p => (p.Type & where) > 0).ToArray();
                        AddRandomPattern(result, rand.SelectRandomly(patterns3), step / 4);
                        break;
                    default:
                        result.Add(stepsToNext * step);
                        break;
                }

            }

            return result.ToArray();
        }

        private void AddRandomPattern(List<int> result, Pattern pattern, int step)
        {
            foreach (var n in pattern.Notes)
            {
                result.Add(n * step);
            }
        }

        private enum MeasureType
        {
            Opening = 1,
            Middle = 2,
            Closing = 4,

            NotOpening = 6,
            NotClosing = 3,

            Any = 7
        }

        private class Pattern
        {
            public MeasureType Type { get; }
            public int[] Notes { get; }

            public Pattern(MeasureType type, params int[] notes)
            {
                Type = type;
                Notes = notes;
            }
        }
    }
}
