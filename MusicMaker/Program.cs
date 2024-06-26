﻿using Composer;
using Composer.ChordProgression;
using MusicCore;

namespace MusicMaker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Parsing command line arguments");
            var par = new Parameters();
            if (!par.Parse(args))
            {
                return;
            }
            
            if (string.IsNullOrEmpty(par.OutputFile) && !par.Play)
            {
                Console.WriteLine("No output selected. Choose one or both of output file and direct playback.");
                par.ShowUsage();
                return;
            }

            Console.WriteLine("Determining harmony");
            ChordTransitionGraph chordGraph = par.Harmony switch
            {
                Harmony.Simple => par.Scale == MusicalScale.Major ?
                    new SimpleMajorChordProgressionGraph() :
                    new SimpleMinorChordProgressionGraph(),
                Harmony.Classic => par.Scale == MusicalScale.Major ?
                    new ClassicalMajorChordProgressionGraph() :
                    new ClassicalMinorChordProgressionGraph(),
                Harmony.Complex => par.Scale == MusicalScale.Major ?
                    new ClassicalMajorChordProgressionGraphWithSecondaries() :
                    new ClassicalMinorChordProgressionGraphWithSecondaries(),
                Harmony.Free => new AmbientHarmonyGraph(par.Scale),
                _ => throw new NotImplementedException()
            };

            Console.WriteLine("Generating chord progression");
            IChordProgressionGenerator chordGenerator;
            if (par.Harmony != Harmony.Ambient)
            {
                var options = par.Harmony switch
                {
                    Harmony.Simple => new ChordProgressionOptions(ChromaticApproach.StrictlyDiatonic),
                    Harmony.Classic => new ChordProgressionOptions(ChromaticApproach.MostlyDiatonic),
                    Harmony.Complex => new ChordProgressionOptions(ChromaticApproach.MostlyChromatic),
                    Harmony.Free => new ChordProgressionOptions(ChromaticApproach.Free),
                    _ => throw new ArgumentException($"Unknown harmony type: {par.Harmony}")
                };
                chordGenerator = new AdvancedFunctionalChordProgression(par.Scale, options);
            }
            else
            {
                chordGenerator = new GraphBasedChordProgression(new AmbientHarmonyGraph(par.Scale));
            }
            
            var chords = chordGenerator.GenerateProgression(par.PhraseLength);

            Console.WriteLine("Generating rhythm");
            var rhythmGenerator = new BasicRhythm();
            var rhythm = rhythmGenerator.CreateRhythm(par.PhraseLength, par.Meter);
            
            var parts = new List<Staff>();

            Console.WriteLine("Generating melody");
            var melodyMaker = new SimpleMelodyMaker()
                .InKey(par.Tonic, chordGraph.Scale)
                .InClef(Clef.Treble)
                .OnInstrument(Instrument.AcousticGrandPiano)
                .InTempo(par.Tempo)
                .WithRhythm(rhythm)
                .OverChords(chords);

            switch (par.Harmony)
            {
                case Harmony.Simple:
                    break;
                case Harmony.Classic:
                    melodyMaker = melodyMaker
                        .WithChromaticTransitions();
                    break;
                case Harmony.Complex:
                case Harmony.Free:
                    melodyMaker = melodyMaker
                        .WithChromaticTransitions()
                        .WithNctStrongBeats();
                    break;
            }
                            
            var melody = melodyMaker.GenerateMelody(par.Length);
            parts.Add(melody);
            
            var bassMakerFactory = new BassMakerFactory();
            var bassMaker = bassMakerFactory.CreateBassMaker(par.BassType, par.BassPattern);
            if (bassMaker != null)
            {
                Console.WriteLine("Generating bass");
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