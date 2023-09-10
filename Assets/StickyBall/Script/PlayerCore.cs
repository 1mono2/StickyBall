using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;
using UniRx.InternalUtil;
using MyUtility;

public class PlayerCore : MonoBehaviour
{
    [SerializeField] GameObject childProps;

    private float scaleMultiply = 2f; // too big to multipy 2
    const byte ballSizeMin = 1;
    const byte ballSizeMax = 7;
    private ReactiveProperty<byte> _ballSize = new ReactiveProperty<byte>(ballSizeMin);
    public IReadOnlyReactiveProperty<byte> ballSize => _ballSize;

    public float sizePoint { get; private set; } = 0;
    const float sizePointMin = 0;
    const float sizePointMax = 8; // too little to maximum 5
    private ReactiveProperty<SizeCriterion> _objectSize = new ReactiveProperty<SizeCriterion>();
    public IReadOnlyReactiveProperty<SizeCriterion> objectSize => _objectSize; //On property, "=> **" equal to "{get;} = **" 
    const int ballQuantitySizeMin = 0;
    private ReactiveProperty<int> _ballQuantitySize = new ReactiveProperty<int>(ballQuantitySizeMin);
    public IReadOnlyReactiveProperty<int> ballQuantitySize => _ballQuantitySize;


    void BallSizeUp()
    {
        SoundEffectManager.Instance.PlayBallSizeUpSE();
        sizePoint = sizePointMin;
        _ballSize.Value += 1;
        foreach (Transform child in childProps.transform)
        {
            var childSizeCriterion = child.GetComponent<SizeCriterion>();
            if(childSizeCriterion.size <= _ballSize.Value - 3)
            {
                Destroy(child.gameObject);
            }
            else
            {
                child.gameObject.transform.localScale /= scaleMultiply;
                child.gameObject.transform.localPosition /= (scaleMultiply - 0.5f);
            }
        }
        this.transform.localScale *= scaleMultiply;


    }
    
    private void OnCollisionEnter(Collision collision)
    {
      
        if (collision.gameObject.CompareTag("Sticky"))
        {
            SizeCriterion size = collision.transform.GetComponent<SizeCriterion>();

            if (size.size <= _ballSize.Value)
            {
                // items stick ball
                collision.transform.parent = childProps.transform;
                collision.transform.GetComponent<Collider>().enabled = false;
                // changing layer to "ResultObject" 
                SetLayerRecursively(collision.gameObject, 3); // layer3 = ResultObject
                // playing sound
                SoundEffectManager.Instance.PlayRandomStickSE();
                // if collision has another sound effect, playing it.
                PlaySound disposablePlaySound;
                if (collision.transform.TryGetComponent<PlaySound>(out disposablePlaySound))
                {
                    disposablePlaySound.Invoke();
                }

                // convey size of objects 
                _objectSize.Value = size; 
                sizePoint += size.size / _ballSize.Value; // size of Objects devides current ball size is point
                if(sizePoint >= sizePointMax && _ballSize.Value <= ballSizeMax)
                {
                    BallSizeUp();
                }

                // Size of Quantity
                _ballQuantitySize.Value += 10 * size.size;

                VibrationManager.Vibration(VibrationManager.Length.Short);
            }
            
        }
    }

    void SetLayerRecursively(GameObject obj, int newLayer )
    {
        obj.layer = newLayer;

        foreach (Transform child  in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

}
