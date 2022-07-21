using MusicCore;

namespace Composer.ChordProgression
{
    internal class FlaggedAbstractChord : AbstractChord
    {
        public HarmonicFunction Function { get; init; }
        public ChordFlags Flags { get; init; }

        public FlaggedAbstractChord(ScaleStep root, ChordType type = ChordType.Major, int? inversion = null) 
            : base(root, type, inversion)
        { }

        public override string ToString()
        {
            var inv = Inversion switch
            {
                null => "(any inversion)",
                0 => "(root position)",
                int n => $"(inversion {n})"
            };
            return $"{Root} {Type} {inv} [function={Function}; flags={Flags}]";
        }
    }
}
