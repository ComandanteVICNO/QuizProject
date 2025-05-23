using System;
using UnityEngine;

public class GameDataHolder : MonoBehaviour
{
    [SerializeField] private int currentCategoryID;

    [SerializeField]public  JsonDataStructure.Category currentCategory;

    private DataLoader dataLoader;
    private void Start()
    {
        dataLoader = FindAnyObjectByType<DataLoader>();
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
