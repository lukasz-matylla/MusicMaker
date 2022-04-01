using Tools;

namespace Composer
{
    public abstract class PatternGraphBase : IRhythmicPatternGraph
    {
        public IReadOnlyList<RhythmicPattern> Patterns => patterns;

        protected readonly List<RhythmicPattern> patterns;
        protected readonly List<GraphTransition> similarities;


        protected PatternGraphBase()
        {
            patterns = new List<RhythmicPattern>();
            similarities = new List<GraphTransition>();
        }

        public double Similarity(int from, int to)
        {
            var similarity = similarities.FirstOrDefault(t => t.From == from && t.To == to);
            return similarity?.Weight ?? 0;
        }

        public double Similarity(RhythmicPattern from, RhythmicPattern to)
        {
            var fromIndex = FindPatternIndex(from);
            var toIndex = FindPatternIndex(to);

            var similarity = similarities.FirstOrDefault(t => t.From == fromIndex && t.To == toIndex);
            return similarity?.Weight ?? 0;
        }

        protected int FindPatternIndex(RhythmicPattern pattern)
        {
            return patterns.FindIndex(e => e.Equals(pattern));
        }

        protected int AddPattern(RhythmicPattern pattern)
        {
            var index = FindPatternIndex(pattern);

            if (index == -1)
            {
                index = patterns.Count;
                patterns.Add(pattern);
            }

            return index;
        }

        protected void AddSimilarity(RhythmicPattern from, RhythmicPattern to, double weight = 1)
        {
            var fromIndex = AddPattern(from);
            var toIndex = AddPattern(to);

            var similarityIndex1 = similarities.FindIndex(t => t.From == fromIndex && t.To == toIndex);
            var similarityIndex2 = similarities.FindIndex(t => t.From == fromIndex && t.To == toIndex);

            if (similarityIndex1 >= 0 && similarityIndex1 >= 0)
            {
                similarities[similarityIndex1].Weight = weight;
                similarities[similarityIndex2].Weight = weight;
            }
            else
            {
                similarities.Add(new GraphTransition(fromIndex, toIndex, weight));
                similarities.Add(new GraphTransition(toIndex, fromIndex, weight));
            }
        }
    }
}
