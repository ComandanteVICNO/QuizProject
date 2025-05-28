using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

public class DataLoader : MonoBehaviour
{
    public event Action onDataReloaded;
    
    
    [FormerlySerializedAs("jsonFile")]
    [Header("Quiz Data")]
    [SerializeField] TextAsset defaultQuizDataFile;
    [SerializeField] private string fileName = "quizData";
    private string exeFolderPath;
    private string dataFolderName = "QuizData/";
    private string dataFolderPath;
    private string defaultQuizDataFileName;
    private string quizDataFilePath;
    [SerializeField] public JsonDataStructure.JsonData quizData;
    [SerializeField] private List<TextAsset> defaultQuizDataFilesList;
    
    
    [Header("Language Data")]
    [SerializeField] public TextAsset languageFile;
    private string languageDataFilePath;
    
    [SerializeField] private string languageFileName = "languageData.json";
    [SerializeField] private List<Sprite> languageImages;
    
    [SerializeField] public LanguageDataStructure.LanguageData languageData;
    [SerializeField] GameLanguageManager gameLanguageManager;
    bool isRunningInInspector = false;
    private void Awake()
    {
#if UNITY_EDITOR
        isRunningInInspector = true;
#endif
        gameLanguageManager = FindAnyObjectByType<GameLanguageManager>();
        if (isRunningInInspector)
        {
            quizData = JsonUtility.FromJson<JsonDataStructure.JsonData>(defaultQuizDataFile.text);
            languageData = JsonUtility.FromJson<LanguageDataStructure.LanguageData>(languageFile.text);
            gameLanguageManager.gameLanguageData = languageData;
            gameLanguageManager.SetDefaultLanguage();
            Debug.Log("THIS RAN!");
        }
        else
        {
            Debug.Log("But this didnt!");
            //Get exe folder
            exeFolderPath = Path.GetDirectoryName(Application.dataPath);
        
            //Get data folder path, string is: data/
            dataFolderPath = Path.Combine(exeFolderPath, dataFolderName);
        
            //Get quiz data file path, string is: data/jsonfile
            
            
            CheckIfDataFolderExists();
            
            languageDataFilePath = Path.Combine(dataFolderPath, languageFileName);
            CheckIfLanguageFileExists();
            
            
            
            gameLanguageManager.gameLanguageData = languageData;
           
            gameLanguageManager.SetDefaultLanguage();
           
            
            
            defaultQuizDataFileName = fileName + "_en.json";
            
            string newFileName = fileName + gameLanguageManager.currentLanguage.languageID + ".json";
            quizDataFilePath = Path.Combine(dataFolderPath, newFileName);
            
            CheckIfQuizDataFileExists();
            CheckAndSaveLocalizationImages();
        }
    }




    void CheckIfDataFolderExists()
    {
        if (!Directory.Exists(dataFolderPath))
        {
            Directory.CreateDirectory(dataFolderPath);
        }
    }
    
    

    
    void CheckIfLanguageFileExists()
    {
        //Check if language file exists, if not create file
        if (File.Exists(languageDataFilePath))
        {
            string json = File.ReadAllText(languageDataFilePath);
            languageData = JsonUtility.FromJson<LanguageDataStructure.LanguageData>(json); 
        }
        else
        {
            File.WriteAllText(languageDataFilePath, languageFile.text);
            languageData = JsonUtility.FromJson<LanguageDataStructure.LanguageData>(languageFile.text);
        }

        

    }
    
    void CheckIfQuizDataFileExists()
    {
        //Check if quiz data file exits, if not create file
        if (File.Exists(quizDataFilePath))
        {
            string json = File.ReadAllText(quizDataFilePath);
            quizData = JsonUtility.FromJson<JsonDataStructure.JsonData>(json);
        }
        else
        {
            string defaultQuizDataFilePath = Path.Combine(dataFolderPath, defaultQuizDataFileName);
            
            File.WriteAllText(defaultQuizDataFilePath, defaultQuizDataFile.text);
            quizData = JsonUtility.FromJson<JsonDataStructure.JsonData>(defaultQuizDataFile.text);
        }
        
        foreach (var textFile in defaultQuizDataFilesList)
        {
            string name = textFile.name + ".json";
            string path = Path.Combine(dataFolderPath, name);

            if (!File.Exists(path))
            {
                File.WriteAllText(path, textFile.text);
            }
        }
    }
    public void LoadQuizData()
    {
        string newFileName = fileName + gameLanguageManager.currentLanguage.languageID + ".json";
        quizDataFilePath = Path.Combine(dataFolderPath, newFileName);

        if (File.Exists(quizDataFilePath))
        {
            string json = File.ReadAllText(quizDataFilePath);
            quizData = JsonUtility.FromJson<JsonDataStructure.JsonData>(json);
        }
        else
        {
            string defaultQuizDataFilePath = Path.Combine(dataFolderPath, defaultQuizDataFileName);
            
            File.WriteAllText(defaultQuizDataFilePath, defaultQuizDataFile.text);
            quizData = JsonUtility.FromJson<JsonDataStructure.JsonData>(defaultQuizDataFile.text);
        }
        
        onDataReloaded?.Invoke();
        
    }
    
    public void LoadLanguageData(string languageID)
    {
        string newFileName = fileName + languageID + ".json";
        string newFilePath = Path.Combine(dataFolderPath, newFileName);

        if (File.Exists(newFilePath))
        {
            string json = File.ReadAllText(newFilePath);
            quizData = JsonUtility.FromJson<JsonDataStructure.JsonData>(json);
        }
    }
    
    public Sprite LoadSprite(string spriteName)
    {
        string imagePath = Path.Combine(dataFolderPath, spriteName);

        if (File.Exists(imagePath))
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
            
            return sprite;
        }
        return null;
    }

    public void CheckAndSaveLocalizationImages()
    {
        foreach (Sprite sprite in languageImages)
        {
            string savePath = Path.Combine(dataFolderPath, sprite.name + ".png");

            if (!File.Exists(savePath))
            {
                Texture2D originalTexture = sprite.texture;
                Texture2D textureToSave;

                if (sprite.rect.width != originalTexture.width || sprite.rect.height != originalTexture.height)
                {
                    textureToSave = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGBA32, false);
                    Color[] pixels = originalTexture.GetPixels(
                        (int)sprite.rect.x, (int)sprite.rect.y,
                        (int)sprite.rect.width, (int)sprite.rect.height);
                    textureToSave.SetPixels(pixels);
                    textureToSave.Apply();
                }
                else
                {
                    textureToSave = new Texture2D(originalTexture.width, originalTexture.height, TextureFormat.RGBA32, false);
                    textureToSave.SetPixels(originalTexture.GetPixels());
                    textureToSave.Apply();
                }

                byte[] imageBytes = textureToSave.EncodeToPNG();
                File.WriteAllBytes(savePath, imageBytes);

                if (textureToSave != originalTexture)
                {
                    DestroyImmediate(textureToSave);
                }
            }
        }
    }
    
}
