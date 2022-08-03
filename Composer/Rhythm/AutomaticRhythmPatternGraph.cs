namespace Composer
{
    public class AutomaticRhythmPatternGraph : PatternGraphBase
    {
        private const double close = 0.6;
        private const double distant = 0.2;

        public AutomaticRhythmPatternGraph()
            : base()
        {
            GeneratePatterns(PatternGraphRhythm.DuplePatternLength);
            GeneratePatterns(PatternGraphRhythm.TriplePatternLength);
        }

        private void GeneratePatterns(int patternLength)
        {
            var toProcess = new Queue<RhythmicPattern>();
            var processed = new List<RhythmicPattern>();
            var initial = new RhythmicPattern(0, patternLength);
            toProcess.Enqueue(initial);
            AddPattern(initial);

            var beatHierarchy = CreateBeatHierarchy(patternLength);

            while (toProcess.Any())
            {
                var current = toProcess.Dequeue();

                if (processed.Contains(current))
                {
                    continue;
                }

                processed.Add(current);

                var derived = DerivePatterns(current, beatHierarchy);
                foreach (var pattern in derived)
                {
                    toProcess.Enqueue(pattern);
                }
            }
        }

        private IEnumerable<RhythmicPattern> DerivePatterns(RhythmicPattern current, double[] beatHierarchy)
        {
            for (var i = 0; i < current.Notes.Length; i++)
            {
                var len = current.Notes[i];

                if (len % 2 == 0)
                {
                    var p = WithOneReplaced(current, i, beatHierarchy, len / 2, len / 2);
                    AddSimilarity(current, p, close);
                    yield return p;
                }

                if (len % 3 == 0)
                {
                    var p = WithOneReplaced(current, i, beatHierarchy, 2 * len / 3, len / 3);
                    AddSimilarity(current, p, close);
                    yield return p;

                    p = WithOneReplaced(current, i, beatHierarchy, len / 3, 2 * len / 3);
                    AddSimilarity(current, p, distant);
                    yield return p;
                }

                if (len % 4 == 0)
                {
                    var p = WithOneReplaced(current, i, beatHierarchy, 3 * len / 4, len / 4);
                    AddSimilarity(current, p, close);
                    yield return p;

                    /*p = WithOneReplaced(current, i, beatHierarchy, len / 4, 3 * len / 4);
                    AddSimilarity(current, p, distant);
                    yield return p;*/
                }

                /*if (len % 7 == 0)
                {
                    var p = WithOneReplaced(current, i, beatHierarchy, 6 * len / 7, len / 7);
                    AddSimilarity(current, p, close);
                    yield return p;

                    p = WithOneReplaced(current, i, beatHierarchy, 4 * len / 3, 3 * len / 7);
                    AddSimilarity(current, p, close);
                    yield return p;
                }*/
            }

            /*for (var i = 0; i < current.Notes.Length - 1; i++)
            {
                var len = current.Notes[i];
                var next = current.Notes[i + 1];

                if (len % 2 == 0 && len == next)
                {
                    var p = WithTwoReplaced(current, i, beatHierarchy, 3 * len / 2, len / 2);
                    AddSimilarity(current, p, close);
                    yield return p;

                    p = WithTwoReplaced(current, i, beatHierarchy, len / 2, 3 * len / 2);
                    AddSimilarity(current, p, distant);
                    yield return p;
                }

                if (len % 2 == 0 && len / 2 == next)
                {
                    var p = WithTwoReplaced(current, i, beatHierarchy, next, len);
                    AddSimilarity(current, p, distant);
                    yield return p;
                }

                if (len % 3 == 0 && len == next)
                {
                    var p = WithTwoReplaced(current, i, beatHierarchy, 4 * len / 3, 2 * len / 3);
                    AddSimilarity(current, p, close);
                    yield return p;

                    p = WithTwoReplaced(current, i, beatHierarchy, 2 * len / 3, 4 * len / 3);
                    AddSimilarity(current, p, distant);
                    yield return p;
                }

                if (len % 4 == 0 && len == next)
                {
                    var p = WithTwoReplaced(current, i, beatHierarchy, 5 * len / 4, 3 * len / 4);
                    p = WithOneReplaced(p, i, beatHierarchy, 3 * len / 4, 2 * len / 4);
                    AddSimilarity(current, p, distant);
                    yield return p;
                }
            }*/
        }

        private RhythmicPattern WithOneReplaced(RhythmicPattern source, int index, double[] beatHierarchy, int first, int second)
        {
            if (first + second != source.Notes[index])
            {
                throw new InvalidOperationException();
            }

            var notes = new List<int>(source.Notes);
            notes.RemoveAt(index);
            notes.Insert(index, first);
            notes.Insert(index + 1, second);
            var energy = CalculateEnergy(notes, beatHierarchy);
            return new RhythmicPattern(energy, notes);
        }

        private RhythmicPattern WithTwoReplaced(RhythmicPattern source, int index, double[] beatHierarchy, int first, int second)
        {
            if (first + second != source.Notes[index] + source.Notes[index + 1])
            {
                throw new InvalidOperationException();
            }

            var notes = new List<int>(source.Notes);
            notes[index] = first;
            notes[index + 1] = second;
            var energy = CalculateEnergy(notes, beatHierarchy);
            return new RhythmicPattern(energy, notes);
        }

        double CalculateEnergy(IEnumerable<int> notes, double[] beatHierarchy)
        {
            var pos = 0;
            var result = 0.0;
            foreach (var note in notes)
            {
                result += beatHierarchy[pos];
                pos += note;
            }
            return result;
        }

        private double[] CreateBeatHierarchy(int patternLength)
        {
            // TODO: Implement generic logic for this

            if (patternLength == 8)
            {
                return new[] { 0, 4, 2, 2, 0.5, 4, 1, 2 };
            }

            if (patternLength == 12)
            {
                return new[] { 0, 4, 2, 2, 0.7, 4, 2, 3, 0.5, 4, 1, 2 };
            }

            return Enumerable.Repeat(1.0, patternLength).ToArray();
        }
    }
}
