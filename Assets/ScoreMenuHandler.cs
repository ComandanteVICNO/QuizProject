using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMenuHandler : MonoBehaviour
{
    [SerializeField] ScoreTracker scoreTracker;
    [SerializeField] GameDataHolder gameDataHolder;
    [SerializeField] ScoreboardHolder scoreboardHolder;
    
    [SerializeField]private float currentScore;
    private float achievedScore;
    [SerializeField] float maxScore;
    
    [SerializeField] float scorePercentage;
    
    ScoreBoardDataStructure.PlayerScore playerScore;

    [SerializeField] private Button addPlayerButton;
    [SerializeField] private TMP_InputField playerName;
    
    [SerializeField]GameObject scoreboardTextHolder;
    [SerializeField] private TMP_Text playerText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text scoreCounterTitle;
    private void OnEnable()
    {
        scoreboardTextHolder.gameObject.SetActive(false);
        playerName.gameObject.SetActive(true);
        addPlayerButton.gameObject.SetActive(true);

        DataLoader dataloader = FindAnyObjectByType<DataLoader>();
        dataloader.LoadScoreboardData();
        
        maxScore = scoreTracker.maxScore;
        achievedScore = scoreTracker.currentScore;
        
        scorePercentage = (scoreTracker.currentScore / maxScore) * 100;
        StartCoroutine(ScoreAnimation());
    }

    public void AddPlayerToScoreBoard()
    {
        if (playerName.text == "" ) return;
        
        
        playerScore = new ScoreBoardDataStructure.PlayerScore();
        playerScore.playerName = playerName.text;
        playerScore.score = (int)scorePercentage;
        scoreboardHolder.scoreBoardData.scoreBoard.Add(playerScore);
        
        
        playerName.gameObject.SetActive(false);
        addPlayerButton.gameObject.SetActive(false);
        scoreboardHolder.SaveScoreboardData();
        
        DisplayScoreBoard();
    }

    void DisplayScoreBoard()
    {
        scoreText.text = "";
        playerText.text = "";
        
        scoreboardTextHolder.gameObject.SetActive(true);
        List<ScoreBoardDataStructure.PlayerScore> scoreList =
            new List<ScoreBoardDataStructure.PlayerScore>(scoreboardHolder.scoreBoardData.scoreBoard);
        
        List<ScoreBoardDataStructure.PlayerScore> top10List =
            new List<ScoreBoardDataStructure.PlayerScore>();

        
        for (int i = 0; i < 15; i++)
        {
            ScoreBoardDataStructure.PlayerScore highestScore = new ScoreBoardDataStructure.PlayerScore();
            foreach (ScoreBoardDataStructure.PlayerScore score in scoreList)
            {
                if (score.score >= highestScore.score)
                {
                    highestScore = score;
                }
            }

            if (highestScore.score != 0)
            {
                top10List.Add(highestScore);
                
            }
            scoreList.Remove(highestScore);
        }
        
        foreach (ScoreBoardDataStructure.PlayerScore player in top10List)
        {
            playerText.text += player.playerName + "\n";
            scoreText.text += player.score + "%" + "\n";
        }
        
    }

    IEnumerator ScoreAnimation()
    {
        scoreCounterTitle.text = "0 | " + maxScore + " - " + scorePercentage + "%";
        yield return new WaitForSeconds(0.5f);

        while (currentScore < achievedScore)
        {
            currentScore++;
            scoreCounterTitle.text = scoreCounterTitle.text = currentScore + " | " + maxScore + " - " + (int)scorePercentage + "%"; 
            yield return new WaitForSeconds(0.5f);
        }
    }
    
}
