namespace Tools
{
    public struct Ratio
    {
        public int Numerator { get; }
        public int Denominator { get; }

        public Ratio(int num, int denom)
        {
            Numerator = num;
            Denominator = denom;
        }

        public Ratio Simplify()
        {
            if (Numerator == 0)
            {
                return new Ratio(0, 1);
            }

            if (Denominator == 0)
            {
                return new Ratio(1, 0);
            }

            var f = Gcd(Numerator, Denominator);
            return new Ratio(Numerator / f, Denominator / f);
        }

        private int Gcd(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                {
                    a %= b;
                }
                else
                {
                    b %= a;
                }
            }

            return Math.Abs(a) + Math.Abs(b);
        }

        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }
    }
}
