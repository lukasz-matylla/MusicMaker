namespace MusicCore
{
    public class ScaleStep : IEquatable<ScaleStep>
    {
        public int Step { get; }
        public Accidental Accidental { get; }
        public int Octave { get; }

        public ScaleStep(int step, Accidental modifier = Accidental.None, int octave = 0)
        {
            Step = step;
            Accidental = modifier;
            Octave = octave;
        }

        public override string ToString()
        {
            var notePart = (Step+1).ToString();
            var modifierPart = Accidental switch
            {
                Accidental.None => string.Empty,
                Accidental.Flat => "b",
                Accidental.Sharp => "#",
                Accidental.DoubleFlat => "bb",
                Accidental.DoubleSharp => "##",
                _ => throw new ArgumentOutOfRangeException(nameof(Accidental))
            };
            var octavePart = Octave == 0 ? string.Empty :
                Octave > 0 ? $"+{Octave}" : $"{Octave}";

            return $"{notePart}{modifierPart}{octavePart}";
        }

        public bool Equals(ScaleStep? other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.Step == Step &&
                other.Accidental == Accidental &&
                other.Octave == Octave;
        }

        public static implicit operator ScaleStep(int step) => new ScaleStep(step, Accidental.None, 0);
    }
}