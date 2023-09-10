using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class DisposableAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    void Start()
    {
        ObservableStateMachineTrigger trigger = animator.GetBehaviour<ObservableStateMachineTrigger>();

        IDisposable exitState = trigger
            .OnStateExitAsObservable()
            .Subscribe(onStateInfo =>
            {
                Destroy(this.gameObject);
            }).AddTo(this);
    }

   
}
