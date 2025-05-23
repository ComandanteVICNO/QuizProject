using System.IO;
using UnityEngine;

public class DataLoader : MonoBehaviour
{

    [SerializeField] TextAsset jsonFile;
    [SerializeField] private string fileName = "quizData.json";
    private string exeFolderPath;
    private string dataFolderName = "QuizData/";
    private string dataFolderPath;
    private string dataFilePath;

    [SerializeField] public JsonDataStructure.JsonData quizData;

    
    bool isRunningInInspector = false;
    private void Awake()
    {
#if UNITY_EDITOR
        isRunningInInspector = true;
#endif

        if (isRunningInInspector)
        {
            quizData = JsonUtility.FromJson<JsonDataStructure.JsonData>(jsonFile.text);
        
            Debug.Log("THIS RAN!");
        }
        else
        {
            Debug.Log("But this didnt!");
            exeFolderPath = Path.GetDirectoryName(Application.dataPath);
        
            dataFolderPath = Path.Combine(exeFolderPath, dataFolderName);
        
            dataFilePath = Path.Combine(dataFolderPath, fileName);
        
            if (!Directory.Exists(dataFolderPath))
            {
                Directory.CreateDirectory(dataFolderPath);
            }

            if (File.Exists(dataFilePath))
            {
                string json = File.ReadAllText(dataFilePath);
                quizData = JsonUtility.FromJson<JsonDataStructure.JsonData>(json);
            }
            else
            {
                File.WriteAllText(dataFilePath, jsonFile.text);
                quizData = JsonUtility.FromJson<JsonDataStructure.JsonData>(jsonFile.text);
            }
        }





    }
}
