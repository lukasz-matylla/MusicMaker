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

        public static int[] Factors(this int n)
        {
            return Enumerable.Range(1, n)
                .Where(k => n % k == 0)
                .ToArray();
        }
    }
}
