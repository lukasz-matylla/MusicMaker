namespace MusicCore
{
    public enum Key
    {
        C = 0,
        G = -5,
        D = 2,
        A = -3, 
        E = 4,
        B = -1,
        Gb = -6,
        Db = 1,
        Ab = -4,
        Eb = 3,
        Bb = -2,
        F = 5
    }

    public static class KeyExtensions
    {
        public static ScaleStep ToCMajorNote(this Key key)
        {
            return key switch
            {
                Key.C => new ScaleStep(0),
                Key.G => new ScaleStep(4, octave: -1),
                Key.D => new ScaleStep(1),
                Key.A => new ScaleStep(5, octave: -1),
                Key.E => new ScaleStep(2),
                Key.B => new ScaleStep(6, octave: -1),
                Key.Gb => new ScaleStep(4, accidental: Accidental.Flat, octave: -1),
                Key.Db => new ScaleStep(1, accidental: Accidental.Flat),
                Key.Ab => new ScaleStep(5, accidental: Accidental.Flat, octave: -1),
                Key.Eb => new ScaleStep(2, accidental: Accidental.Flat),
                Key.Bb => new ScaleStep(6, accidental: Accidental.Flat, octave: -1),
                Key.F => new ScaleStep(3),
                _ => throw new ArgumentOutOfRangeException(nameof(key)),
            };
        }
    }
}
