using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(TextMeshProUGUI))]
public class BallSizeUpDisplayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] PlayerCore playerCore;
    const float DURATION = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        playerCore.ballSize
            .Where(size => size != 1)
            .Subscribe(_ =>
        {
            Tween tween1 = DOTween.To(() => 0.0f, _alpha =>
            {
                text.alpha = _alpha;
            }, 1.0f, DURATION);

            Tween tween2 = DOTween.To(() => 1.0f, _alpha =>
            {
                text.alpha = _alpha;
            }, 0.0f, DURATION);

            var move = text.rectTransform.DOMoveY(40.0f, DURATION).SetRelative();
            var remove = text.rectTransform.DOMoveY(-40.0f, 0).SetRelative();

            Sequence sequence = DOTween.Sequence();
            sequence.Append(tween1);
            sequence.Join(move);
            sequence.Append(tween2);
            sequence.Append(remove);
            sequence.Play();
        }).AddTo(this);
    }
   
}
