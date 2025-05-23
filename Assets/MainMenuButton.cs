using System;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuButton : MonoBehaviour
{
    [Serializable]
    public class ReturnCategoryIndex : UnityEvent<int> { }

    public ReturnCategoryIndex onReturnIndex;

    public int categoryIndex;


    public void ReturnCategoryIndexNumber()
    {
        onReturnIndex.Invoke(categoryIndex);
    }



}
