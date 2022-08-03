using MusicCore;
using System.Diagnostics;
using Tools;

namespace Composer.ChordProgression
{
    public class AdvancedFunctionalChordProgression : IChordProgressionGenerator
    {
        private readonly Random rand;
        private readonly MusicalScale scale;
        private readonly ChordProgressionOptions options;
        private readonly ChordFlags optionsFilter;
        private readonly IFilteredTransitionGraph<FlaggedAbstractChord> transitionGraph;

        private readonly IReadOnlyDictionary<ChordType, ScaleInterval[]> chordStructure = new Dictionary<ChordType, ScaleInterval[]>
        {
            { ChordType.Major, new[] { ScaleInterval.MajorThird, ScaleInterval.MinorThird }},
            { ChordType.Minor, new[] { ScaleInterval.MinorThird, ScaleInterval.MajorThird }},
            { ChordType.Diminished, new[] { ScaleInterval.MinorThird, ScaleInterval.MinorThird }},
            { ChordType.Augmented, new[] { ScaleInterval.MajorThird, ScaleInterval.MajorThird }},
            { ChordType.Sus4, new[] { ScaleInterval.PerfectFourth, ScaleInterval.MajorSecond }},
            { ChordType.Sus2, new[] { ScaleInterval.MajorSecond, ScaleInterval.PerfectFourth }},
            { ChordType.Major7, new[] { ScaleInterval.MajorThird, ScaleInterval.MinorThird, ScaleInterval.MajorThird }},
            { ChordType.Minor7, new[] { ScaleInterval.MinorThird, ScaleInterval.MajorThird, ScaleInterval.MinorThird }},
            { ChordType.Dominant7, new[] { ScaleInterval.MajorThird, ScaleInterval.MinorThird, ScaleInterval.MinorThird }},
            { ChordType.HalfDiminished, new[] { ScaleInterval.MinorThird, ScaleInterval.MinorThird, ScaleInterval.MajorThird }},
            { ChordType.FullyDiminished, new[] { ScaleInterval.MinorThird, ScaleInterval.MinorThird, ScaleInterval.MinorThird }},
            { ChordType.MinMaj7, new[] { ScaleInterval.MinorThird, ScaleInterval.MajorThird, ScaleInterval.MajorThird }},
            { ChordType.DominantB5, new[] { ScaleInterval.MajorThird, ScaleInterval.DiminishedThird, ScaleInterval.MajorThird } }
        };

        public AdvancedFunctionalChordProgression(MusicalScale scale, ChordProgressionOptions? options = null)
        {
            this.scale = scale;
            this.options = options ?? new ChordProgressionOptions();
            optionsFilter = CreateOptionsFilter();
            transitionGraph = CreateTransitionGraph();
            rand = new Random();          
        }

        private ChordFlags CreateOptionsFilter()
        {
            var result = ChordFlags.Diatonic;

            if (!options.ForceNaturalMinor)
            {
                result |= ChordFlags.MelodicMinor;
            }

            if (options.UseSecondaryDominants)
            {
                result |= ChordFlags.SecondaryDominant;
            }

            if (options.UseBorrowedChords)
            {
                result |= ChordFlags.Borrowed;
            }

            if (options.UseNeapolitan)
            {
                result |= ChordFlags.Neapolitan;
            }

            if (options.UseExtendedChords)
            {
                result |= ChordFlags.Extended;
            }

            if (options.UseChromaticMediants)
            {
                result |= ChordFlags.ChromaticMediant;
            }

            if (options.UseTritoneSubstitution)
            {
                result |= ChordFlags.TritoneSubstitution;
            }

            if (options.UseAlteredHarmony)
            {
                result |= ChordFlags.Altered;
            }

            return result;
        }

        private bool ChordFilter(FlaggedAbstractChord chord, HarmonicFunction function = HarmonicFunction.Any)
        {
            if (!optionsFilter.HasFlag(chord.Flags))
            {
                return false;
            }

            if (!chord.Function.HasFlag(function))
            {
                return false;
            }

            if (!options.UseSuspended &&
                (chord.Type == ChordType.Sus2 || chord.Type == ChordType.Sus4))
            {
                return false;
            }

            if (!options.UseFullyDiminished && chord.Type == ChordType.FullyDiminished)
            {
                return false;
            }

            if (!options.UseAugmentedChords && chord.Type == ChordType.Augmented)
            {
                return false;
            }

            return true;
        }

