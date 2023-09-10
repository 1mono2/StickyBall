using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class PlaySound : MonoBehaviour
{
    public UnityEvent unityEvent;


    public void Invoke()
    {
        if (unityEvent != null)
        {
            unityEvent.Invoke();
        }
       
    }
}
