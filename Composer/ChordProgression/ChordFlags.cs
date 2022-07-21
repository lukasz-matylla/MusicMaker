namespace Composer.ChordProgression
{
    [Flags]
    internal enum ChordFlags
    {
        Diatonic = 0,
        ExtendedMinor = 1,
        SecondaryDominant = 2,
        Borrowed = 4,
        Neapolitan = 8,
        AugmentedSixth = 16,
        ChromaticMediant = 128,
        TritoneSubstitution = 256,
        Altered = 512,
    }
}
