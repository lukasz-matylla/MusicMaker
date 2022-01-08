using System.Diagnostics;

namespace Tools
{
    public static class RandomDistribution
    {
        public static double[] Mult(this double[] v, double c)
        {
            return v.Select(x => x * c).ToArray();
        }

        public static double[] Mult(this double[] v, double[] w)
        {
            return Enumerable.Range(0, v.Length).Select(i => v[i] * w[i]).ToArray();
        }

        public static double[] Add(this double[] v, double[] w)
        {
            return Enumerable.Range(0, v.Length).Select(i => v[i] + w[i]).ToArray();
        }

        public static T SelectRandomly<T>(this IReadOnlyList<T> items, Random r)
        {
            return items[r.Next(items.Count)];
        }

        public static T SelectRandomly<T>(this IReadOnlyList<T> items, double[] weights, Random r)
        {
            var sum = weights.Sum();
            if (sum == 0)
            {
                Debug.WriteLine("All weights are zero");
                return items.SelectRandomly(r);
            }

            var x = r.NextDouble() * sum;
            for (var i = 0; i < weights.Length; i++)
            {
                if (x < weights[i])
                {
                    return items[i];
                }

                x -= weights[i];
            }

            return items.Last();
        }
    }
}
