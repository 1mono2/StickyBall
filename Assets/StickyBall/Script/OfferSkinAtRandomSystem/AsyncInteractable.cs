using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

[RequireComponent(typeof(Button))]
public class AsyncInteractable : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Button button;
    void Start()
    {
        button.ObserveEveryValueChanged(btn => btn.interactable)
            .Subscribe(isOn => {
                target.SetActive(!isOn);
            });
    }

   
}
