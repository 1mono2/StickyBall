using System;
using UnityEngine;
using UniRx;

public class LoadAndHoldSkinValue : MonoBehaviour
{
    ReactiveProperty<int> _currentSkinValue = new ReactiveProperty<int>(0); // SkinValue starts from 0
    public IReadOnlyReactiveProperty<int> currentSkinValue => _currentSkinValue;
    const string SKIN_VALUE = "skinValue";

    void Start()
    {
        Load();
    }

    void Load()
    {
        if (PlayerPrefs.HasKey(SKIN_VALUE))
        {
            _currentSkinValue.Value = PlayerPrefs.GetInt(SKIN_VALUE);
        }
        else
        {
            ChangeSkinValue(0);
        }
        
    }

    public void ChangeSkinValue(int value)
    {
        _currentSkinValue.Value = value;
        PlayerPrefs.SetInt(SKIN_VALUE, value);
    }
}
