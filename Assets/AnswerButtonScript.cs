using TMPro;
using UnityEngine;

public class AnswerButtonScript : MonoBehaviour
{
    private bool isAnswerCorrect;
    [SerializeField]TMP_Text buttonTextField;
    QuizHandler quizHandler;


    public void InitializeButton(string answerText, bool isCorrect)
    {
        isAnswerCorrect = isCorrect;
        buttonTextField.text = answerText;

        quizHandler = FindAnyObjectByType<QuizHandler>();

    }

    public void AnswerQuestion()
    {
        quizHandler.ButtonAnswer(isAnswerCorrect);
    }
    
    
}
