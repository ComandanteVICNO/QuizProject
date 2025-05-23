using System;
using TMPro;
using UnityEngine;

public class GameDataHolder : MonoBehaviour
{
    [SerializeField] private int currentCategoryID;

    [SerializeField]public  JsonDataStructure.Category currentCategory;
    [SerializeField] private TMP_Text mainMenuTitle;

    private DataLoader dataLoader;
    private void Start()
    {
        dataLoader = FindAnyObjectByType<DataLoader>();
        mainMenuTitle.text = dataLoader.quizData.gameTitle;
    }

    public void UpdateCategory(int categoryID)
    {
        currentCategoryID = categoryID;

        foreach (JsonDataStructure.Category category in dataLoader.quizData.categories)
        {
            if (category.categoryID == categoryID)
            {
                currentCategory = category;
            }
        }

    }
    
}
