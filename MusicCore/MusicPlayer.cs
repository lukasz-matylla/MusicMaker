using NAudio.Midi;

namespace MusicCore
{
    public class MusicPlayer
    {
        private HashSet<int> channels;
        
        public const int PercussionChannel = 10;
        public const int MaxChannel = 16;

        public MusicPlayer()
        {
            channels = new HashSet<int>();
        }

        public void Play(params Staff[] parts)
        {
            if (parts.Length == 0)
            {
                return;
            }

            var channelMapping = CreateChannelMapping(parts);
            var events = new List<MidiEvent>();

            events.AddRange(CreateInstrumentEvents(channelMapping, parts));

            var milisecondsPerQuarterNote = 60000 / parts[0].Tempo;
            var tempoChange = new TempoEvent(milisecondsPerQuarterNote, 0);
            events.Add(tempoChange);

            for (var partIndex = 0; partIndex < parts.Length; partIndex++)
            {
                for (var measure = 0; measure < parts[partIndex].MeasureCount; measure++)
                {
                    var start = parts[partIndex].StartTime + parts[partIndex].MeasureLength * measure;

                    foreach (var note in parts[partIndex][measure])
                    {
                        var onEvent = new NoteOnEvent(start + note.StartTime, 
                            channelMapping[partIndex], 
                            parts[partIndex].ToAbsolutePitch(note.Pitch), 
                            100, 
                            note.Length);
                        events.Add(onEvent);
                        events.Add(onEvent.OffEvent);
                    }
                }
            }

            var milisecondsPerTick = milisecondsPerQuarterNote / (int)NoteValue.Quarter;
            Play(events, milisecondsPerTick);

            foreach (var channel in channelMapping)
            {
                channels.Remove(channel);
            }
        }

        private IEnumerable<MidiEvent> CreateInstrumentEvents(int[] channelMapping, Staff[] parts)
        {
            for (var i = 0; i < parts.Length; i++)
            {
                yield return new PatchChangeEvent(0, channelMapping[i], (int)parts[i].Instrument - 1);
            }
        }

        private void Play(IEnumerable<MidiEvent> events, int milisecondsPerTick)
        {
            var toPlay = events.OrderBy(e => e.AbsoluteTime).ToArray();

            using (var mo = new MidiOut(0))
            {
                long t = 0;

                foreach (var ev in toPlay)
                {
                    if (ev.AbsoluteTime > t)
                    {
                        var ms = (ev.AbsoluteTime - t) * milisecondsPerTick;
                        Thread.Sleep((int)ms);
                        t = ev.AbsoluteTime;
                    }

                    mo.Send(ev.GetAsShortMessage());
                }
            }
        }

        private int[] CreateChannelMapping(Staff[] parts)
        {
            var result = new List<int>();
            for (var i = 0; i < parts.Length; i++)
            {
                result.Add(ReserveFreeChannel());
            }
            return result.ToArray();
        }

        private int ReserveFreeChannel()
        {
            for (var i = 1; i <= MaxChannel; i++)
            {
                if (i != PercussionChannel && !channels.Contains(i))
                {
                    channels.Add(i);
                    return i;
                }
            }

            throw new InvalidOperationException("Can't find a free channel");
        }
    }
}
