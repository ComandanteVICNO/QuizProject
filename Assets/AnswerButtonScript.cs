using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButtonScript : MonoBehaviour
{
    public bool isAnswerCorrect;
    [SerializeField]TMP_Text buttonTextField;
    QuizHandler quizHandler;
    private Button button;


    public void InitializeButton(string answerText, bool isCorrect)
    {
        isAnswerCorrect = isCorrect;
        buttonTextField.text = answerText;

        quizHandler = FindAnyObjectByType<QuizHandler>();
        button = GetComponent<Button>();

    }

    public void AnswerQuestion()
    {
        quizHandler.ButtonAnswer(isAnswerCorrect);
    }

    public void SetMode()
    {
        if (isAnswerCorrect)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = false;
        }
    }
    
    
}
