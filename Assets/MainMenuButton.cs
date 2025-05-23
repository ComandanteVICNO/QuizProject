using System;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuButton : MonoBehaviour
{

    public int categoryID;
    private GameDataHolder gameData;
    
    MenuToggler menuToggler;
    
    private void Start()
    {
         gameData = FindAnyObjectByType<GameDataHolder>();
         menuToggler = FindAnyObjectByType<MenuToggler>();
    }

    public void UpdateCategoryID()
    {
        gameData.UpdateCategory(categoryID);
        menuToggler.ActivateQuizMenu();
    }


}
