using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInstantiator : MonoBehaviour
{
    DataLoader dataLoader;

    JsonDataStructure.JsonData jsonData;

    [SerializeField] GameObject buttonToSpawn;
    [SerializeField] Transform buttonParentTrasnform;

    private void Start()
    {
        dataLoader = FindAnyObjectByType<DataLoader>();

        jsonData = dataLoader.quizData;

        foreach(JsonDataStructure.Category category in dataLoader.quizData.categories)
        {
            //Summon button from the pits of hell
            GameObject button = Instantiate(buttonToSpawn, buttonParentTrasnform);

            //Write text in button
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            buttonText.text = category.name;
            
            //Deal with color
            int r = category.categoryColorR;
            int g = category.categoryColorG;
            int b = category.categoryColorB;

            Image buttonImage = button.GetComponent<Image>();
            
            buttonImage.color = new Color(r/255f, g/255f, b/255f);
        }
        
        
    }
}
