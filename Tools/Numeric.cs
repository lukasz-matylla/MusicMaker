namespace Tools
{
    public static class Numeric
    {
        public static int WrapTo(this int n, int scale)
        {
            var result = n % scale;

            return result >= 0 ?
                result :
                result + scale;
        }

        public static int Limit(this int n, int lower, int upper)
        {
            if (n > upper)
            {
                return upper;
            }
            if (n < lower)
            {
                return lower;
            }
            return n;
        }

        public static int[] Factors(this int n)
        {
            return Enumerable.Range(1, n)
                .Where(k => n % k == 0)
                .ToArray();
        }

        public static IDictionary<int, int> Factorize(this int n)
        {
            var result = new Dictionary<int, int>();

            for (var i = 2; i < n; i++)
            {
                if (n % i == 0)
                {
                    n /= i;

                    result[i] = 1 + result.GetValueOrDefault(i);
                }
            }

            return result;
        }
    }
}
