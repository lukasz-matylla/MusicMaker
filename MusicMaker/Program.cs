using CommandLineParser.Arguments;
using CommandLineParser.Exceptions;
using Composer;
using MusicCore;

namespace MusicMaker
{
    public class Program
    {
        const int DefaultPhraseLength = 8;
        const int DefaultPhrases = 2;
        const int DefaultTempo = 110;

        public static void Main(string[] args)
        {
            var parser = new CommandLineParser.CommandLineParser();
            var play = new SwitchArgument('p', "play", "Play the piece after composing it", false);
            parser.Arguments.Add(play);
            var outputFile = new ValueArgument<string>('o', "output", "Name of the output musicxml file. If none is provided, no output file is generated.");
            outputFile.Optional = true;
            parser.Arguments.Add(outputFile);
            var key = new EnumeratedValueArgument<string>('k', "key", "Key of the composition. Default: major.", new[] { "major", "minor" });
            key.Optional = true;
            parser.Arguments.Add(key);
            var tertian = new SwitchArgument('r', "tertian", "Use tertian harmony instead of classical (functional) one.", false);
            parser.Arguments.Add(tertian);
            var bassType = new EnumeratedValueArgument<string>('b', "bass", "Type of the bass line. Possible values: none, simple, alberti, walking. Default: alberti", 
                new[] { "none", "simple", "alberti", "walking" });
            bassType.Optional = true;
            parser.Arguments.Add(bassType);
            var albertiType = new EnumeratedValueArgument<string>('a', "alberti", "Alberti bass pattern; only matters when Alberti bass is chosen. Possible values: down, up, updown, zigzag. Default: down", 
                new[] { "down", "up", "updown", "zigzag" });
            albertiType.Optional = true;
            parser.Arguments.Add(albertiType);
            var meterChoice = new EnumeratedValueArgument<string>('m', "meter", "Meter to use in composition. Default: 4/4", 
                new[] { "CC", "2/4", "3/4", "4/4", "5/4", "6/4", "6/8", "7/8" });
            meterChoice.Optional = true;
            parser.Arguments.Add(meterChoice);
            var tempo = new BoundedValueArgument<int>('t', "tempo", $"Tempo. Default: {DefaultTempo}", 50, 240);
            tempo.Optional = true;
            parser.Arguments.Add(tempo);
            var phraseLength = new BoundedValueArgument<int>('h', "phraseLength", $"Phrase length in measures. Default: {DefaultPhraseLength}", 2, 12);
            phraseLength.Optional = true;
            parser.Arguments.Add(phraseLength);
            var length = new BoundedValueArgument<int>('l', "length", $"Piece length in phrases. Default: {DefaultPhrases}", 1, 8);
            length.Optional = true;
            parser.Arguments.Add(length);

            try
            {
                parser.ParseCommandLine(args);
            }
            catch (CommandLineException ex)
            {
                Console.WriteLine(ex.Message);
                parser.ShowUsage();
                return;
            }

            if (string.IsNullOrEmpty(outputFile.Value) && !play.Value)
            {
                Console.WriteLine("No output provided. Choose one or both of output file and direct playback.");
                parser.ShowUsage();
                return;
            }

            ChordTransitionGraph chordGraph = new ClassicalMajorChordProgressionGraphWithSecondaries();
            if (key.Parsed && key.Value == "minor")
            {
                chordGraph = new ClassicalMinorChordProgressionGraphWithSecondaries();
            }

            if (tertian.Value)
            {
                chordGraph = new TertianHarmonyGraph(chordGraph.Scale);
            }

            var meter = Meter.CC;
            if (meterChoice.Parsed)
            {
                meter = meterChoice.Value switch
                {
                    "CC" => Meter.CC,
                    "2/4" => new Meter(2, 4),
                    "3/4" => new Meter(3, 4),
                    "4/4" => Meter.CC,
                    "5/4" => new Meter(5, 4),
                    "6/4" => new Meter(6, 4),
                    "6/8" => new Meter(6, 8),
                    "7/8" => new Meter(7, 8),
                    _ => throw new ArgumentOutOfRangeException("meter")
                };
            }

            var measuresInPhrase = phraseLength.Parsed ? phraseLength.Value : DefaultPhraseLength;
            var totalPhrases = length.Parsed ? length.Value : DefaultPhrases;
            var pieceTempo = tempo.Parsed ? tempo.Value : DefaultTempo;

            var chordGenerator = new GraphBasedChordProgression(chordGraph);
            var chords = chordGenerator.GenerateProgression(measuresInPhrase);

            var rhythmGenerator = new BasicRhythm();
            var rhythm = rhythmGenerator.CreateRhythm(measuresInPhrase, meter);

            var melodyMaker = new SimpleMelodyMaker(rangeFrom: 60, rangeTo: 85);
            var piece = melodyMaker.GenerateMelody(chords, rhythm, tonic: 60, scale: chordGraph.Scale, tempo: pieceTempo, measuresCount: measuresInPhrase * totalPhrases);

            if (!bassType.Parsed || bassType.Value == "alberti")
            {
                var pattern = AlbertiPattern.Down;

                if (albertiType.Parsed)
                {
                    pattern = albertiType.Value switch
                    {
                        "down" => AlbertiPattern.Down,
                        "up" => AlbertiPattern.Up,
                        "updown" => AlbertiPattern.UpDown,
                        "zigzag" => AlbertiPattern.ZigZag,
                        _ => throw new ArgumentOutOfRangeException("alberti")
                    };
                }

                var notesPerBeat = pattern == AlbertiPattern.UpDown ? 2 : 1;

                var bassMaker = new AlbertiBassMaker(pattern: pattern, notesPerBeat: notesPerBeat);
                var bass = bassMaker.GenerateBass(chords, rhythm, scale: chordGraph.Scale, tempo: pieceTempo, measuresCount: measuresInPhrase * totalPhrases);
                piece = piece.Merge(bass);
            }
            else if (bassType.Value == "simple")
            {
                var bassMaker = new SimpleBasslineMaker();
                var bass = bassMaker.GenerateBass(chords, rhythm, scale: chordGraph.Scale, tempo: pieceTempo, measuresCount: measuresInPhrase * totalPhrases);
                piece = piece.Merge(bass);
            }
            else if (bassType.Value == "walking")
            {
                var bassMaker = new WalkingBassMaker();
                var bass = bassMaker.GenerateBass(chords, rhythm, scale: chordGraph.Scale, tempo: pieceTempo, measuresCount: measuresInPhrase * totalPhrases);
                piece = piece.Merge(bass);
            }

            if (!string.IsNullOrEmpty(outputFile.Value))
            {
                var name = outputFile.Value;
                if (!name.Contains('.'))
                {
                    name += ".musicxml";
                }
                var mx = new MusicXml();
                Console.WriteLine($"Writing composition to {name}");
                mx.WriteToFile(name, piece);
            }

            if (play.Value)
            {
                Console.WriteLine("Playing the piece on default output");
                var player = new MusicPlayer();
                player.Play(piece);
            }
        }
    }
}