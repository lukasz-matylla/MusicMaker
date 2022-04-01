using MusicCore;
using Tools;

namespace Composer
{
    public class WalkingBasslineMaker : BassMakerBase, IBasslineMaker
    {
        

        private readonly Random r = new Random();

        private readonly int[] importance = new[] { 8, 30, 16, 24, 8, 6, 4, 4 };

        protected override void FillBar(Staff result, int measure, Chord chord, Chord nextChord, IReadOnlyList<Note> beats, int wrapAbove)
        {
            var root = GetChordTone(chord, 0, wrapAbove);
            var nextRoot = GetChordTone(nextChord, 0, wrapAbove);

            var smoothLines = SequenceOptimizer.FindOptimal(result.Meter.Top - 1, chord.Notes.Count, s => LineQuality(s, chord, wrapAbove, result.Scale, root, nextRoot));
            var line = smoothLines[r.Next(smoothLines.Length)];

            var noteLength = result.Meter.BeatLength;
            result.AddNote(measure, new Note(root, noteLength));

            for (var i = 1; i < result.Meter.Top; i++)
            {
                var start = i * noteLength;
                var pitch = GetChordTone(chord, line[i - 1], wrapAbove, false);
                result.AddNote(measure, new Note(pitch, noteLength, start));
            }
        }

        private double LineQuality(int[] line, Chord chord, int wrapAbove, MusicalScale scale, ScaleStep prev, ScaleStep next)
        {
            var value = line
                .Distinct()
                .Where(n => n < importance.Length)
                .Select(n => importance[n])
                .Sum();

            var pitches = line
                .Select(n => GetChordTone(chord, n, wrapAbove, false))
                .ToArray();

            value -= scale.NoteInterval(prev, pitches[0]) * scale.NoteInterval(prev, pitches[0]);
            for (var i = 0; i < line.Length - 1; i++)
            {
                value -= scale.NoteInterval(pitches[i], pitches[i + 1]) * scale.NoteInterval(pitches[i], pitches[i + 1]);
            }
            value -= 5 * scale.NoteInterval(pitches.Last(), next)* scale.NoteInterval(pitches.Last(), next);

            return value;
        }
    }
}
