using MusicCore;
using Tools;

namespace Composer
{
    public class GraphBasedChordProgression : IChordProgressionGenerator
    {
        private readonly IChordTransitionGraph transitions;
        private readonly Random rand;

        private readonly Chord I = new Chord(0, 2, 4);
        private readonly Chord V = new Chord(4, 6, 1);

        public GraphBasedChordProgression(IChordTransitionGraph transitions)
        {
            this.transitions = transitions;
            rand = new Random();
        }

        public Chord[] GenerateProgression(int length, CadenceType cadence = CadenceType.Strong)
        {
            var buffer = new Chord[length];
            GenerateProgression(buffer, cadence, 0, length);
            return buffer;
        }

        private void GenerateProgression(Chord[] buffer, CadenceType cadence = CadenceType.Strong, int start = 0, int count = -1)
        {
            if (count < 0)
            {
                count = buffer.Length - start;
            }

            if (count <= 0)
            {
                return;
            }

            if (start < 0 || start + count > buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            switch (cadence)
            {
                case CadenceType.Strong:
                    buffer[start] = I;
                    buffer[start + count - 1] = I;
                    GenerateBackward(buffer, start + count - 1, count - 3);
                    FillBetween(buffer, start + 1, buffer[start], buffer[start + 2]);
                    break;

                case CadenceType.Half:
                case CadenceType.Weak:
                    GenerateProgression(buffer, CadenceType.Strong, start, count - 1);
                    buffer[start + count - 1] = V;
                    break;

                case CadenceType.None:
                    buffer[start] = I;
                    GenerateForward(buffer, start + 1, count - 1);
                    break;

                case CadenceType.Loop:
                    var tempBuffer = new Chord[count];
                    tempBuffer[count - 1] = I;
                    GenerateBackward(tempBuffer, count - 2, count - 1);
                    buffer[start] = I;
                    for (var i = 1; i < count; i++)
                    {
                        buffer[start + i] = tempBuffer[i - 1];
                    }
                    break;
            }
        }

        private void FillBetween(Chord[] buffer, int pos, Chord before, Chord after)
        {
            var weights = transitions.WeightsFrom(before)
                .Mult(transitions.WeightsTo(after));
            var chord = transitions.Chords.SelectRandomly(weights, rand);
            buffer[pos] = chord;
        }

        private void GenerateBackward(Chord[] buffer, int start, int count)
        {
            for (var i = 0; i < count; i++)
            {
                var chord = transitions.Chords.SelectRandomly(transitions.WeightsTo(buffer[start - i]), rand);
                buffer[start - 1 - i] = chord;
            }
        }

        private void GenerateForward(Chord[] buffer, int start, int count)
        {
            for (var i = 0; i < count; i++)
            {
                var chord = transitions.Chords.SelectRandomly(transitions.WeightsFrom(buffer[start - 1 + i]), rand);
                buffer[start + i] = chord;
            }
        }
    }
}
