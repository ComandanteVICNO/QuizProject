using TMPro;
using UnityEngine;

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
            GameObject button = Instantiate(buttonToSpawn, buttonParentTrasnform);

            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();

            buttonText.text = category.name;
        }
        
        
    }
}
