using UnityEngine;
using UniRx;
using DG.Tweening;

public class CameraBlur : MonoBehaviour
{
    [SerializeField] PlayerCore playerCore;
    [SerializeField] RadialBlur radialBlur;

    // Start is called before the first frame update
    void Start()
    {
        playerCore.ballSize
            .Where(size => size != 1)
            .Subscribe(_ =>
            {
                Tween start = DOTween.To(() => 0.0f, __ => radialBlur.enabled = true, 1.0f, 0.0f);
                Tween tween = DOTween.To(() => 
                radialBlur.strength,_strength => radialBlur.strength = _strength , 1.0f, 0.3f);
                Tween tweenRollback = DOTween.To(() =>
                radialBlur.strength, _strength => radialBlur.strength = _strength, 0.0f, 0.3f);
                Tween finish = DOTween.To(() => 0.0f, __ => radialBlur.enabled = false, 1.0f, 0.0f);
                Sequence sequence = DOTween.Sequence();
                sequence.Append(start);
                sequence.Append(tween);
                sequence.Append(tweenRollback);
                sequence.Append(finish);
            });
    }
}
