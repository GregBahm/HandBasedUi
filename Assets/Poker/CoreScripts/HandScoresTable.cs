using System;
using System.Collections.Generic;
using System.Linq;

public class HandScoresTable
{
    public static IReadOnlyList<HandScoreDescription> StraightFlushes { get; }
    public static IReadOnlyList<HandScoreDescription> FourOfAKinds { get; }
    public static IReadOnlyList<HandScoreDescription> FullHouses { get; }
    public static IReadOnlyList<HandScoreDescription> Flushes { get; }
    public static IReadOnlyList<HandScoreDescription> Straights { get; }
    public static IReadOnlyList<HandScoreDescription> ThreeOfAKinds { get; }
    public static IReadOnlyList<HandScoreDescription> TwoPairs { get; }
    public static IReadOnlyList<HandScoreDescription> Pairs { get; }
    public static IReadOnlyList<HandScoreDescription> HighCards { get; }

    public static IReadOnlyDictionary<string, HandScoreDescription> All { get; }
    public static IReadOnlyDictionary<string, int> Scores { get; }

    static HandScoresTable()
    {
        StraightFlushes = HandScoreDescription.GetStraightFlushes().ToList().AsReadOnly();
        FourOfAKinds = HandScoreDescription.GetFourOfAKinds().ToList().AsReadOnly();
        FullHouses = HandScoreDescription.GetFullHouses().ToList().AsReadOnly();
        Flushes = HandScoreDescription.GetFlushes().ToList().AsReadOnly();
        Straights = HandScoreDescription.GetStraights().ToList().AsReadOnly();
        ThreeOfAKinds = HandScoreDescription.GetThreeOfAKinds().ToList().AsReadOnly();
        TwoPairs = HandScoreDescription.GetTwoPairs().ToList().AsReadOnly();
        Pairs = HandScoreDescription.GetPairs().ToList().AsReadOnly();
        HighCards = HandScoreDescription.GetHighCards().ToList().AsReadOnly();

        List<HandScoreDescription> all = new List<HandScoreDescription>();
        all.AddRange(StraightFlushes);
        all.AddRange(FourOfAKinds);
        all.AddRange(FullHouses);
        all.AddRange(Flushes);
        all.AddRange(Straights);
        all.AddRange(ThreeOfAKinds);
        all.AddRange(TwoPairs);
        all.AddRange(Pairs);
        all.AddRange(HighCards);
        All = all.ToDictionary(item => item.Key, item => item);
        Scores = GetScoreProbabilities();
    }

    private static Dictionary<string, int> GetScoreProbabilities()
    {
        Dictionary<string, int> ret = new Dictionary<string, int>();
        int rank = 0;
        foreach (KeyValuePair<string, HandScoreDescription> entry in All)
        {
            ret.Add(entry.Key, rank);
            rank++;
        }
        return ret;
    }
}
