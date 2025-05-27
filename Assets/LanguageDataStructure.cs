using System;
using System.Collections.Generic;
using UnityEngine;

public class LanguageDataStructure : MonoBehaviour
{
    [Serializable]
    public class LanguageData
    {
        public List<Localization> languageList;
    }
    
    [Serializable]
    public class Localization
    {
        public string name;
        public string languageID;
        public string imageFileName;
        public MenuLanguage menuLanguage;
    }
    
    [Serializable]
    public class MenuLanguage
    {
        public string gameTitle;
        public string languageMenuTitle;
        public string languageMenuButton;
        public string mainMenuButton;
        public string tipButton;
    }
}
