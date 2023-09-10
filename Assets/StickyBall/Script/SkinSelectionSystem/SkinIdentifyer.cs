using System;
using UnityEngine;

public class SkinIdentifyer : MonoBehaviour
{
    [SerializeField] int skinNum;
    [SerializeField] LoadAndHoldSkinValue skinValue;

    public void ActivateSkin()
    {
        skinValue.ChangeSkinValue(skinNum);
    }

}
