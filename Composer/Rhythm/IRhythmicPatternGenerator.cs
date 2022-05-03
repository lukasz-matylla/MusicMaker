using MusicCore;

namespace Composer
{
    public interface IRhythmicPatternGenerator
    {
        // Notes at step 0 are strong beats, others are weak beats/notes
        Staff CreateRhythm(int measures, Meter meter, Staff? contrastTo = null);

        // Notes at step 0 are strong beats, others are weak beats/notes
        Staff CreateRhythm(Staff toFill)
        {
            return CreateRhythm(toFill.MeasureCount, toFill.Meter);
        }
    }
}
