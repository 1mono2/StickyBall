using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Diagnostics;

public class SkinChanger : MonoBehaviour
{

    [SerializeField] LoadAndHoldSkinValue skinValue;
    [SerializeField] GameObject[] balls;
    [SerializeField] CheckButtonInteractable[] checkButtonInteractables;
    [SerializeField] Image selectFrame;

    void Start()
    {
        RectTransform rectSelectFrame = selectFrame.GetComponent<RectTransform>();
        skinValue.currentSkinValue
            .DelayFrame(1) // masic
            .Subscribe(value =>
            {
                AllUnActivate();
                balls[value]?.SetActive(true);
                rectSelectFrame.position = checkButtonInteractables[value].rect.position;
            }).AddTo(this);

    }

    void AllUnActivate()
    {
        foreach(GameObject ball in balls)
        {
            ball.SetActive(false);
        }
    }

}
