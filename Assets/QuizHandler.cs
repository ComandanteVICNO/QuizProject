using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class QuizHandler : MonoBehaviour
{
    private int curretQuestionIndex = 0;
    
    GameDataHolder gameDataHolder;
    [SerializeField] private List<JsonDataStructure.Question> questionsList;
    [SerializeField] private List<JsonDataStructure.Answer> answersList;
    [SerializeField] private List<int> answerResponseValueList;
    
    
    
    //Ui decoration
    [Header("UI Elements")]
    [SerializeField] private Image backgroundPanelImage;
    
    
    //Button Instance stuff
    [Header("Button stuff")]
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject buttonParent;
    [SerializeField] private List<GameObject> buttonList;
    
    
    //Text and stuff
    [Header("Text Stuff")]
    [SerializeField] private TMP_Text questionTitle;
    [SerializeField] private TMP_Text tipTitle;
    [SerializeField] private TMP_Text explanationTitle;
    
    //Skip buttons
    [Header("Skip Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;
    
    [SerializeField] Color correctButtonColor;
    [SerializeField] Color incorrectButtonColor;
    
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
            answerResponseValueList.Add(-1);
            
        }
        curretQuestionIndex = 0;
        
        //Update Quiz with data
        UpdateQuizQuestion(curretQuestionIndex);
        
        
        //Update UI Panel
        backgroundPanelImage.color = new Color(gameDataHolder.currentCategory.categoryColorR/255f, gameDataHolder.currentCategory.categoryColorG / 255f, gameDataHolder.currentCategory.categoryColorB / 255f);
    }

    private void OnDisable()
    {
        answerResponseValueList.Clear();
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
        
        //add answers to list
        answersList.Clear();
        foreach (JsonDataStructure.Answer answer in questionsList[questionIndex].answers )
        {
            answersList.Add(answer);
        }
        
        
        foreach (JsonDataStructure.Answer answer in answersList)
        {
            //for each answer add a button and add that button to a list
            GameObject button = Instantiate(buttonPrefab, buttonParent.transform);
            buttonList.Add(button);

            //acess the button script and initalize button
            AnswerButtonScript buttonScript = button.GetComponent<AnswerButtonScript>();
            buttonScript.InitializeButton(answer.answer, answer.isAnswerCorrect); 
            //If question was already answered, set button modes and colors
            if (answerResponseValueList[questionIndex] == 0)
            {
                buttonScript.SetMode();
                if (buttonScript.isAnswerCorrect)
                {
                    button.GetComponent<Image>().color = correctButtonColor;
                }
                else
                {
                    button.GetComponent<Image>().color = incorrectButtonColor;
                }
                explanationTitle.text = questionsList[questionIndex].explanation;
            }
            else if (answerResponseValueList[questionIndex] == 1)
            {
                buttonScript.SetMode();
                if (buttonScript.isAnswerCorrect)
                {
                    button.GetComponent<Image>().color = correctButtonColor;
                }
                explanationTitle.text = questionsList[questionIndex].explanation;
            }
        }
        
        
        
        questionTitle.text = questionsList[questionIndex].question;
        //tipTitle.text = questionsList[questionIndex].tip;
       // explanationTitle.text = questionsList[questionIndex].explanation;

        
        
        //Disable or Enable skip Buttons
        
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
        ToogleTip(false);
        UpdateQuizQuestion(curretQuestionIndex);
    }

    public void QuizQuestionBackward()
    {
        curretQuestionIndex--;
        ToogleTip(false);
        UpdateQuizQuestion(curretQuestionIndex);
    }

    public void ButtonAnswer(bool isCorrect)
    {
        explanationTitle.text = questionsList[curretQuestionIndex].explanation;
        
        if (isCorrect)
        {
            Debug.Log("Correct!!!");
            answerResponseValueList[curretQuestionIndex] = 1;
        }
        else
        {
            Debug.Log("Wrong!!!");
            answerResponseValueList[curretQuestionIndex] = 0;
        }

        foreach (GameObject button in buttonList)
        {
            AnswerButtonScript buttonScript = button.GetComponent<AnswerButtonScript>();
            buttonScript.SetMode();
            if (isCorrect)
            {
                if (buttonScript.isAnswerCorrect)
                {
                    button.GetComponent<Image>().color = correctButtonColor;
                }
            }
            else
            {
                if (buttonScript.isAnswerCorrect)
                {
                    button.GetComponent<Image>().color = correctButtonColor;
                }
                else
                {
                    button.GetComponent<Image>().color = incorrectButtonColor;
                }
            }
        }
        
    }

    public void ToogleTip(bool isTipShown)
    {
        if (isTipShown)
        {
            tipTitle.text = questionsList[curretQuestionIndex].tip;
        }
        else
        {
            tipTitle.text = "";
        }
    }
    
}
