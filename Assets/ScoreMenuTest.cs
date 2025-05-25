using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMenuTest : MonoBehaviour
{
    [SerializeField] private int maxScore;
    [SerializeField] private int userScore;
    [SerializeField] private int currentScore;
    [SerializeField] float timeUntilMaxScore;
    private float timeBetweenScoreJumps;

    [SerializeField] private TMP_Text scoreField;

    [SerializeField] Image scoreImage;
    [SerializeField] private float maxImageValue = 1;
    [SerializeField] private float minImageValue = 0;
    [SerializeField] private float currentImageValue;
    float normalizedImageValue;
    private float timeForImageToComplete;
    private void Start()
    {
        timeBetweenScoreJumps = timeUntilMaxScore / maxScore;
        StartCoroutine(DisplayScore());

        normalizedImageValue = (userScore - 0) / (maxScore - 0);

        timeForImageToComplete = timeUntilMaxScore / normalizedImageValue;

    }

    private void Update()
    {
        scoreImage.fillAmount += timeForImageToComplete * Time.deltaTime;
    }

    IEnumerator DisplayScore()
    {
        yield return new WaitForSeconds(2f);

        while (currentScore < userScore)
        {
            currentScore++;
            scoreField.text = currentScore + " / " + maxScore;
            yield return new WaitForSeconds(timeBetweenScoreJumps);
        }
        
    }
}
