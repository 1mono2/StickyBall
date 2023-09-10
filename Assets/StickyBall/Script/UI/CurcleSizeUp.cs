using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

[RequireComponent(typeof(Image))]
public class CurcleSizeUp : MonoBehaviour
{
    [SerializeField] Image curcle;
    [SerializeField] PlayerCore playerCore;

    void Start()
    {
        playerCore.ballSize
          .Where(size => size != 1)
          .Subscribe(size =>
          {
              curcle.rectTransform.localScale = new Vector3(1.7f + size * 0.1f, 1.7f + size * 0.1f, 1);
           }).AddTo(this);
    }
}
