using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyUtility;

public class SoundEffectManager : SingletonMonoBehaviour<SoundEffectManager>
{
    protected override bool DontDestroy =>  false;  //On property, "=> **" equal to "{get;} = **" 

    [Header("MotionSE")]
    [SerializeField] AudioSource stickSE1;
    [SerializeField] AudioSource stickSE2;
    [SerializeField] AudioSource stickSE3;
    [SerializeField] AudioSource stickSE4;

    [Header("BallSizeUpSE")]
    [SerializeField] AudioSource ballSizeUpSE1;

    [Header("ScreamSE")]
    [SerializeField] AudioSource femaleScreamSE1;
    [SerializeField] AudioSource maleScreamSE2;

    public void PlayRandomStickSE()
    {
        int random =  Random.Range(1, 100);
        if(random >= 1 && random <26)
        {
            stickSE1.Play();
        }else if(random >= 26 && random < 51)
        {
            stickSE2.Play();
        }
        else if (random >= 51 && random < 76)
        {
            stickSE3.Play();
        }
        else if (random >= 76 && random < 100)
        {
            stickSE4.Play();
        }
    }

    public void PlayBallSizeUpSE()
    {
        ballSizeUpSE1.Play();
    }

    public void PlayRandomScreamSE()
    {
        int random = Random.Range(1, 100);
        if (random >= 1 && random < 26)
        {
            femaleScreamSE1.Play();
        }
        else if (random >= 26 && random < 51)
        {
            maleScreamSE2.Play();
        }else if(random >= 51){
            // No play
        }
    }

    public void PlayFemaleScreamSE()
    {
        femaleScreamSE1.Play();
    }


    public void PlayMaleScreamSE()
    {
        maleScreamSE2.Play();
    }

}
