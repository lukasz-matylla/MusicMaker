using Composer;

namespace MusicMaker
{
    public class BassMakerFactory
    {
        public IBasslineMaker? CreateBassMaker(BassType bassType, ArpeggioPattern bassPattern)
        {
            switch (bassType)
            {
                case BassType.None:
                    return null;
                case BassType.Simple:
                    return new SimpleBasslineMaker();
                case BassType.Rhythmic:
                    return new RhythmicBassMaker();
                case BassType.Arpeggio:
                    return new ArpeggioBasslineMaker(2, bassPattern);
                case BassType.Walking:
                    return new WalkingBasslineMaker();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
