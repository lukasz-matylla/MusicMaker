using MusicCore;
using Tools;

namespace Composer
{
    public static class RhythmTools
    {
        public static int[] GetStrongBeats(Meter meter)
        {
            if (meter.IsDuple)
            {
                return new[] { 0, meter.MeasureLength / 2 };
            }

            if (meter.IsTriple)
            {
                return new[] { 0 };
            }

            var i = 0;
            var result = new List<int>();
            while (i < meter.Top)
            {
                result.Add(i * meter.BeatLength);

                if (meter.Top - i > 4)
                {
                    i += 3;
                }
                else
                {
                    i += 2;
                }
            }
            return result.ToArray();
        }

        public static int BeatsInStrongBeat(int n, int[] strongBeats, int measureLength, int step)
        {
            var length = n < strongBeats.Length - 1 ?
                strongBeats[n + 1] - strongBeats[n] :
                measureLength - strongBeats[n];

            return length / step;
        }

        public static int SectionLength(int measures, int maxSize)
        {
            var divisors = measures.Factors();
            return divisors.Where(d => d <= maxSize).Last();
        }
    }

    public enum MeasureType
    {
        Opening,
        Middle,
        Closing,
    }
}
