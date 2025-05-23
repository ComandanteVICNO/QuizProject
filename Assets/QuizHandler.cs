using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizHandler : MonoBehaviour
{
    private int curretQuestionIndex = 0;
    
    GameDataHolder gameDataHolder;
    [SerializeField] private List<JsonDataStructure.Question> questionsList;
    [SerializeField] private List<JsonDataStructure.Answer> answersList;
    
    //Ui decoration
    [SerializeField] private Image backgroundPanelImage;
    
    
    //Button Instance stuff
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject buttonParent;
    [SerializeField] private List<GameObject> buttonList;
    
    
    //Text and stuff
    [SerializeField] private TMP_Text questionTitle;
    [SerializeField] private TMP_Text tipTitle;
    [SerializeField] private TMP_Text explanationTitle;
    
    //Skip buttons
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;
    private void Awake()
    {
        gameDataHolder = FindAnyObjectByType<GameDataHolder>();
    }

    private void OnEnable()
    {
        //Grab question data
        foreach (JsonDataStructure.Question question in gameDataHolder.currentCategory.questions)
        {
            questionsList.Add(question);
        }
        curretQuestionIndex = 0;
        
        //Update Quiz with data
        UpdateQuizQuestion(curretQuestionIndex);
        
        
        //Update UI Panel
        backgroundPanelImage.color = new Color(gameDataHolder.currentCategory.categoryColorR/255f, gameDataHolder.currentCategory.categoryColorG / 255f, gameDataHolder.currentCategory.categoryColorB / 255f);
    }

    private void OnDisable()
    {
        questionsList.Clear();
    }


    void UpdateQuizQuestion(int questionIndex)
    {
        //Kill all buttons and clear list
        foreach (GameObject button in buttonList)
        {
            Destroy(button);
        }
        buttonList.Clear();
        
        answersList.Clear();
        foreach (JsonDataStructure.Answer answer in questionsList[questionIndex].answers )
        {
            answersList.Add(answer);
        }
        
        
        foreach (JsonDataStructure.Answer answer in answersList)
        {
            GameObject button = Instantiate(buttonPrefab, buttonParent.transform);
            buttonList.Add(button);

            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            buttonText.text = answer.answer;
        }

        questionTitle.text = questionsList[questionIndex].question;
        tipTitle.text = questionsList[questionIndex].tip;
        explanationTitle.text = questionsList[questionIndex].explanation;

        
        
        //Disable or Enable Buttons
        
        int maxIndex = questionsList.Count - 1;
        if (curretQuestionIndex <= 0)
        {
            backButton.interactable = false;
        }
        else
        {
            backButton.interactable = true;
        }

        if (curretQuestionIndex >= maxIndex)
        {
            nextButton.interactable = false;
        }
        else
        {
            nextButton.interactable = true;
        }
        
        
    }

    public void QuizQuestionForward()
    {
        curretQuestionIndex++;
        UpdateQuizQuestion(curretQuestionIndex);
    }

    public void QuizQuestionBackward()
    {
        curretQuestionIndex--;
        UpdateQuizQuestion(curretQuestionIndex);
    }
    
    
}