        private IFilteredTransitionGraph<FlaggedAbstractChord> CreateTransitionGraph()
        {
            if (scale.Equals(MusicalScale.Major))
            {
                return new MajorAbstractChordGraph();
            }
            else if (scale.Equals(MusicalScale.Minor))
            {
                return new MinorAbstractChordGraph();
            }
            else
            {
                ValidateScale(scale);
                return new GeneralAbstractChordGraph();
            }
        }

        private void ValidateScale(MusicalScale scale)
        {
            if ((scale.Steps[2] != 3 && scale.Steps[2] != 4)
                || scale.Steps[4] != 7)
            {
                throw new InvalidOperationException("Can't build a functional progresion in a scale that doesn't have a minor or major third and perfect fifth");
            }
        }

        public Chord[] GenerateProgression(int length, CadenceType cadence = CadenceType.Strong)
        {
            var progression = new FlaggedAbstractChord[length];

            var numPhrases = (int)Math.Ceiling((double)length / options.MaxPhraseLength);
            var averagePhrase = (double)length / numPhrases;

            for (var i = 0; i < numPhrases; i++)
            {
                var start = (int)Math.Round(i * averagePhrase);
                var end = (int)Math.Round((i + 1) * averagePhrase) - 1;

                if (i < numPhrases - 1)
                {
                    var phraseCadence = rand.NextDouble() < options.HalfCadencePhraseEnding ? 
                        CadenceType.Half : 
                        CadenceType.Weak;

                    GenerateAbstractProgression(progression, start, end, phraseCadence);
                }
                else
                {
                    GenerateAbstractProgression(progression, start, end, cadence);
                }
            }

            LogChordProgression(progression);
            var result = RealizeChords(progression);
            
            return result;
        }

        private void LogChordProgression(FlaggedAbstractChord[] progression)
        {
            foreach (var chord in progression)
            {
                Debug.WriteLine(chord);
            }
        }

        private void GenerateAbstractProgression(FlaggedAbstractChord[] progression, int start, int end, CadenceType phraseCadence)
        {
            var initialTonic = rand.SelectRandomly(transitionGraph.FilteredItems(c => ChordFilter(c, HarmonicFunction.TonicInitial)));
            var finalTonic = (initialTonic.Type != ChordType.Major && initialTonic.Type != ChordType.Major7 && options.UsePiccardy) ? 
                new FlaggedAbstractChord(0, ChordType.Major, 0) : 
                initialTonic;
            var strongDominants = transitionGraph.FilteredItems(c => ChordFilter(c, HarmonicFunction.DominantSolo | HarmonicFunction.DominantStrong));
            if (strongDominants.Count == 0)
            {
                strongDominants = transitionGraph.FilteredItems(c => ChordFilter(c, HarmonicFunction.DominantSolo));
            }
            var strongDominant = rand.SelectRandomly(strongDominants);

            progression[start] = initialTonic;

            switch (phraseCadence)
            {
                case CadenceType.Strong:
                    progression[end] = finalTonic;
                    FillTPD(progression, start + 1, end - 1, enforceDominant: true, strong: true);
                    break;
                case CadenceType.Weak:
                    progression[end] = initialTonic;
                    FillTPD(progression, start + 1, end - 1, strong: false);
                    break;
                case CadenceType.Half:
                    progression[end] = strongDominant;
                    progression[end - 1] = initialTonic;
                    FillTPD(progression, start + 1, end - 2);
                    break;
                case CadenceType.Loop:
                    FillTPD(progression, start + 1, end, enforceDominant: true);
                    break;
                case CadenceType.Any:
                    FillTPD(progression, start + 1, end);
                    break;
            }
        }

        private void FillTPD(FlaggedAbstractChord[] progression, int start, int end, bool? strong = null, bool enforceDominant = false)
        {
            var length = end - start + 1;
            var partition = rand.Partition(length, 3, 1, 0, enforceDominant ? 1 : 0);

            FillDominant(progression, start + partition[0] + partition[1], end, strong);
            FillPredominant(progression, start + partition[0], start + partition[0] + partition[1] - 1);
            FillTonic(progression, start, start + partition[0] - 1);
        }

