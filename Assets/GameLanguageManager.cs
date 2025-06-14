using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameLanguageManager : MonoBehaviour
{
    public LanguageDataStructure.LanguageData gameLanguageData;
    public LanguageDataStructure.Localization currentLanguage;
    public DataLoader dataLoader;

    public Sprite loadedSprite;
    [SerializeField] private Image image;


    [SerializeField] private List<LanguageDataStructure.Localization> languageList;
    [SerializeField] private int currentLanguageIndex = 0;

    
    
    [Header("Localization Titles")] [SerializeField]
    private TMP_Text gameTitle;

    [SerializeField] private TMP_Text languageMenuTitle;
    [SerializeField] private TMP_Text languageMenuName;
    [SerializeField] private Image languageMenuImage;
    [SerializeField] private TMP_Text languageMenuButtonTitle;
    [SerializeField] private TMP_Text mainMenuButtonTitle;
    [SerializeField] private TMP_Text tipButtonTitle;
    [Header("Pagination Buttons")]
    [SerializeField] private Button paginationNextButton;
    [SerializeField] private Button paginationPreviousButton;
    
    [Header("Score Menu Stuff")]
    [SerializeField] private TMP_Text submitAnswersButton;
    [SerializeField] private TMP_Text scoreMenuTitle;
    [SerializeField] private TMP_Text scoreInputFieldPlaseHolderText;
    [SerializeField] private TMP_Text addPlayerButton;
    private void Start()
    {
        
        dataLoader = FindAnyObjectByType<DataLoader>();
        
        languageMenuTitle.text = gameLanguageData.languageList[currentLanguageIndex].menuLanguage.languageMenuTitle;
        languageMenuButtonTitle.text = gameLanguageData.languageList[currentLanguageIndex].menuLanguage.languageMenuButton;
        languageMenuName.text = gameLanguageData.languageList[currentLanguageIndex].name;
        languageMenuImage.sprite = dataLoader.LoadSprite(gameLanguageData.languageList[currentLanguageIndex].imageFileName);
        
        gameTitle.text = gameLanguageData.languageList[currentLanguageIndex].menuLanguage.gameTitle;
        mainMenuButtonTitle.text = gameLanguageData.languageList[currentLanguageIndex].menuLanguage.mainMenuButton;
        tipButtonTitle.text = gameLanguageData.languageList[currentLanguageIndex].menuLanguage.tipButton;

        submitAnswersButton.text = gameLanguageData.languageList[currentLanguageIndex].menuLanguage.submitButton;
        scoreMenuTitle.text = gameLanguageData.languageList[currentLanguageIndex].menuLanguage.scoreTitle;
        scoreInputFieldPlaseHolderText.text = gameLanguageData.languageList[currentLanguageIndex].menuLanguage.playerInputFieldText;
        addPlayerButton.text = gameLanguageData.languageList[currentLanguageIndex].menuLanguage.addPlayerButton;
        ApplyLanguageToGame();
    }

    public void SetDefaultLanguage()
    {
        currentLanguage = gameLanguageData.languageList[0];
    }
    
    

    public void StartLanguageUI()
    {
        //dataLoader.CheckAndSaveLocalizationImages();
        languageList = new List<LanguageDataStructure.Localization>();
        foreach (LanguageDataStructure.Localization language in gameLanguageData.languageList)
        {
            languageList.Add(language);
        }
        //Update UI
        languageMenuTitle.text = languageList[currentLanguageIndex].menuLanguage.languageMenuTitle;
        languageMenuButtonTitle.text = languageList[currentLanguageIndex].menuLanguage.languageMenuButton;
        languageMenuName.text = languageList[currentLanguageIndex].name;
        
        languageMenuImage.sprite = dataLoader.LoadSprite(languageList[currentLanguageIndex].imageFileName);
        
        //Check Pagination
        if (currentLanguageIndex <= 0)
        {
            paginationPreviousButton.interactable = false;
        }
        else
        {
            paginationPreviousButton.interactable = true;
        }

        if (currentLanguageIndex >= languageList.Count)
        {
            paginationNextButton.interactable = false;
        }
        else
        {
            paginationNextButton.interactable = true;
        }
    }

    public void LanguagePagination(int index)
    {
        currentLanguageIndex += index;
        if(currentLanguageIndex < 0) currentLanguageIndex = 0;
        if(currentLanguageIndex > languageList.Count - 1) currentLanguageIndex = languageList.Count - 1;

        if (currentLanguageIndex <= 0)
        {
            paginationPreviousButton.interactable = false;
        }
        else
        {
            paginationPreviousButton.interactable = true;
        }

        if (currentLanguageIndex >= languageList.Count)
        {
            paginationNextButton.interactable = false;
        }
        else
        {
            paginationNextButton.interactable = true;
        }
        
        languageMenuTitle.text = languageList[currentLanguageIndex].menuLanguage.languageMenuTitle;
        languageMenuButtonTitle.text = languageList[currentLanguageIndex].menuLanguage.languageMenuButton;
        languageMenuName.text = languageList[currentLanguageIndex].name;
        languageMenuImage.sprite = dataLoader.LoadSprite(languageList[currentLanguageIndex].imageFileName);
    }
    
    
    public void ApplyLanguageToGame()
    {
        
        dataLoader.LoadLanguageData(gameLanguageData.languageList[currentLanguageIndex].languageID);
        
        currentLanguage = gameLanguageData.languageList[currentLanguageIndex];
        
        languageMenuTitle.text = languageList[currentLanguageIndex].menuLanguage.languageMenuTitle;
        languageMenuButtonTitle.text = languageList[currentLanguageIndex].menuLanguage.languageMenuButton;
        languageMenuName.text = languageList[currentLanguageIndex].name;
        languageMenuImage.sprite = dataLoader.LoadSprite(languageList[currentLanguageIndex].imageFileName);
        
        gameTitle.text = languageList[currentLanguageIndex].menuLanguage.gameTitle;
        mainMenuButtonTitle.text = languageList[currentLanguageIndex].menuLanguage.mainMenuButton;
        tipButtonTitle.text = languageList[currentLanguageIndex].menuLanguage.tipButton;
        
        submitAnswersButton.text = languageList[currentLanguageIndex].menuLanguage.submitButton;
        scoreMenuTitle.text = languageList[currentLanguageIndex].menuLanguage.scoreTitle;
        scoreInputFieldPlaseHolderText.text = languageList[currentLanguageIndex].menuLanguage.playerInputFieldText;
        addPlayerButton.text = languageList[currentLanguageIndex].menuLanguage.addPlayerButton;
        
        dataLoader.LoadQuizData();
        
    }
    
    
}
