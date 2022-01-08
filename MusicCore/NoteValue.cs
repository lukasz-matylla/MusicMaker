namespace MusicCore
{
    public enum NoteValue
    {
        Eighth = 96,
        Quarter = Eighth * 2,
        Half = Quarter * 2,
        Whole = Half * 2,
        Sixteenth = Eighth / 2,
        ThirtySecond = Sixteenth / 2,
        
        WholeDot = Whole + Half,
        HalfDot = Half + Quarter,
        QuarterDot = Quarter + Eighth,
        EighthDot = Eighth + Sixteenth,
        SixteenthDot = Sixteenth + ThirtySecond
    }
}
