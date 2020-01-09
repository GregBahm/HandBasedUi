using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;

public class PokerMain : MonoBehaviour
{
    public int OpponentsCount;
    public TextAsset HandFrequencyTable;
    public TextMeshPro WinProbabilityText;
    
    private ScoreAnalysisTable scoreAnalysis;
    public HandState HandState;
    public StateScoring Scoring;
    private RandomHandGenerator playerHandGenerator;
    private RandomHandGenerator opponentHandGenerator;

    public static PokerMain Instance;

    public float CardOptionMargin;
    public const float CardHeight = 1.62f;

    public GameObject CardOptionPrefab;

    private StringBuilder outputBuilder = new StringBuilder();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        scoreAnalysis = new ScoreAnalysisTable(HandFrequencyTable);
        HandState = new HandState();
        RefreshGenerators();
    }

    public Hand GetRandomPlayerHand()
    {
        return playerHandGenerator.GetRandomHand();
    }
    
    public Hand GetRandomOpponentHand()
    {
        return opponentHandGenerator.GetRandomHand();
    }

    private void ScoreSomeHands()
    {
        Hand playerHand = GetRandomPlayerHand();
        Hand[] opponentHands = new Hand[OpponentsCount];
        for (int i = 0; i < OpponentsCount; i++)
        {
            opponentHands[i] = GetRandomOpponentHand();
        }
        Scoring.RegisterHandScore(playerHand, opponentHands);
    }

    private void Update()
    {
        RefreshGenerators();
        UpdateWinProbabilityLabel();
        ScoreSomeHands();
    }

    private void UpdateWinProbabilityLabel()
    {
        outputBuilder.Clear();
        outputBuilder.AppendLine("Rounds: " + Scoring.RoundsPlayed);
        outputBuilder.AppendLine("Wins: " + Scoring.Wins + "\tLosses: " + Scoring.Losses + "\tTies: " + Scoring.Ties);
        outputBuilder.AppendLine("Win Probability: " + (int)(Scoring.WinProbability * 100) + "%");

        WinProbabilityText.text = outputBuilder.ToString();
    }

    private void RefreshGenerators()
    {
        if (!HandState.PlayerGeneratorUpToDate || !HandState.OpponentGeneratorUpToDate)
        {
            playerHandGenerator = HandState.GetPlayerHandGenerator();
            opponentHandGenerator = HandState.GetOpponentHandGenerator();
            Scoring = new StateScoring(scoreAnalysis);
        }
    }
}
