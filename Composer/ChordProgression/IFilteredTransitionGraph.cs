namespace Composer.ChordProgression
{
    public interface IFilteredTransitionGraph<TItem>
    {
        IReadOnlyList<TItem> Items { get; }
        IReadOnlyList<TItem> FilteredItems(Func<TItem, bool>? filter = null);
        double[] WeightsFrom(TItem item, Func<TItem, bool>? filter = null);
        double[] WeightsTo(TItem item, Func<TItem, bool>? filter = null);
    }
}
