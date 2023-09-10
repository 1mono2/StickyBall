using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckButtonInteractable : MonoBehaviour
{
    [SerializeField] Button button;
    public RectTransform rect;

    private void Start()
    {
        this.rect = button.gameObject.GetComponent<RectTransform>();
    }

    public bool GetInteractable()
    {
        return button.interactable;
    }

    public void SetInteractable(bool setBool)
    {
        button.interactable = setBool;
    }

}
