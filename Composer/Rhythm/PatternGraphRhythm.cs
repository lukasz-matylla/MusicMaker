using MusicCore;
using Tools;

namespace Composer
{
    public class PatternGraphRhythm : IRhythmicPatternGenerator
    {
        public const int DuplePatternLength = 8;
        public const int TriplePatternLength = 12;

        private const int CutIntoSectionsThreshold = 8;

        private readonly IRhythmicPatternGraph graph;
        private readonly double temperature;
        private readonly int preferredPatternNumber;
        private readonly double avoidRepeats;

        private readonly Random rand;
        private readonly int[] noteValues;

        public PatternGraphRhythm(
            IRhythmicPatternGraph graph,
            double temperature = 1.0,
            int preferredPatternNumber = 2,
            double avoidRepeats = 0.3)
        {
            this.graph = graph;
            this.temperature = temperature;
            this.preferredPatternNumber = preferredPatternNumber;
            this.avoidRepeats = avoidRepeats;

            rand = new Random();
            noteValues = Enum.GetValues<NoteValue>().Cast<int>().ToArray();
        }

        public Staff CreateRhythm(int measures, Meter meter)
        {
            var result = new Staff(meter: meter, measuresCount: measures);

            var usedPatterns = new List<RhythmicPattern>();

            var strongBeats = RhythmTools.GetStrongBeats(meter);
            var phraseLength = measures > CutIntoSectionsThreshold ?
                RhythmTools.SectionLength(measures) :
                measures;

            var step = meter.BeatLength;
            if (meter.Top == 2)
            {
                step /= 2;
            }

            var rhythms = new List<int[]>();
            rhythms.Add(MakeRhythm(meter.MeasureLength, step, usedPatterns, strongBeats, MeasureType.Opening));
            for (var i = 1; i < phraseLength - 1; i++)
            {
                rhythms.Add(MakeRhythm(meter.MeasureLength, step, usedPatterns, strongBeats, MeasureType.Middle));
            }
            rhythms.Add(MakeRhythm(meter.MeasureLength, step, usedPatterns, strongBeats, MeasureType.Closing));

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

        private int[] MakeRhythm(int measureLength, int step, List<RhythmicPattern> usedPatterns, int[] strongBeats, MeasureType where)
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

                RhythmicPattern selectedElement;

                switch (stepsToNext)
                {
                    case 2:
                        selectedElement = SelectElement(usedPatterns, where, false);
                        AddElement(result, selectedElement, step / 4);
                        break;
                    case 3:
                        selectedElement = SelectElement(usedPatterns, where, true);
                        AddElement(result, selectedElement, step / 4);
                        break;
                    default:
                        result.Add(stepsToNext * step);
                        break;
                }

            }

            return result.ToArray();
        }

        private RhythmicPattern SelectElement(List<RhythmicPattern> usedPatterns, MeasureType where, bool isTriple)
        {
            var temp = where switch
            {
                MeasureType.Opening => temperature,
                MeasureType.Middle => temperature * 1.5,
                MeasureType.Closing => temperature * 0.5,
                _ => throw new NotImplementedException()
            };

            var patternLength = isTriple ? TriplePatternLength : DuplePatternLength;

            var availablePatterns = graph.Patterns
                .Where(e => e.Length == patternLength)
                .ToArray();

            var weights = availablePatterns
                .Select(e => Math.Exp(-(e.Energy / temp)* (e.Energy / temp)))
                .ToArray();

            if (usedPatterns.Count < preferredPatternNumber)
            {
                var weightsToAvoidRepeats = availablePatterns
                    .Select(e => usedPatterns.Contains(e) ? avoidRepeats : 1.0)
                    .ToArray();
                weights = weights.Mult(weightsToAvoidRepeats);
            }
            else
            {
                var weightsToPreferSimilar = availablePatterns
                    .Select(e => usedPatterns.Contains(e) ? 1.0 : usedPatterns.Max(p => graph.Similarity(e, p)))
                    .ToArray();
                weights = weights.Mult(weightsToPreferSimilar);
            }

            var result = availablePatterns.SelectRandomly(weights, rand);

            if (!usedPatterns.Contains(result))
            {
                usedPatterns.Add(result);
            }
            
            return result;
        }

        private void AddElement(List<int> result, RhythmicPattern element, int step)
        {
            foreach (var n in element.Notes)
            {
                var len = n * step;
                
                while (len > 0)
                {
                    var longestNote = noteValues.Where(l => l <= len).Max();
                    result.Add(longestNote);
                    len -= longestNote;
                }
            }
        }
    }
}
