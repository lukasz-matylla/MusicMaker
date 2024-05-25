namespace Tools
{
    public static class RandomExtensions
    {
        public static int[] Partition(this Random rand, int size, int parts)
        {
            var cuts = new List<int>();
            for (var i = 0; i < parts - 1; i++)
            {
                cuts.Add(rand.Next(size));
            }
            cuts.Sort();

            var result = new int[parts];
            result[0] = cuts[0];
            for (var i = 1; i < parts - 1; i++)
            {
                result[i] = cuts[i] - cuts[i - 1];
            }
            result[parts - 1] = size - cuts[parts - 2];

            return result;
        }

        public static int[] Partition(this Random rand, int size, int parts, params int[] minSizes)
        {
            if (minSizes.Length < parts)
            {
                minSizes = minSizes.Concat(Enumerable.Repeat(0, parts - minSizes.Length)).ToArray();
            }
            else if (minSizes.Length > parts)
            {
                minSizes = minSizes.Take(parts).ToArray();
            }

            var sumSizes = minSizes.Sum();
            var toDivide = size - sumSizes;
            var partition = Partition(rand, toDivide, parts);

            for (var i = 0; i < parts; i++)
            {
                partition[i] += minSizes[i];
            }
            return partition;
        }


    }
}
