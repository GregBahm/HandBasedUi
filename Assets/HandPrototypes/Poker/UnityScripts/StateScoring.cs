using System;
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

    public int[] SelfScores { get; } = new int[ScoreAnalysisTable.UniqueSevenCardHands];
    public int[] OpponentScores { get; } = new int[ScoreAnalysisTable.UniqueSevenCardHands];

    public StateScoring(ScoreAnalysisTable scoreAnalysis)
    {
        this.scoreAnalysis = scoreAnalysis;
    }

    public void RegisterHandScore(Hand playerHand, IEnumerable<Hand> opponentHands)
    {
        int playerScore = GetPlayerScore(playerHand);
        int opponentScore = GetOpponentScore(opponentHands);
        UpdateTableData(playerScore, opponentScore);
    }

    private void UpdateTableData(int playerScore, int opponentScore)
    {
        SelfScores[playerScore]++;
        OpponentScores[opponentScore]++;

        if (playerScore < opponentScore)
        {
            Wins++;
        }
        else if (opponentScore < playerScore)
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
