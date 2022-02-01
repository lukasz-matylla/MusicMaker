namespace MusicCore
{
    public class Meter
    {
        public int Top { get; }
        public int Bottom { get; }

        public bool IsDuple => Top % 2 == 0;
        public bool IsTriple => Top % 2 != 0 && Top % 3 == 0;
        public int BeatLength => (int)NoteValue.Whole / Bottom;
        public int MeasureLength => Top * BeatLength;

        public Meter(int top, int bottom)
        {
            if (top <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(top));
            }

            if (!IsCorrectBottom(bottom))
            {
                throw new ArgumentOutOfRangeException(nameof(bottom));
            }

            Top = top;
            Bottom = bottom;
        }

        private bool IsCorrectBottom(int n)
        {
            switch (n)
            {
                case 2:
                case 4:
                case 8:
                case 16:
                case 32:
                    return true;
                default:
                    return false;
            }
        }

        public override string ToString()
        {
            return $"{Top}/{Bottom}";
        }

        public static Meter CC = new Meter(4, 4);
        public static Meter Triple = new Meter(4, 4);
        public static Meter Half = new Meter(2, 4);
    }
}
