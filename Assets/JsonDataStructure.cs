using System;
using System.Collections.Generic;
using UnityEngine;

public class JsonDataStructure : MonoBehaviour
{
    [Serializable]
    public class Answer
    {
        public string answer;
        public bool isAnswerCorrect;
    }

    [Serializable]
    public class Question
    {
        public string question;
        public string tip;
        public string explanation;
        public int difficulty;
        public List<Answer> answers;
    }

    [Serializable]
    public class Category
    {
        public int categoryID;
        public string name;
        public string backgroundImage;
        public List<Question> questions;
    }

    [Serializable]
    public class JsonData
    {
        List<Category> categories;
    }


}
