using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace MusicCore
{
    public class ScaleInterval : IEquatable<ScaleInterval>, IComparable<ScaleInterval>
    {
        public int Steps { get; }
        public int Halftones { get; }

        public ScaleInterval(int steps, int halftones)
        {
            Steps = steps;
            Halftones = halftones;
        }

        public ScaleInterval Inverted => new ScaleInterval(-Steps, -Halftones);

        public ScaleInterval Abs => Steps >= 0 ? this : Inverted;

        public ScaleInterval Normalized(int scaleSize) => new ScaleInterval(Steps.WrapTo(scaleSize), Halftones.WrapTo(12));

        public static readonly ScaleInterval Unison = new ScaleInterval(0, 0);

        public static readonly ScaleInterval MinorSecond = new ScaleInterval(1, 1);
        public static readonly ScaleInterval MajorSecond = new ScaleInterval(1, 2);

        public static readonly ScaleInterval DiminishedThird = new ScaleInterval(2, 2);
        public static readonly ScaleInterval MinorThird = new ScaleInterval(2, 3);
        public static readonly ScaleInterval MajorThird = new ScaleInterval(2, 4);

        public static readonly ScaleInterval PerfectFourth = new ScaleInterval(3, 5);

        public static readonly ScaleInterval PerfectFifth = new ScaleInterval(4, 7);

        public static readonly ScaleInterval MinorSixth = new ScaleInterval(5, 8);
        public static readonly ScaleInterval MajorSixth = new ScaleInterval(5, 9);

        public static readonly ScaleInterval MinorSeventh = new ScaleInterval(6, 10);
        public static readonly ScaleInterval MajorSeventh = new ScaleInterval(6, 11);

        public static readonly ScaleInterval Octave = new ScaleInterval(7, 12);

        public bool Equals(ScaleInterval? other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.Steps == Steps && other.Halftones == Halftones;
        }

        public int CompareTo(ScaleInterval? other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            
            if (Steps > other.Steps)
            {
                return 1;
            }

            if (Steps < other.Steps)
            {
                return -1;
            }

            return Halftones.CompareTo(other.Halftones);
        }

        public override string ToString()
        {
            return $"({Steps}s, {Halftones}h)";
        }

        public static ScaleInterval operator + (ScaleInterval a, ScaleInterval b)
        {
            return new ScaleInterval(a.Steps + b.Steps, a.Halftones + b.Halftones);
        }

        public static ScaleInterval operator -(ScaleInterval a, ScaleInterval b)
        {
            return new ScaleInterval(a.Steps - b.Steps, a.Halftones - b.Halftones);
        }
    }
}
