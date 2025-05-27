using System.IO;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    [Header("Quiz Data")]
    [SerializeField] TextAsset jsonFile;
    [SerializeField] private string fileName = "quizData.json";
    private string exeFolderPath;
    private string dataFolderName = "QuizData/";
    private string dataFolderPath;
    private string quizDataFilePath;
    [SerializeField] public JsonDataStructure.JsonData quizData;
    
    [Header("Language Data")]
    [SerializeField] public TextAsset languageFile;
    private string languageDataFilePath;
    
    [SerializeField] private string languageFileName = "languageData.json";

    [SerializeField] public LanguageDataStructure.LanguageData languageData;
    
    bool isRunningInInspector = false;
    private void Awake()
    {
#if UNITY_EDITOR
        isRunningInInspector = true;
#endif

        if (isRunningInInspector)
        {
            quizData = JsonUtility.FromJson<JsonDataStructure.JsonData>(jsonFile.text);
            languageData = JsonUtility.FromJson<LanguageDataStructure.LanguageData>(languageFile.text);
            Debug.Log("THIS RAN!");
        }
        else
        {
            Debug.Log("But this didnt!");
            exeFolderPath = Path.GetDirectoryName(Application.dataPath);
        
            dataFolderPath = Path.Combine(exeFolderPath, dataFolderName);
        
            quizDataFilePath = Path.Combine(dataFolderPath, fileName);
        
            
            
            languageDataFilePath = Path.Combine(dataFolderPath, languageFileName);
            
            if (!Directory.Exists(dataFolderPath))
            {
                Directory.CreateDirectory(dataFolderPath);
            }

            
            //Check if language file exists, if not create file
            if (File.Exists(languageDataFilePath))
            {
                string json = File.ReadAllText(quizDataFilePath);
                languageData = JsonUtility.FromJson<LanguageDataStructure.LanguageData>(json); 
            }
            else
            {
                File.WriteAllText(languageDataFilePath, languageFile.text);
                languageData = JsonUtility.FromJson<LanguageDataStructure.LanguageData>(languageFile.text);
            }
            
            
            
            //Check if quiz data file exits, if not create file
            if (File.Exists(quizDataFilePath))
            {
                string json = File.ReadAllText(quizDataFilePath);
                quizData = JsonUtility.FromJson<JsonDataStructure.JsonData>(json);
            }
            else
            {
                File.WriteAllText(quizDataFilePath, jsonFile.text);
                quizData = JsonUtility.FromJson<JsonDataStructure.JsonData>(jsonFile.text);
            }
        }





    }
}
