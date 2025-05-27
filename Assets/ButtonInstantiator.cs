using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInstantiator : MonoBehaviour
{
    DataLoader dataLoader;

    JsonDataStructure.JsonData jsonData;

    [SerializeField] GameObject buttonToSpawn;
    [SerializeField] Transform buttonParentTrasnform;

List<GameObject> instantiatedButtons = new List<GameObject>();


private void OnEnable()
{
        FindAnyObjectByType<DataLoader>().onDataReloaded += InstantiateButtons;
}

private void OnDisable()
{
    FindAnyObjectByType<DataLoader>().onDataReloaded -= InstantiateButtons;
}

private void Start()
    {
        dataLoader = FindAnyObjectByType<DataLoader>();
        InstantiateButtons();
    }

    void InstantiateButtons()
    {
        jsonData = dataLoader.quizData;

        foreach (GameObject button in instantiatedButtons)
        {
            Destroy(button);
        }
        instantiatedButtons.Clear();
        
        foreach (JsonDataStructure.Category category in dataLoader.quizData.categories)
        {
            
            //Summon button from the pits of hell
            GameObject button = Instantiate(buttonToSpawn, buttonParentTrasnform);
            instantiatedButtons.Add(button);
            //Write text in button
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            buttonText.text = category.name;

            //Deal with color
            int r = category.categoryColorR;
            int g = category.categoryColorG;
            int b = category.categoryColorB;

            Image buttonImage = button.GetComponent<Image>();

            buttonImage.color = new Color(r / 255f, g / 255f, b / 255f);
            
            //Update category id in button
            MainMenuButton buttonScript = button.GetComponent<MainMenuButton>();
            
            buttonScript.categoryID = category.categoryID;
            
        }
    }
}