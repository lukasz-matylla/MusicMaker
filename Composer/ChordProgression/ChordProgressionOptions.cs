namespace Composer.ChordProgression
{
    public class ChordProgressionOptions
    {
        public bool UseInversions { get; init; } = true;
        public bool ForceNaturalMinor { get; init; } = false;
        public bool UsePiccardy { get; init; } = true;
        public bool UseSecondaryDominants { get; init; } = false; 
        public bool UseBorrowedChords { get; init; } = false; // includes IV in minor, iv in major, v in major etc.
        public bool UseSuspended { get; init; } = false;
        public bool UseNeapolitan { get; init; } = false;
        public bool UseAugmentedSixth { get; init; } = false;
        public bool UseFullyDiminished { get; init; } = false;
        public bool UseAugmentedChords { get; init; } = false;
        public bool UseChromaticMediants { get; init; } = false;
        public bool UseTritoneSubstitution { get; init; } = false;
        public bool UseAlteredHarmony { get; init; } = false;


        public bool AvoidExcessiveInversions { get; init; } = true; // second inversions only as neighbor/passing/cadential


        public int MaxPhraseLength { get; init; } = 8;
        public double HalfCadencePhraseEnding { get; init; } = 0.5;
        
        public ChordProgressionOptions(ChromaticApproach chromaticApproach = ChromaticApproach.StrictlyDiatonic)
        {
            switch (chromaticApproach)
            {
                case ChromaticApproach.StrictlyDiatonic:
                    break;
                case ChromaticApproach.MostlyDiatonic:
                    UseSecondaryDominants = true;
                    UseBorrowedChords = true;
                    UseNeapolitan = true;
                    break;
                case ChromaticApproach.MostlyChromatic:
                    UseSecondaryDominants = true;
                    UseBorrowedChords = true;
                    UseNeapolitan = true;
                    UseAugmentedSixth = true;
                    UseFullyDiminished = true;
                    UseAugmentedChords = true;
                    break;
                case ChromaticApproach.StrictlyChromatic:
                    UseSecondaryDominants = true;
                    UseBorrowedChords = true;
                    UseNeapolitan = true;
                    UseAugmentedSixth = true;
                    UseFullyDiminished = true;
                    UseAugmentedChords = true;
                    UseChromaticMediants = true;
                    UseTritoneSubstitution = true;
                    UseAlteredHarmony = true;
                    break;
            }
        }
    }
}
