using System.Collections.Generic;
using System.Linq;

public class StateScoring
{
    private ScoreAnalysisTable scoreAnalysis;

    public int RoundsPlayed { get; private set; }
    public int Wins { get; private set; }
    public int Losses { get; private set; }

    public int Ties { get { return RoundsPlayed - (Wins + Losses); } }

    public float WinProbability { get { return RoundsPlayed == 0 ? .5f : (float)Wins / RoundsPlayed; } }
    public float LossProbability { get { return RoundsPlayed == 0 ? .5f : (float)Losses / RoundsPlayed; } }

    private int[] selfScores;
    private int[] opponentScores;

    public StateScoring(ScoreAnalysisTable scoreAnalysis)
    {
        this.scoreAnalysis = scoreAnalysis;
        selfScores = new int[ScoreAnalysisTable.UniqueSevenCardHands];
        opponentScores = new int[ScoreAnalysisTable.UniqueSevenCardHands];
    }

    public void RegisterHandScore(Hand playerHand, IEnumerable<Hand> opponentHands)
    {
        int playerScore = GetPlayerScore(playerHand);
        int opponentScore = GetOpponentScore(opponentHands);

        selfScores[playerScore]++;
        opponentScores[opponentScore]++;

        if (playerScore < opponentScore)
        {
            Wins++;
        }
        else if(opponentScore < playerScore)
        {
            Losses++;
        }
        RoundsPlayed++;
    }

    private int GetPlayerScore(Hand playerHand)
    {
        return scoreAnalysis.GetSevenCardRanking(playerHand);
    }

    private int GetOpponentScore(IEnumerable<Hand> opponentHands)
    {
        int opponentScore = opponentHands.Min(item => item.Score);
        return scoreAnalysis.GetSevenCardRanking(opponentScore);
    }
}
