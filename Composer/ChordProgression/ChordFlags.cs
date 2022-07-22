namespace Composer.ChordProgression
{
    [Flags]
    internal enum ChordFlags
    {
        Diatonic = 0,
        MelodicMinor = 1,
        SecondaryDominant = 2,
        Borrowed = 4,
        Neapolitan = 8,
        Extended = 16,
        ChromaticMediant = 32,
        TritoneSubstitution = 64,
        Altered = 128,
    }
}
