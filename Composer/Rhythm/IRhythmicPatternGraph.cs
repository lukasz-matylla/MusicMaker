namespace Composer
{
    public interface IRhythmicPatternGraph
    {
        IReadOnlyList<RhythmicPattern> Patterns { get; }
        double Similarity(int from, int to);
        double Similarity(RhythmicPattern from, RhythmicPattern to);
    }
}
