using Composer;
using MusicCore;

namespace MusicMaker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var par = new Parameters();
            if (!par.Parse(args))
            {
                return;
            }

            if (string.IsNullOrEmpty(par.OutputFile) && !par.Play)
            {
                Console.WriteLine("No output provided. Choose one or both of output file and direct playback.");
                par.ShowUsage();
                return;
            }

            ChordTransitionGraph chordGraph = par.Harmony switch
            {
                Harmony.Simple => par.Scale == MusicalScale.Major ?
                    new SimpleMajorChordProgressionGraph() :
                    new SimpleMinorChordProgressionGraph(),
                Harmony.Classic => par.Scale == MusicalScale.Major ?
                    new ClassicalMajorChordProgressionGraphWithSecondaries() :
                    new ClassicalMinorChordProgressionGraphWithSecondaries(),
                Harmony.Tertian => new TertianHarmonyGraph(par.Scale),
                _ => throw new NotImplementedException()
            };

            var chordGenerator = new GraphBasedChordProgression(chordGraph);
            var chords = chordGenerator.GenerateProgression(par.PhraseLength);

            //var rhythmGenerator = new BasicRhythm();
            var rhythmGraph = new AutomaticRhythmPatternGraph();
            var rhythmGenerator = new PatternGraphRhythm(rhythmGraph);
            var rhythm = rhythmGenerator.CreateRhythm(par.PhraseLength, par.Meter);

            var parts = new List<Staff>();

            var melody = new SimpleMelodyMaker()
                .InKey(par.Tonic, chordGraph.Scale)
                .InClef(Clef.Treble)
                .OnInstrument(Instrument.AcousticGrandPiano)
                .InTempo(par.Tempo)
                .WithRhythm(rhythm)
                .OverChords(chords)
                .GenerateMelody(par.Length);
            parts.Add(melody);

            var bassMakerFactory = new BassMakerFactory();
            var bassMaker = bassMakerFactory.CreateBassMaker(par.BassType, par.BassPattern);
            if (bassMaker != null)
            {
                var bass = bassMaker.GenerateBass(chords, rhythm, key: par.Tonic, scale: chordGraph.Scale, tempo: par.Tempo, measuresCount: par.Length);
                parts.Add(bass);
            }

            if (!string.IsNullOrEmpty(par.OutputFile))
            {
                var name = par.OutputFile;
                if (!name.Contains('.'))
                {
                    name += ".musicxml";
                }
                var mx = new MusicXml();
                Console.WriteLine($"Writing composition to {name}");
                mx.WriteToFile(name, parts.ToArray());
            }

            if (par.Play)
            {
                Console.WriteLine("Playing the piece on default output");
                var player = new MusicPlayer();
                player.Play(parts.ToArray());
            }
        }
    }
}