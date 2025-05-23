using UnityEngine;

public class MenuToggler : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] private GameObject quizMenu;


    public void ActivateMainMenu()
    {
        quizMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ActivateQuizMenu()
    {
        quizMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    
}
