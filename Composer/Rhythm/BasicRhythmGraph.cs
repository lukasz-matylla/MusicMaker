namespace Composer
{
    public class BasicRhythmGraph : PatternGraphBase
    {
        protected RhythmicPattern H = new RhythmicPattern(0, 8);
        protected RhythmicPattern QQ = new RhythmicPattern(0, 4, 4);

        protected RhythmicPattern QdE = new RhythmicPattern(0.5, 6, 2);
        protected RhythmicPattern QEE = new RhythmicPattern(0.5, 4, 2, 2);
        protected RhythmicPattern EEEE = new RhythmicPattern(0.5, 2, 2, 2, 2);

        protected RhythmicPattern EQd = new RhythmicPattern(1, 2, 6);
        protected RhythmicPattern EEQ = new RhythmicPattern(1, 2, 2, 4);
        protected RhythmicPattern EQE = new RhythmicPattern(1, 2, 4, 2);


        protected RhythmicPattern Hd = new RhythmicPattern(0, 12);

        protected RhythmicPattern HQ = new RhythmicPattern(0.5, 8, 4);
        protected RhythmicPattern QQQ = new RhythmicPattern(0.5, 4, 4, 4);
        protected RhythmicPattern HEE = new RhythmicPattern(0.5, 8, 2, 2);

        protected RhythmicPattern QQEE = new RhythmicPattern(1, 4, 4, 2, 2);
        protected RhythmicPattern QdQd = new RhythmicPattern(1, 6, 6);

        protected RhythmicPattern QEEEE = new RhythmicPattern(1.5, 4, 2, 2, 2, 2);
        protected RhythmicPattern QEEQ = new RhythmicPattern(1.5, 4, 2, 2, 4);
        protected RhythmicPattern QH = new RhythmicPattern(1.5, 4, 8);
        protected RhythmicPattern E6 = new RhythmicPattern(1.5, 2, 2, 2, 2, 2, 2);

        protected RhythmicPattern EEH = new RhythmicPattern(2, 2, 2, 8);
        protected RhythmicPattern EEQQ = new RhythmicPattern(2, 2, 2, 4, 4);
        protected RhythmicPattern EEQEE = new RhythmicPattern(2, 2, 2, 4, 2, 2);

        public BasicRhythmGraph() : base()
        {
            // Duple
            AddSimilarity(H, QQ, 0.5);
            AddSimilarity(H, QdE, 0.8);

            AddSimilarity(QQ, QdE, 0.8);
            AddSimilarity(QQ, QEE, 0.5);
            AddSimilarity(QQ, EEQ, 0.5);
            AddSimilarity(QQ, EQd, 0.5);

            AddSimilarity(QdE, QEE, 0.8);

            AddSimilarity(QEE, EEEE, 0.8);

            AddSimilarity(EQd, EEQ, 0.8);
            AddSimilarity(EQd, EQE, 0.8);

            AddSimilarity(EEQ, EEEE, 0.8);

            // Triple
            AddSimilarity(Hd, HQ, 0.8);
            AddSimilarity(Hd, QdQd, 0.5);
            AddSimilarity(Hd, QH, 0.5);

            AddSimilarity(HQ, QQQ, 0.8);
            AddSimilarity(HQ, HEE, 0.8);
            AddSimilarity(HQ, QdQd, 0.5);

            AddSimilarity(QQQ, QQEE, 0.8);
            AddSimilarity(QQQ, QH, 0.5);
            AddSimilarity(QQQ, QEEQ, 0.5);
            AddSimilarity(QQQ, EEQQ, 0.5);

            AddSimilarity(HEE, QEEEE, 0.8);

            AddSimilarity(QQEE, QEEEE, 0.8);
            AddSimilarity(QQEE, EEQEE, 0.5);

            AddSimilarity(QdQd, QH, 0.5);

            AddSimilarity(QEEEE, E6, 0.8);

            AddSimilarity(QEEQ, QEEEE, 0.8);

            AddSimilarity(QH, EEH, 0.8);

            AddSimilarity(EEQQ, EEQEE, 0.8);
        }
    }
}
