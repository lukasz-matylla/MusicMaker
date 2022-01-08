using System.Xml;
using System.Xml.Linq;

namespace MusicCore
{
    public class MusicXml
    {
        private readonly Dictionary<NoteValue, string> noteTypes = new Dictionary<NoteValue, string>()
        {
            { NoteValue.Whole, "whole" },
            { NoteValue.HalfDot, "half" },
            { NoteValue.Half, "half" },
            { NoteValue.QuarterDot, "quarter" },
            { NoteValue.Quarter, "quarter" },
            { NoteValue.EighthDot, "eighth" },
            { NoteValue.Eighth, "eighth" },
            { NoteValue.SixteenthDot, "16th" },
            { NoteValue.Sixteenth, "16th" },
            { NoteValue.ThirtySecond, "32nd" },
        };

        public void WriteToFile(string fileName, params Staff[] parts)
        {
            var doc = new XDocument();
            var docType = new XDocumentType("score-partwise", "-//Recordare//DTD MusicXML 4.0 Partwise//EN", "http://www.musicxml.org/dtds/partwise.dtd", null);
            doc.AddFirst(docType);

            var score = new XElement("score-partwise",
                new XAttribute("version", "4.0"));
            doc.Add(score);

            var partList = MakePartList(parts);
            score.Add(partList);

            for (var i = 0; i < parts.Length; i++)
            {
                var part = PartToXml(parts[i], i);
                score.Add(part);
            }

            using (var writer = XmlWriter.Create(fileName, new XmlWriterSettings() { Indent = true }))
            {
                doc.WriteTo(writer);
            }
        }

        private XElement MakePartList(Staff[] staves)
        {
            var partList = new XElement("part-list");

            for (var i = 0; i < staves.Length; i++)
            {
                var scorePart = new XElement("score-part",
                    new XAttribute("id", $"P{i+1}"),
                    new XElement("part-name", staves[i].Instrument.ToString()));
                partList.Add(scorePart);
            }

            return partList;
        }

        private XElement PartToXml(Staff part, int index)
        {
            var partElement = new XElement("part");
            partElement.Add(new XAttribute("id", $"P{index + 1}"));

            var topNote = part.Measures.SelectMany(m => m).Max(n => part.Scale.StepToPitch(n.Pitch));
            var bottomNote = part.Measures.SelectMany(m => m).Min(n => part.Scale.StepToPitch(n.Pitch));

            var twoStaves = bottomNote < -2;
            var upperOctaveUp = topNote > 24;
            var lowerOctaveDown = bottomNote < -24;

            for (var i = 0; i < part.MeasureCount; i++)
            {
                var measure = part.Measures[i];

                var measureElement = new XElement("measure");
                measureElement.Add(new XAttribute("number", $"{i + 1}"));

                if (i == 0)
                {
                    var attributes = CreateAttributes(twoStaves, upperOctaveUp, lowerOctaveDown, part.Meter, part.Scale);
                    measureElement.Add(attributes);

                    measureElement.Add(new XElement("sound",
                        new XAttribute("tempo", part.Tempo)));
                }

                var position = 0;
                foreach (var note in part.Measures[i])
                {
                    if (note.StartTime < position)
                    {
                        measureElement.Add(
                            new XElement("backup", 
                            new XElement("duration", position - note.StartTime)));
                    }
                    else if (note.StartTime > position)
                    {
                        measureElement.Add(
                            new XElement("forward",
                            new XElement("duration", note.StartTime - position)));
                    }

                    var noteElement = CreateNote(note, twoStaves, part.Scale);
                    measureElement.Add(noteElement);
                    position = note.EndTime;
                }

                partElement.Add(measureElement);
            }

            return partElement;
        }

        private XElement CreateNote(Note note, bool twoStaves, MusicalScale scale)
        {
            var noteElement = new XElement("note");

            var xPosition = 140 + note.StartTime;
            noteElement.Add(
                new XAttribute("default-x", xPosition));

            var pitch = CreatePitch(note.Pitch, scale);
            noteElement.Add(pitch);

            noteElement.Add(
                new XElement("duration", note.Length));

            noteElement.Add(
                new XElement("type", noteTypes[(NoteValue)note.Length]));

            if (note.Length == (int)NoteValue.HalfDot ||
                note.Length == (int)NoteValue.QuarterDot ||
                note.Length == (int)NoteValue.EighthDot ||
                    note.Length == (int)NoteValue.SixteenthDot)
            {
                noteElement.Add(new XElement("dot"));
            }

            if (twoStaves && scale.StepToPitch(note.Pitch) < 0)
            {
                //noteElement.Add(new XElement("voice", 3));
                noteElement.Add(new XElement("staff", 2));
            }
            else
            {
                //noteElement.Add(new XElement("voice", 1));
                noteElement.Add(new XElement("staff", 1));
            }

            return noteElement;
        }

        private XElement CreatePitch(ScaleStep pitch, MusicalScale scale)
        {
            var step = "CDEFGAB"[pitch.Step];
            var pitchElement = new XElement("pitch");

            pitchElement.Add(new XElement("step", step));

            var mod = (int)pitch.Accidental + scale.StepToPitch(pitch) - MusicalScale.Major.StepToPitch(pitch);
            pitchElement.Add(new XElement("alter", mod));
                        
            pitchElement.Add(new XElement("octave", pitch.Octave + 4));

            return pitchElement;
        }

        private XElement CreateAttributes(bool twoStaves, bool upperOctaveUp, bool lowerOctaveDown, Meter meter, MusicalScale scale)
        {
            var attributes = new XElement("attributes");

            var divisions = new XElement("divisions", (int)NoteValue.Quarter);
            attributes.Add(divisions);

            var key = new XElement("key", 
                new XElement("fifths",  
                scale == MusicalScale.Minor ? -3 : 0));
            attributes.Add(key);

            var time = new XElement("time", 
                new XElement("beats", meter.Top),
                new XElement("beat-type", meter.Bottom));
            attributes.Add(time);

            if (twoStaves)
            {
                var staves = new XElement("staves", 2);
                attributes.Add(staves);
            }

            var clef = new XElement("clef",
                new XAttribute("number", 1),
                new XElement("sign", "G"));
            if (upperOctaveUp)
            {
                clef.Add(new XElement("clef-octave-change", 1));
            }
            attributes.Add(clef);

            if (twoStaves)
            {
                var bassClef = new XElement("clef",
                new XAttribute("number", 2),
                new XElement("sign", "F"));

                if (upperOctaveUp)
                {
                    bassClef.Add(new XElement("clef-octave-change", -1));
                }

                attributes.Add(bassClef);
            }

            return attributes;
        }
    }
}
