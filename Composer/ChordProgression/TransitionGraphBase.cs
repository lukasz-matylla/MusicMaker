using Tools;

namespace Composer.ChordProgression
{
    public class TransitionGraphBase<TItem> : IFilteredTransitionGraph<TItem>
    {
        protected readonly List<TItem> items;
        protected readonly List<GraphTransition> transitions;

        #region ITransitionGraph

        public IReadOnlyList<TItem> Items => items;

        public IReadOnlyList<TItem> FilteredItems(Func<TItem, bool>? filter = null)
        {
            if (filter == null)
            {
                return items;
            }

            return items.Where(i => filter(i)).ToArray();
        }

        public double[] WeightsFrom(TItem item, Func<TItem, bool>? filter = null)
        {
            return WeightsFrom(FindItemIndex(item), filter ?? (_ => true));
        }

        public double[] WeightsTo(TItem item, Func<TItem, bool>? filter = null)
        {
            return WeightsTo(FindItemIndex(item), filter ?? (_ => true));
        }

        #endregion

        protected TransitionGraphBase()
        {
            items = new List<TItem>();
            transitions = new List<GraphTransition>();
        }

        private double[] WeightsFrom(int item, Func<TItem, bool> filter)
        {
            return Enumerable.Range(0, items.Count)
                .Select(i => filter(items[i]) ? GetWeight(item, i) : 0)
                .ToArray();
        }

        private double[] WeightsTo(int item, Func<TItem, bool> filter)
        {
            return Enumerable.Range(0, items.Count)
                .Select(i => filter(items[i]) ? GetWeight(i, item) : 0)
                .ToArray();
        }

        private double GetWeight(int from, int to)
        {
            var transition = transitions.FirstOrDefault(t => t.From == from && t.To == to);
            return transition?.Weight ?? 0;
        }

        protected int FindItemIndex(TItem? item)
        {
            if (item == null)
            {
                return -1;
            }

            return items.FindIndex(x => IsEquivalent(x, item));
        }

        protected virtual bool IsEquivalent(TItem x, TItem y)
        {
            if (x == null || y == null)
            {
                throw new ArgumentNullException();
            }

            return x.Equals(y);
        }

        protected void AddTransition(TItem from, TItem to, double weight = 1)
        {
            var fromIndex = EnsureItem(from);
            var toIndex = EnsureItem(to);

            var transitionIndex = transitions.FindIndex(t => t.From == fromIndex && t.To == toIndex);

            if (transitionIndex >= 0)
            {
                transitions[transitionIndex].Weight = weight;
            }
            else
            {
                transitions.Add(new GraphTransition(fromIndex, toIndex, weight));
            }
        }

        protected int EnsureItem(TItem item)
        {
            var index = FindItemIndex(item);

            if (index == -1)
            {
                index = items.Count;
                items.Add(item);
            }

            return index;
        }

        
    }
}
