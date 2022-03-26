using Composer;

namespace MusicMaker
{
    public class BassMakerFactory
    {
        public IBasslineMaker? CreateBassMaker(BassType bassType, AlbertiPattern bassPattern)
        {
            switch (bassType)
            {
                case BassType.None:
                    return null;
                case BassType.Simple:
                    return new SimpleBasslineMaker();
                case BassType.Rhythmic:
                    return new RhythmicBassMaker();
                case BassType.Alberti:
                    var notesPerBeat = bassPattern ==
                        AlbertiPattern.UpDown ? 2 : 1;
                    return new AlbertiBassMaker(notesPerBeat, bassPattern);
                case BassType.Walking:
                    return new WalkingBasslineMaker();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
