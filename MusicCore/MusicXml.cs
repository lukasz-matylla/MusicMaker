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

            var scaleTransfer = ScaleToCMajor(part.Scale, part.Key);

            for (var i = 0; i < part.MeasureCount; i++)
            {
                var measureElement = new XElement("measure");
                measureElement.Add(new XAttribute("number", $"{i + 1}"));

                if (i == 0)
                {
                    var attributes = CreateAttributes(part.Clef, part.Key, part.Meter, part.Scale);
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

                    var noteElement = CreateNote(note, part.Clef, scaleTransfer);
                    measureElement.Add(noteElement);
                    position = note.EndTime;
                }

                partElement.Add(measureElement);
            }

            return partElement;
        }

        private XElement CreateNote(Note note, Clef clef, ScaleStep[] transfer)
        {
            var noteElement = new XElement("note");

            var pitch = CreatePitch(note.Pitch, clef, transfer);
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

            return noteElement;
        }

        private XElement CreatePitch(ScaleStep pitch, Clef clef, ScaleStep[] transfer)
        {
            var targetPitch = transfer[pitch.Step];
            var targetOctave = targetPitch.Octave + pitch.Octave + (int)clef;
            var targetAccidental = (int)targetPitch.Accidental + (int)pitch.Accidental;

            var pitchElement = new XElement("pitch");

            var step = "CDEFGAB"[targetPitch.Step];
            pitchElement.Add(new XElement("step", step));

            pitchElement.Add(new XElement("alter", targetAccidental));
                        
            pitchElement.Add(new XElement("octave", targetOctave));

            return pitchElement;
        }

        private ScaleStep[] ScaleToCMajor(MusicalScale scale, Key key)
        {
            var tonic = key.ToCMajorNote();
            
            if (scale.Count == MusicalScale.Major.Count)
            {
                var result = new ScaleStep[scale.Count];

                for (var i = 0; i < scale.Count; i++)
                {
                    var note = MusicalScale.Major.ChangeBySteps(tonic, i);
                    var mod = scale.Steps[i] + (int)tonic.Accidental + (int)key - MusicalScale.Major.StepToPitch(note);
                    result[i] = note.WithAccidental((Accidental)mod);
                }

                return result;
            }

            // No handling yet for non-heptatonic scales
            throw new NotImplementedException();
        }

        private XElement CreateAttributes(Clef clef, Key key, Meter meter, MusicalScale scale)
        {
            var attributes = new XElement("attributes");

            var divisions = new XElement("divisions", (int)NoteValue.Quarter);
            attributes.Add(divisions);

            var signature = KeySignature(key, scale);
            var keyElement = new XElement("key", 
                new XElement("fifths",  signature));
            attributes.Add(keyElement);

            var time = new XElement("time", 
                new XElement("beats", meter.Top),
                new XElement("beat-type", meter.Bottom));
            attributes.Add(time);

            var clefType = clef switch
            {
                Clef.Treble8up => "G",
                Clef.Treble => "G",
                Clef.Alto => "C",
                Clef.Bass => "F",
                Clef.Bass8down => "F",
                _ => throw new ArgumentOutOfRangeException(nameof(clef)),
            };

            var clefElement = new XElement("clef",
                new XAttribute("number", 1),
                new XElement("sign", clefType));

            if (clef == Clef.Treble8up)
            {
                clefElement.Add(new XElement("clef-octave-change", 1));
            }
            else if (clef == Clef.Bass8down)
            {
                clefElement.Add(new XElement("clef-octave-change", -1));
            }

            attributes.Add(clefElement);

            return attributes;
        }

        private int KeySignature(Key key, MusicalScale scale)
        {
            var result = key switch
            {
                Key.C => 0,
                Key.G => 1,
                Key.D => 2,
                Key.A => 3,
                Key.E => 4,
                Key.B => 5,
                Key.Gb => -6,
                Key.Db => -5,
                Key.Ab => -4,
                Key.Eb => -3,
                Key.Bb => -2,
                Key.F => -1,
                _ => throw new ArgumentOutOfRangeException(nameof(key))
            };

            if (scale == MusicalScale.Minor)
            {
                result -= 3;
            }

            if (result < -6)
            {
                result += 12;
            }

            return result;
        }
    }
}
