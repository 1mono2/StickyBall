using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


namespace MyUtility
{

public static class VibrationManager
{
    const string SAVE_VARATION_FLAG = "VibrationFlag"; // 0:On, 1:OFF
    //protected override bool DontDestroy => true;

    public enum Length
    {
        Short,
        //Midium,
        //Long,
    }

    public static void Vibration(Length length)
    {
        if(PlayerPrefs.GetInt(SAVE_VARATION_FLAG) == 0)
        { 
            switch (length)
            {
                case Length.Short:
#if UNITY_ANDROID && !UNITY_EDITOR
                    AndroidUtili.Vibrate(35);
#elif !UNITY_EDITOR && UNITY_IOS
                    iOSUtili.PlaySystemSound(1519);
#endif

                    break;
                    //case Length.Midium:
                    //    AndroidUtili.Vibrate(2000);
                    //    iOSUtili.PlaySystemSound(1000);
                    //    break;
                    //case Length.Long:
                    //    break;
            }
        }

    }

    public static void DeactivateVibration()
    {
        PlayerPrefs.SetInt(SAVE_VARATION_FLAG, 1);
    }

    public static void ActivateVibration()
    {
        PlayerPrefs.SetInt(SAVE_VARATION_FLAG, 0);
    }
}

}