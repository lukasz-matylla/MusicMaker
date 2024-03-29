﻿using MusicCore;

namespace Composer
{
    public class ClassicalMajorChordProgressionGraph : SimpleMajorChordProgressionGraph
    {
        public override MusicalScale Scale => MusicalScale.Major;

        protected readonly Chord IVsus2 = new Chord(3, 4, 0);
        protected readonly Chord N = new Chord(new ScaleStep(1, Accidental.Flat), 3, new ScaleStep(5, Accidental.Flat)).Inversion(1);
        protected readonly Chord Vsus4 = new Chord(4, 0, 1);
        protected readonly Chord Fr = new Chord(new ScaleStep(5, Accidental.Flat), 0, 1, new ScaleStep(3, Accidental.Sharp));
        protected readonly Chord Ger = new Chord(new ScaleStep(5, Accidental.Flat), 0, new ScaleStep(2, Accidental.Flat), new ScaleStep(3, Accidental.Sharp));
        protected readonly Chord vii00 = new Chord(6, 1, 3, new ScaleStep(5, Accidental.Flat));
        protected readonly Chord bVII = new Chord(new ScaleStep(6, Accidental.Flat), 1, 3);
        protected readonly Chord bVI = new Chord(new ScaleStep(5, Accidental.Flat), 0, 2);

        public ClassicalMajorChordProgressionGraph()
            : base()
        {
            AddTransition(I, V.Inversion(1));
            AddTransition(I, V.Inversion(2));
            AddTransition(I, Vsus4);
            AddTransition(I, vi);
            AddTransition(I, bVII);

            AddTransition(I.Inversion(2), Vsus4);

            AddTransition(ii, N);
            AddTransition(ii, Fr);
            AddTransition(ii, Ger);
            AddTransition(ii, vii00);
            AddTransition(ii, bVII);

            AddTransition(V.Inversion(1), I);
            AddTransition(V.Inversion(1), vi);
            AddTransition(V.Inversion(2), I);
            AddTransition(V.Inversion(2), iii);

            AddTransition(IV, N);
            AddTransition(IV, Fr);
            AddTransition(IV, Ger);
            AddTransition(IV, Vsus4);
            AddTransition(IV, bVII);

            AddTransition(IVsus2, I);
            AddTransition(IVsus2, IV);
            AddTransition(IVsus2, Vsus4);

            AddTransition(N, I);
            AddTransition(N, IVsus2);
            AddTransition(N, V);
            AddTransition(N, V7);

            AddTransition(Vsus4, V);
            AddTransition(Vsus4, V7);

            AddTransition(I.Inversion(2), Vsus4);

            AddTransition(Fr, V);
            AddTransition(Fr, V7);

            AddTransition(Ger, V);
            AddTransition(Ger, V7);

            AddTransition(vii0, vii00);

            AddTransition(vii00, I.Inversion(2));
            AddTransition(vii00, V7, 3);
            AddTransition(vii00, vi);
            AddTransition(vii00, bVII);

            AddTransition(bVII, I);
            AddTransition(bVII, bVI);

            AddTransition(bVI, V);
            AddTransition(bVI, V7);

        }
    }
}
