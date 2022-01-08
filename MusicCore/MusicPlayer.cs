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

        public void Play(Staff staff)
        {
            var channel = ReserveFreeChannel();
            var events = new List<MidiEvent>();

            var instrumentChange = new PatchChangeEvent(0, channel, (int)staff.Instrument - 1);
            events.Add(instrumentChange);

            var milisecondsPerQuarterNote = 60000 / staff.Tempo;
            var tempoChange = new TempoEvent(milisecondsPerQuarterNote, 0);
            events.Add(tempoChange);

            for (var measure = 0; measure < staff.MeasureCount; measure++)
            {
                var start = staff.StartTime + staff.MeasureLength * measure;

                foreach (var note in staff[measure])
                {
                    var onEvent = new NoteOnEvent(start + note.StartTime, channel, staff.ToAbsolutePitch(note.Pitch), 100, note.Length);
                    events.Add(onEvent);
                    events.Add(onEvent.OffEvent);
                }
            }

            var milisecondsPerTick = milisecondsPerQuarterNote / (int)NoteValue.Quarter;
            Play(events, milisecondsPerTick);

            channels.Remove(channel);
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
