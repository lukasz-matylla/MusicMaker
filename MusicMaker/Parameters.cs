using CommandLineParser.Arguments;
using CommandLineParser.Exceptions;
using Composer;
using MusicCore;

namespace MusicMaker
{
    public class Parameters
    {
        private const int DefaultPhraseLength = 8;
        private const int DefaultPhrases = 4;
        private const int DefaultTempo = 110;

        private readonly IReadOnlyDictionary<string, Meter> MeterTypes = new Dictionary<string, Meter>()
        {
            { "CC" , Meter.CC },
            { "2/4", new Meter(2, 4) },
            { "3/4", new Meter(3, 4) },
            { "4/4", Meter.CC },
            { "5/4", new Meter(5, 4) },
            { "6/4", new Meter(6, 4) },
            { "6/8", new Meter(6, 8) },
            { "7/8", new Meter(7, 8) }
        };

        private readonly CommandLineParser.CommandLineParser parser;

        private readonly SwitchArgument play;
        private readonly ValueArgument<string> outputFile;
        private readonly EnumeratedValueArgument<string> key;
        private readonly EnumeratedValueArgument<string> tonic;
        private readonly EnumeratedValueArgument<string> harmony;
        private readonly EnumeratedValueArgument<string> bassType;
        private readonly EnumeratedValueArgument<string> bassPattern;
        private readonly EnumeratedValueArgument<string> meterChoice;
        private readonly BoundedValueArgument<int> tempo;
        private readonly BoundedValueArgument<int> phraseLength;
        private readonly BoundedValueArgument<int> length;
        private readonly BoundedValueArgument<double> temperature;

        public bool Play => play.Value;
        public string? OutputFile => outputFile.Parsed ? outputFile.Value : null;
        public MusicalScale Scale
        {
            get
            {
                if (key.Parsed && key.Value == "minor")
                {
                    return MusicalScale.Minor;
                }

                return MusicalScale.Major;
            }
        }
        public Key Tonic
        {
            get
            {
                if (tonic.Parsed && Enum.TryParse<Key>(tonic.Value, true, out var tonicKey))
                {
                    return tonicKey;
                }

                if (Scale == MusicalScale.Minor)
                {
                    return Key.A;
                }

                return Key.C;
            }
        }
        public Harmony Harmony => harmony.ParseWithDefault(Harmony.Simple);
        public BassType BassType => bassType.ParseWithDefault(BassType.None);
        public ArpeggioPattern BassPattern => bassPattern.ParseWithDefault(ArpeggioPattern.Alberti);
        public Meter Meter
        {
            get
            {
                if (meterChoice.Parsed && MeterTypes.TryGetValue(meterChoice.Value.ToUpper(), out var value))
                {
                    return value;
                }

                return Meter.CC;
            }
        }

        public double Temperature => temperature.Parsed ? temperature.Value : 1.0;

        public int Tempo => tempo.Parsed ? tempo.Value : DefaultTempo;
        public int PhraseLength => phraseLength.Parsed ? phraseLength.Value : DefaultPhraseLength;
        public int Length => length.Parsed ? length.Value * PhraseLength: DefaultPhrases * PhraseLength;
        

        public Parameters()
        {
            parser = new CommandLineParser.CommandLineParser();

            play = new SwitchArgument('p', "play", "Play the piece after composing it", false);
            parser.Arguments.Add(play);

            outputFile = new ValueArgument<string>('o', "output", "Name of the output musicxml file. If none is provided, no output file is generated.");
            outputFile.Optional = true;
            parser.Arguments.Add(outputFile);

            key = new EnumeratedValueArgument<string>('k', "key", "Key of the composition. Default: major.", new[] { "major", "minor" });
            key.Optional = true;
            key.IgnoreCase = true;
            parser.Arguments.Add(key);

            tonic = new EnumeratedValueArgument<string>('n', "tonic", "Tonic of the composition. Default: C for majot key, A for minor",
                Enum.GetNames<Key>());
            tonic.Optional = true;
            tonic.IgnoreCase = true;
            parser.Arguments.Add(tonic);

            var harmonyOptions = Enum.GetNames<Harmony>();
            harmony = new EnumeratedValueArgument<string>('h', "harmony", $"Type of harmony. Possible values: {string.Join(", ", harmonyOptions)}. Default: simple",
                harmonyOptions);
            harmony.Optional = true;
            harmony.IgnoreCase = true;
            parser.Arguments.Add(harmony);

            var bassOptions = Enum.GetNames<BassType>();
            bassType = new EnumeratedValueArgument<string>('b', "bass", $"Type of the bass line. Possible values: {string.Join(", ", bassOptions)}. Default: {BassType.Arpeggio}",
                bassOptions);
            bassType.Optional = true;
            bassType.IgnoreCase = true;
            parser.Arguments.Add(bassType);

            var bassPatternOptions = Enum.GetNames<ArpeggioPattern>();
            bassPattern = new EnumeratedValueArgument<string>('a', "bassPattern", $"Bass pattern; only matters when Arpeggio bass is chosen. Possible values: {string.Join(", ", bassPatternOptions)}. Default: {ArpeggioPattern.Alberti}",
                bassPatternOptions);
            bassPattern.Optional = true;
            bassPattern.IgnoreCase = true;
            parser.Arguments.Add(bassPattern);

            meterChoice = new EnumeratedValueArgument<string>('m', "meter", $"Meter to use in composition. Possible values: {string.Join(", ", MeterTypes.Keys)}. Default: 4/4",
                MeterTypes.Keys.ToArray());
            meterChoice.Optional = true;
            parser.Arguments.Add(meterChoice);

            tempo = new BoundedValueArgument<int>('t', "tempo", $"Tempo. Default: {DefaultTempo}", 50, 240);
            tempo.Optional = true;
            parser.Arguments.Add(tempo);

            phraseLength = new BoundedValueArgument<int>('r', "phraseLength", $"Phrase length in measures. Default: {DefaultPhraseLength}", 2, 32);
            phraseLength.Optional = true;
            parser.Arguments.Add(phraseLength);

            length = new BoundedValueArgument<int>('l', "length", $"Piece length in phrases. Phrases share the same rhythm and chord progression, but differ in melody. Default: {DefaultPhrases}", 1, 8);
            length.Optional = true;
            parser.Arguments.Add(length);

            temperature = new BoundedValueArgument<double>('e', "temperature",
                "'Temperature' of the rhythm; high values prefer shorter notes and more syncopation. Default: 1", 0.1, 5);
            temperature.Optional = true;
            parser.Arguments.Add(temperature);
        }

        public bool Parse(string[] args)
        {
            try
            {
                parser.ParseCommandLine(args);
            }
            catch (CommandLineException ex)
            {
                Console.WriteLine(ex.Message);
                ShowUsage();
                return false;
            }

            return true;
        }

        public void ShowUsage()
        {
            parser.ShowUsage();
        }
    }

    public enum Harmony
    {
        Simple,
        Classic,
        Complex,
        Free,
        Ambient
    }
}
