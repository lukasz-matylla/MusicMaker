namespace Tools
{
    public static class SequenceOptimizer
    {
        public static int[][] FindOptimal(int length, int range, Func<int[], double> eval)
        {
            double bestValue = double.MinValue;
            List<int[]> bestSequences = new List<int[]>();

            var possible = AllSequences(length, range);

            foreach (var sequence in possible)
            {
                var v = eval(sequence);

                if (v > bestValue)
                {
                    bestSequences.Clear();
                    bestValue = v;
                }

                if (v == bestValue)
                {
                    bestSequences.Add(sequence);
                }
            }

            return bestSequences.ToArray();
        }

        public static IEnumerable<int[]> AllSequences(int length, int range)
        {
            var current = new int[length];
            
            while (true)
            {
                yield return (int[])current.Clone();

                for (var i = 0; i < length; i++)
                {
                    if (current[i] < range - 1)
                    {
                        current[i]++;
                        break;
                    }

                    current[i] = 0;

                    if (i == length - 1)
                    {
                        yield break;
                    }
                }
            }
        }
    }
}