        private void FillTonic(FlaggedAbstractChord[] progression, int start, int end)
        {
            if (start > end)
            {
                return;
            }

            var prev = start > 0 ? progression[start - 1] : null;
            var next = end < progression.Length - 1 ? progression[end + 1] : progression[0];

            progression[end] = SelectTonicFinal(start == end ? prev : null, next);

            // No special handling for TonicInitial here - it's already filled in GenerateAbstractProgression

            for (var i = end - 1; i >= start; i--)
            {
                FillMiddle(progression, i, HarmonicFunction.Tonic);
            }
        }

        private FlaggedAbstractChord SelectTonicFinal(FlaggedAbstractChord? prev, FlaggedAbstractChord next)
        {
            var weights = transitionGraph.WeightsTo(next, c => ChordFilter(c, HarmonicFunction.TonicFinal));

            if (prev != null)
            {
                weights = weights.Mult(transitionGraph.WeightsFrom(prev, c => ChordFilter(c, HarmonicFunction.TonicFinal)));
            }

            var result = rand.SelectRandomly(transitionGraph.Items, weights);
            return result;
        }

        private void FillPredominant(FlaggedAbstractChord[] progression, int start, int end)
        {
            if (start > end)
            {
                return;
            }

            var prev = start > 0 ? progression[start - 1] : null;
            var next = end < progression.Length - 1 ? progression[end + 1] : progression[0];

            progression[end] = SelectPredominantFinal(start == end ? prev : null, next, start == end);

            if (start < end)
            {
                progression[start] = SelectPredominantInitial(prev, progression[end]);
            }

            for (var i = end - 1; i > start; i--)
            {
                FillMiddle(progression, i, HarmonicFunction.Predominant);
            }
        }

        private FlaggedAbstractChord SelectPredominantInitial(FlaggedAbstractChord? prev, FlaggedAbstractChord last)
        {
            var weights = transitionGraph.WeightsTo(last, c => ChordFilter(c, HarmonicFunction.PredominantInitial | HarmonicFunction.Predominant));

            if (prev != null)
            {
                weights = weights.Mult(transitionGraph.WeightsFrom(prev, c => ChordFilter(c, HarmonicFunction.PredominantInitial)));
            }

            var result = rand.SelectRandomly(transitionGraph.Items, weights);
            return result;
        }

        private FlaggedAbstractChord SelectPredominantFinal(FlaggedAbstractChord? prev, FlaggedAbstractChord next, bool isAlsoInitial)
        {
            var function = HarmonicFunction.PredominantFinal;
            if (isAlsoInitial)
            {
                function |= HarmonicFunction.PredominantInitial;
            }
            var weights = transitionGraph.WeightsTo(next, c => ChordFilter(c, function));

            if (prev != null)
            {
                // PredominantInitial used intentionally - that's the case when we only have a single predominant chord
                weights = weights.Mult(transitionGraph.WeightsFrom(prev, c => ChordFilter(c, HarmonicFunction.PredominantInitial)));
            }

            var result = rand.SelectRandomly(transitionGraph.Items, weights);
            return result;
        }

        private void FillDominant(FlaggedAbstractChord[] progression, int start, int end, bool? strong)
        {
            if (start > end)
            {
                return;
            }
                        
            progression[end] = SelectDominantFinal(start == end, strong);

            if (start < end)
            {
                var prev = start > 0 ? progression[start - 1] : null;
                progression[start] = SelectDominantInitial(prev, progression[end]);
            }

            for (var i = end - 1; i > start; i--)
            {
                FillMiddle(progression, i, HarmonicFunction.Dominant);
            }
        }

        private FlaggedAbstractChord SelectDominantInitial(FlaggedAbstractChord? prev, FlaggedAbstractChord last)
        {
            var weights = transitionGraph.WeightsTo(last, c => ChordFilter(c, HarmonicFunction.DominantInitial));

            if (prev != null)
            {
                weights = weights.Mult(transitionGraph.WeightsFrom(prev, c => ChordFilter(c, HarmonicFunction.DominantInitial)));
            }

            var result = rand.SelectRandomly(transitionGraph.Items, weights);
            return result;
        }

