namespace CrazyWeasol.Progression
{
    public enum ChaosRank { D, C, B, A, S, SPlus, Crash }
    public static class ChaosRankResolver
    {
        public static ChaosRank Resolve(int score)
        {
            if (score >= 1000) return ChaosRank.Crash;
            if (score >= 750) return ChaosRank.SPlus;
            if (score >= 550) return ChaosRank.S;
            if (score >= 400) return ChaosRank.A;
            if (score >= 275) return ChaosRank.B;
            if (score >= 150) return ChaosRank.C;
            return ChaosRank.D;
        }
    }
}
