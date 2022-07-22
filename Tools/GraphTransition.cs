namespace Tools
{
    public class GraphTransition
    {
        public readonly int From;
        public readonly int To;
        public double Weight;

        public GraphTransition(int from, int to, double weight = 1)
        {
            From = from;
            To = to;
            Weight = weight;
        }

        public override string ToString()
        {
            return $"{From}->{To} (Weight)";
        }
    }
}
