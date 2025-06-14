using System;
using UnityEngine;

public class ScoreboardHolder : MonoBehaviour
{
    public ScoreBoardDataStructure.ScoreBoardData scoreBoardData;
    
    [SerializeField] DataLoader dataLoader;

    private void Start()
    {
        Debug.Log("This RAN");
    }

    public void SaveScoreboardData()
    {
        dataLoader.SaveScoreBoardData(scoreBoardData);
    }
    
}
