using UnityEngine;

public class DataLoader : MonoBehaviour
{

    [SerializeField] TextAsset jsonFile;

    [SerializeField] public JsonDataStructure.JsonData quizData;

    private void Awake()
    {
        quizData = JsonUtility.FromJson<JsonDataStructure.JsonData>(jsonFile.text);
    }
}
