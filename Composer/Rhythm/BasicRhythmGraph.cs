namespace Composer
{
    public class BasicRhythmGraph : PatternGraphBase
    {
        #region duple
        protected RhythmicPattern W = new RhythmicPattern(0, 8);
        protected RhythmicPattern HH = new RhythmicPattern(0, 4, 4);

        protected RhythmicPattern HdQ = new RhythmicPattern(0.5, 6, 2);
        protected RhythmicPattern HQQ = new RhythmicPattern(0.5, 4, 2, 2);
        protected RhythmicPattern Q4 = new RhythmicPattern(0.5, 2, 2, 2, 2);

        protected RhythmicPattern HdEE = new RhythmicPattern(1, 6, 1, 1);
        protected RhythmicPattern HQEE = new RhythmicPattern(1, 4, 2, 1, 1);
        protected RhythmicPattern QHd = new RhythmicPattern(1, 2, 6);
        protected RhythmicPattern QQH = new RhythmicPattern(1, 2, 2, 4);
        protected RhythmicPattern QQQdE = new RhythmicPattern(1, 2, 2, 3, 1);

        protected RhythmicPattern HE4 = new RhythmicPattern(1.5, 4, 1, 1, 1, 1);
        protected RhythmicPattern QdEH = new RhythmicPattern(1.5, 3, 1, 4);
        protected RhythmicPattern QdEQQ = new RhythmicPattern(1.5, 3, 1, 2, 2);
        protected RhythmicPattern QHQ = new RhythmicPattern(1.5, 2, 4, 2);
        protected RhythmicPattern Q3EE = new RhythmicPattern(1.5, 2, 2, 2, 1, 1);
        protected RhythmicPattern QEEH = new RhythmicPattern(1.5, 2, 1, 1, 4);
        protected RhythmicPattern QEEQQ = new RhythmicPattern(1.5, 2, 1, 1, 2, 2);
        protected RhythmicPattern QEEQEE = new RhythmicPattern(1.5, 2, 1, 1, 2, 1, 1);

        protected RhythmicPattern QdEQdE = new RhythmicPattern(2, 3, 1, 3, 1);
        protected RhythmicPattern QdQdQ = new RhythmicPattern(2, 3, 3, 2);
        protected RhythmicPattern QdQdEE = new RhythmicPattern(2, 3, 3, 1, 1);
        protected RhythmicPattern QdQQd = new RhythmicPattern(2, 3, 2, 3);
        protected RhythmicPattern QQdQd = new RhythmicPattern(2, 2, 3, 3);
        protected RhythmicPattern QE4Q = new RhythmicPattern(2, 2, 1, 1, 1, 1, 2);
        protected RhythmicPattern QE6 = new RhythmicPattern(2, 2, 1, 1, 1, 1, 1, 1);
        protected RhythmicPattern EEHd = new RhythmicPattern(2, 1, 1, 6);
        protected RhythmicPattern E4H = new RhythmicPattern(2, 1, 1, 1, 1, 4);
        protected RhythmicPattern E8 = new RhythmicPattern(2, 1, 1, 1, 1, 1, 1, 1, 1);

        protected RhythmicPattern EEHEE = new RhythmicPattern(2.5, 1, 1, 4, 1, 1);
        protected RhythmicPattern EEQQEE = new RhythmicPattern(2.5, 1, 1, 2, 2, 1, 1);
        protected RhythmicPattern EEQEEQ = new RhythmicPattern(2.5, 1, 1, 2, 1, 1, 2);
        #endregion

        #region triple
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
        #endregion

        public BasicRhythmGraph() : base()
        {
            // Duple
            AddSimilarity(W, HH, 0.5);
            AddSimilarity(W, HdQ, 0.8);
            AddSimilarity(W, HQQ, 0.5);
            AddSimilarity(W, Q4, 0.5);
            AddSimilarity(W, QHd, 0.3);
            AddSimilarity(W, EEHd, 0.2);

            AddSimilarity(HH, HdQ, 0.8);
            AddSimilarity(HH, HQQ, 0.5);
            AddSimilarity(HH, QQH, 0.5);
            AddSimilarity(HH, QHd, 0.3);

            // to do

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
