namespace Composer.ChordProgression
{
    [Flags]
    internal enum HarmonicFunction
    {
        None = 0,
        TonicInitial = 1,
        Tonic = 2,
        TonicFinal = 4,
        PredominantInitial = 8,
        Predominant = 16,
        PredominantFinal = 32,
        DominantInitial = 64,
        Dominant = 128,
        DominantFinal = 256,
        DominantStrong = 512,
        DominantSolo = 1024,
        AnyTonic = TonicInitial | Tonic | TonicFinal,
        AnyPredominant = PredominantInitial | Predominant | PredominantFinal,
        AnyDominant = DominantInitial | Dominant | DominantFinal,
        Any = AnyTonic | AnyPredominant | AnyDominant
    }
}
