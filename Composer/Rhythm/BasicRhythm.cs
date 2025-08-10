using MusicCore;
using Tools;

namespace Composer
{
    public class BasicRhythm : IRhythmicPatternGenerator
    {
        private const int CutIntoSectionsThreshold = 8;
        private const int MinMotiveLength = 2;

        private readonly Random rand;
        private readonly double temperature;

        private readonly Pattern[] DuplePatterns =
        [
            new Pattern(MeasureType.Any, 8),

            new Pattern(MeasureType.Any, 4, 4),
            new Pattern(MeasureType.Any, 4, 4),
            new Pattern(MeasureType.Any, 4, 4),
            new Pattern(MeasureType.Any, 4, 4),

            new Pattern(MeasureType.Any, 4, 2, 2),
            new Pattern(MeasureType.Any, 4, 2, 2),
            new Pattern(MeasureType.Any, 4, 2, 2),

            new Pattern(MeasureType.Any, 6, 2),
            new Pattern(MeasureType.Any, 6, 2),          

            new Pattern(MeasureType.Middle, 2, 2, 2, 2),
            new Pattern(MeasureType.Middle, 2, 2, 2, 2),

            new Pattern(MeasureType.Middle, 2, 6),

            new Pattern(MeasureType.Middle, 2, 2, 4),

            new Pattern(MeasureType.Middle, 2, 4, 2),
        ];

        private readonly Pattern[] TriplePatterns =
        [
            new Pattern(MeasureType.Any, 12),

            new Pattern(MeasureType.Any, 8, 4),
            new Pattern(MeasureType.Any, 8, 4),

            new Pattern(MeasureType.Any, 8, 2, 2),
            new Pattern(MeasureType.Any, 8, 2, 2),

            new Pattern(MeasureType.Any, 4, 4, 2, 2),
            new Pattern(MeasureType.Any, 4, 4, 2, 2),
            new Pattern(MeasureType.Any, 4, 4, 2, 2),
            new Pattern(MeasureType.Any, 4, 4, 2, 2),

            new Pattern(MeasureType.Middle, 4, 2, 2, 4),
            new Pattern(MeasureType.Middle, 4, 2, 2, 4),
            new Pattern(MeasureType.Middle, 4, 2, 2, 4),

            new Pattern(MeasureType.Middle, 4, 2, 2, 2, 2),
            new Pattern(MeasureType.Middle, 4, 2, 2, 2, 2),
            new Pattern(MeasureType.Middle, 4, 2, 2, 2, 2),

            new Pattern(MeasureType.Middle, 4, 8),
            new Pattern(MeasureType.Middle, 4, 8),

            new Pattern(MeasureType.Middle, 2, 2, 8),

            new Pattern(MeasureType.Middle, 2, 2, 4, 4),

            new Pattern(MeasureType.Middle, 2, 2, 4, 2, 2),

            new Pattern(MeasureType.Middle, 2, 2, 2, 2, 2, 2),
        ];

        public BasicRhythm(double temperature = 0)
        {
            rand = new Random();
            this.temperature = temperature;
        }

        public Staff CreateRhythm(int measures, Meter meter, Staff? ignore = null)
        {
            var result = new Staff(meter: meter, measuresCount: measures);

            var strongBeats = RhythmTools.GetStrongBeats(meter);
            var step = meter.BeatLength;
            if (meter.Top < 3)
            {
                step /= 2;
            }

            var phraseLength = measures > CutIntoSectionsThreshold ?
                RhythmTools.SectionLength(measures, CutIntoSectionsThreshold) :
                measures;

            var substructure = phraseLength.Factors()
                .Where(f => f >= MinMotiveLength && f <= phraseLength / 2)
                .ToList();

            var rhythms = new List<int[]>
            {
                MakeRhythm(meter.MeasureLength, step, strongBeats, MeasureType.Opening)
            };

            if (substructure.Count == 0)
            {
                for (var i = 1; i < phraseLength - 1; i++)
                {
                    rhythms.Add(MakeRhythm(meter.MeasureLength, step, strongBeats, MeasureType.Middle));
                }
            }
            else
            {
                var motiveLength = substructure.Last();
                var motives = phraseLength / motiveLength;
                for (var i = 1; i < motiveLength; i++)
                {
                    rhythms.Add(MakeRhythm(meter.MeasureLength, step, strongBeats, MeasureType.Middle));
                }

                for (var j = 1; j < motives - 1; j++)
                {
                    for (var i = 0; i < motiveLength; i++)
                    {
                        if (rand.NextDouble() < temperature)
                        {
                            rhythms.Add(MakeRhythm(meter.MeasureLength, step, strongBeats, MeasureType.Middle));
                        }
                        else
                        {
                            rhythms.Add(rhythms[i]);
                        }
                    }
                }

                for (var i = 0; i < motiveLength - 1; i++)
                {
                    if (rand.NextDouble() < temperature)
                    {
                        rhythms.Add(MakeRhythm(meter.MeasureLength, step, strongBeats, MeasureType.Middle));
                    }
                    else
                    {
                        rhythms.Add(rhythms[i]);
                    }
                }
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
