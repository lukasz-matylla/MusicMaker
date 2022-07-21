using MusicCore;

namespace Composer
{
    public class BasicFunctionalMajorProgression : IChordProgressionGenerator
    {
        private static readonly Chord I = new Chord(0, 2, 4);
        private static readonly Chord II = new Chord(1, 3, 5);
        private static readonly Chord III = new Chord(2, 4, 6);
        private static readonly Chord IV = new Chord(3, 5, 0);
        private static readonly Chord V = new Chord(4, 6, 1);
        private static readonly Chord VI = new Chord(5, 0, 2);
        private static readonly Chord VII = new Chord(6, 1, 3);

        private static readonly Chord IVsus2 = new Chord(3, 4, 0);
        private static readonly Chord Vsus4 = new Chord(4, 0, 1);

        private static readonly Chord InMajor_Neapolitan = new Chord(3, new ScaleStep(5, Accidental.Flat), new ScaleStep(1, Accidental.Flat));
        private static readonly Chord InMajor_Italian = new Chord(new ScaleStep(5, Accidental.Flat), 0, new ScaleStep(3, Accidental.Sharp));
        private static readonly Chord InMajor_V7 = new Chord(4, 6, 1, 3);

        private static readonly Chord InMinor_Neapolitan = new Chord(3, 5, new ScaleStep(1, Accidental.Flat));
        private static readonly Chord InMinor_Italian = new Chord(5, 0, new ScaleStep(3, Accidental.Sharp));
        private static readonly Chord InMinor_V7 = new Chord(4, new ScaleStep(6, Accidental.Sharp), 1, 3);
        private static readonly Chord InMinor_Vmaj = new Chord(4, new ScaleStep(6, Accidental.Sharp), 1);
        private static readonly Chord InMinor_vii0 = new Chord(new ScaleStep(6, Accidental.Sharp), 1, 3);

        private static Chord[] TonicChords = new[] { I, III, VI };
        private static Chord[] SubdominantChords = new[] { II, IV };
        private static Chord[] DominantChords = new[] { V };

        private readonly Random rand;

        public MusicalScale Scale { get; private set; }

        public BasicFunctionalMajorProgression(MusicalScale scale)
        {
            rand = new Random();
            Scale = scale;

            if (scale == MusicalScale.Major)
            {
                TonicChords = new[] { I, I.Inversion(1), III, VI, VI.Inversion(1), VI.Inversion(2) };
                SubdominantChords = new[] { II, IV, IV.Inversion(1), IVsus2, InMajor_Neapolitan, InMajor_Italian };
                DominantChords = new[] { I.Inversion(2), V, V.Inversion(1), V.Inversion(2), InMajor_V7, Vsus4, VII.Inversion(1) };
            }
            else if (scale == MusicalScale.Minor)
            {
                TonicChords = new[] { I, I.Inversion(1), III, VI, VI.Inversion(1), VI.Inversion(2), VII };
                SubdominantChords = new[] { II, IV, IV.Inversion(1), IVsus2, InMinor_Neapolitan, InMinor_Italian };
                DominantChords = new[] { I.Inversion(2), V, V.Inversion(1), V.Inversion(2), InMinor_Vmaj, InMinor_Vmaj.Inversion(1),
                    InMinor_V7, InMinor_V7.Inversion(1), Vsus4, VII, InMinor_vii0.Inversion(1) };
            }
        }

        public Chord[] GenerateProgression(int length, CadenceType cadence = CadenceType.Strong)
        {
            var buffer = new Chord[length];
            GenerateProgression(buffer, cadence, 0, length);
            return buffer;
        }

        private void GenerateProgression(Chord[] buffer, CadenceType cadence, int start, int count)
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
                    GenerateTSDProgression(buffer, start, count - 2);
                    buffer[start + count - 2] = V;
                    buffer[start + count - 1] = I;
                    break;
                case CadenceType.Weak:
                    GenerateTSDProgression(buffer, start, count - 1);
                    buffer[start + count - 1] = I;
                    break;
                case CadenceType.Half:
                    GenerateProgression(buffer, CadenceType.Weak, start, count - 1);
                    buffer[start + count - 1] = V;
                    break;
                case CadenceType.Loop:
                case CadenceType.Any:
                    GenerateTSDProgression(buffer, start, count);
                    break;
            }
        }

        private void GenerateTSDProgression(Chord[] buffer, int start, int count)
        {
            var partition = Partition(count - 1);
            buffer[start] = I;
            GenerateTonic(buffer, start + 1, partition[0]);
            GenerateSubdominant(buffer, start + 1 + partition[0], partition[1]);
            GenerateDominant(buffer, start + 1 + partition[0] + partition[1], partition[2]);
        }

        private int[] Partition(int measures)
        {
            var r1 = rand.Next(measures + 1);
            var r2 = rand.Next(measures + 1);

            if (r1 < r2)
            {
                return new[] { r1, r2 - r1, measures - r2 };
            }

            return new[] { r2, r1 - r2, measures - r1 };
        }

        private Chord RandomizeFrom(Chord[] options)
        {
            var r = rand.Next(options.Length);
            return options[r];
        }

        private void GenerateTonic(Chord[] buffer, int start, int count)
        {
            for (var i = 0; i < count; i++)
            {
                buffer[start + i] = RandomizeFrom(TonicChords);
            }
        }

        private void GenerateSubdominant(Chord[] buffer, int start, int count)
        {
            if (count == 0)
            {
                return;
            }

            buffer[start] = IV;

            for (var i = 1; i < count; i++)
            {
                buffer[start + i] = RandomizeFrom(SubdominantChords);
            }
        }

        private void GenerateDominant(Chord[] buffer, int start, int count)
        {
            for (var i = 0; i < count; i++)
            {
                buffer[start + i] = RandomizeFrom(DominantChords);
            }
        }
    }
}