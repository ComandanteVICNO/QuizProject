using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoardDataStructure : MonoBehaviour
{
    [Serializable]
    public class ScoreBoardData
    {
        public List<PlayerScore> scoreBoard;
    }

    [Serializable]
    public class PlayerScore
    {
        public string playerName;
        public int score;
    }
}