        private FlaggedAbstractChord SelectDominantFinal(bool isAlsoInitial, bool? strong)
        {
            var function = HarmonicFunction.DominantFinal;
            if (isAlsoInitial)
            {
                function |= HarmonicFunction.DominantSolo;
            }
            else
            {
                function |= HarmonicFunction.Dominant;
            }

            var finals = transitionGraph.FilteredItems(c => ChordFilter(c, function));
            var filteredFinals = finals;
            if (strong != null)
            {
                filteredFinals = finals
                    .Where(c => c.Function.HasFlag(HarmonicFunction.DominantStrong) == strong.Value)
                    .ToArray();
            }
            
            var result = filteredFinals.Any() ? 
                rand.SelectRandomly(filteredFinals) : 
                rand.SelectRandomly(finals);
            return result;
        }

        private void FillMiddle(FlaggedAbstractChord[] progression, int i, HarmonicFunction function)
        {
            var weights = Enumerable.Repeat(1.0, transitionGraph.Items.Count).ToArray();

            if (i > 0 && progression[i - 1] != null)
            {
                var prev = progression[i - 1];
                var prevWeights = transitionGraph.WeightsFrom(prev, c => ChordFilter(c, function));
                weights = weights.Mult(prevWeights);
            }

            if (i < progression.Length - 1 && progression[i + 1] != null)
            {
                var next = progression[i + 1];
                var nextWeights = transitionGraph.WeightsTo(next, c => ChordFilter(c, function));
                weights = weights.Mult(nextWeights);
            }

            var chosen = rand.SelectRandomly(transitionGraph.Items, weights);
            progression[i] = chosen;
        }

        private Chord[] RealizeChords(AbstractChord[] progression)
        {
            var length = progression.Length;
            var result = new Chord[length];

            // First fill chords with known inversions
            for (var i = 0; i < length; i++)
            {
                var input = progression[i]; 
                if (input.Inversion != null)
                {
                    var chord = AbstractToReal(input);
                    result[i] = chord.Inversion(input.Inversion.Value);
                }
            }

            // For other chords, choose inversion for smoothest voice leading 
            for (var i = length - 1; i >= 0; i--)
            {
                var input = progression[i];
                if (input.Inversion == null)
                {
                    if (options.UseInversions)
                    {
                        var chord = AbstractToReal(input);
                        var prev = result[i - 1];
                        var next = i < length - 1 ? result[i + 1] : result[0];
                        var inversion = MinimizeVoiceMovement(chord, prev, next);
                        result[i] = chord.Inversion(inversion);
                    }
                    else
                    {
                        var chord = AbstractToReal(input);
                        result[i] = chord;
                    }
                }
                
            }

            return result;
        }

        private int MinimizeVoiceMovement(Chord chord, Chord? prev, Chord? next)
        {
            var bestMovement = int.MaxValue;
            var bestInversion = -1;

            for (var i = 0; i < chord.Notes.Count; i++)
            {
                var inverted = chord.Inversion(i);

                var distance = 0;

                if (prev != null)
                {
                    distance += ChordDistance(prev, inverted);
                }
                if (next != null)
                {
                    distance += ChordDistance(inverted, next);
                }

                if (distance < bestMovement)
                {
                    bestMovement = distance;
                    bestInversion = i;
                }
            }

            return bestInversion;
        }

        private int ChordDistance(Chord a, Chord b)
        {
            var distance = 0;

            var minSize = Math.Min(a.Notes.Count, b.Notes.Count);

            for (var i = 0; i < minSize; i++)
            {
                distance += Math.Abs(scale.MinimumHalftoneDistance(a.Notes[i], b.Notes[i]));
            }

            if (a.Notes.Count > minSize)
            {
                for (var i = minSize; i < a.Notes.Count; i++)
                {
                    distance += Math.Abs(scale.MinimumHalftoneDistance(a.Notes[i], b.Notes[i % minSize]));
                }
            }

            if (b.Notes.Count > minSize)
            {
                for (var i = minSize; i < b.Notes.Count; i++)
                {
                    distance += Math.Abs(scale.HalftoneInterval(a.Notes[i % minSize], b.Notes[i]));
                }
            }

            return distance;
        }

        private Chord AbstractToReal(AbstractChord chord)
        {
            var intervals = chordStructure[chord.Type];
            var result = ChordOperations.StackedIntervals(chord.Root, scale, false, intervals);
            return result;
        }
    }
}
