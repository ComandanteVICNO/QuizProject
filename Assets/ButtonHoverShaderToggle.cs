using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverShaderToggle : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
   
    [SerializeField] private Material hoverMaterial;

    [SerializeField]private Button button;
    [SerializeField]private Image buttonImage;

    [SerializeField] TMP_Text buttonTextField;
    [SerializeField] private Vector4 buttonTextMargin;
    
    
    private void Start()
    {
        buttonTextField = button.GetComponentInChildren<TMP_Text>();
        buttonTextField.margin = buttonTextMargin;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        Debug.Log("OnPointerEnter");
        
        if (button.IsInteractable())
        {
            buttonImage.material = hoverMaterial;
            button.transform.DOScale(1.1f,0.2f).SetEase(Ease.OutBounce);
            
        }
        else
        {
            buttonImage.material = null;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit");
        buttonImage.material = null;
        button.transform.DOScale(1f,0.2f).SetEase(Ease.InBounce);
    }

    private void OnDisable()
    {
        button.transform.localScale = Vector3.one;
        buttonImage.material = null;
    }
}
