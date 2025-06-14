using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizHandler : MonoBehaviour
{
    private int currentQuestionIndex = 0;
    
    GameDataHolder gameDataHolder;
    private List<JsonDataStructure.Question> easyQuestions;
    private List<JsonDataStructure.Question> normalQuestions;
    private List<JsonDataStructure.Question> hardQuestions;
    
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
    [SerializeField] private GameObject submitButton;
    
    [Header("Colors")]
    [SerializeField] Color correctButtonColor;
    [SerializeField] Color incorrectButtonColor;
    
    [Header("Refereces")]
    [SerializeField] ScoreTracker scoreTracker;
    
    private void Awake()
    {
        gameDataHolder = FindAnyObjectByType<GameDataHolder>();
    }

    private void OnEnable()
    {
        easyQuestions = new List<JsonDataStructure.Question>();
        normalQuestions = new List<JsonDataStructure.Question>();
        hardQuestions= new List<JsonDataStructure.Question>();
        //Grab question data depending on difficulty
        foreach (JsonDataStructure.Question question in gameDataHolder.currentCategory.questions)
        {
            if (question.difficulty == 0)
            {
                easyQuestions.Add(question);
            }
            else if (question.difficulty == 1)
            {
                normalQuestions.Add(question);
            }
            else if (question.difficulty == 2)
            {
                hardQuestions.Add(question);
            }
            answerResponseValueList.Add(-1);
            
        }
        
        //Shuffle difficulty lists
        ShuffleList(easyQuestions);
        ShuffleList(normalQuestions);
        ShuffleList(hardQuestions);

        foreach (JsonDataStructure.Question question in easyQuestions)
        {
            questionsList.Add(question);   
        }
        foreach (JsonDataStructure.Question question in normalQuestions)
        {
            questionsList.Add(question);   
        }
        foreach (JsonDataStructure.Question question in hardQuestions)
        {
            questionsList.Add(question);   
        }
        
        currentQuestionIndex = 0;
        //Update Quiz with data
        UpdateQuizQuestion(currentQuestionIndex);
        
        
        //Update UI Panel
        backgroundPanelImage.color = new Color(gameDataHolder.currentCategory.categoryColorR/255f, gameDataHolder.currentCategory.categoryColorG / 255f, gameDataHolder.currentCategory.categoryColorB / 255f);
        
        scoreTracker.maxScore = questionsList.Count;
        scoreTracker.currentScore = 0;
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
        
        ShuffleList(answersList);
        
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
        if (currentQuestionIndex <= 0)
        {
            backButton.interactable = false;
        }
        else
        {
            backButton.interactable = true;
        }

        if (currentQuestionIndex >= maxIndex)
        {
            nextButton.interactable = false;
            nextButton.gameObject.SetActive(false);
            submitButton.SetActive(true);
        }
        else
        {
            nextButton.interactable = true;
            nextButton.gameObject.SetActive(true);
            submitButton.SetActive(false);
        }
        
        
    }

    public void QuizQuestionForward()
    {
        explanationTitle.text = "";
        currentQuestionIndex++;
        ToggleTip(false);
        UpdateQuizQuestion(currentQuestionIndex);
    }

    public void QuizQuestionBackward()
    {
        explanationTitle.text = "";
        currentQuestionIndex--;
        ToggleTip(false);
        UpdateQuizQuestion(currentQuestionIndex);
    }

    public void ButtonAnswer(bool isCorrect)
    {
        explanationTitle.text = questionsList[currentQuestionIndex].explanation;
        
        if (isCorrect)
        {
            Debug.Log("Correct!!!");
            answerResponseValueList[currentQuestionIndex] = 1;
            scoreTracker.currentScore++;
        }
        else
        {
            Debug.Log("Wrong!!!");
            answerResponseValueList[currentQuestionIndex] = 0;
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

    public void ToggleTip(bool isTipShown)
    {
        if (isTipShown)
        {
            tipTitle.text = questionsList[currentQuestionIndex].tip;
        }
        else
        {
            tipTitle.text = "";
        }
    }


    void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1); // includes i
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
    
}
