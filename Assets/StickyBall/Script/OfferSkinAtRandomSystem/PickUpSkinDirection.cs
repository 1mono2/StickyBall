using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UniRx;
using System;

public class PickUpSkinDirection :  IDisposable
{

    Canvas pickUpSkinDirectionCanvas;

    Image randomFrame;
    RectTransform frameRect;
    int framePos = 0;

    int targetNum;
    List<int> targetNumList;
    List<RectTransform> rects;

    // it's not good at memory system. need to dispose it?
    public UnityAction completeHandler;

    public PickUpSkinDirection(Canvas pickUpSkinDirectionCanvas,Image randomFrame, int targetNum, List<int> targetNumList, List<RectTransform> rects)
    {
        this.pickUpSkinDirectionCanvas = pickUpSkinDirectionCanvas;
        this.randomFrame = randomFrame;
        this.targetNum = targetNum;
        this.targetNumList = targetNumList;
        this.rects = rects;

        this.randomFrame.gameObject.SetActive(false);
        this.frameRect = randomFrame.GetComponent<RectTransform>();
    }

    ~PickUpSkinDirection()
    {
        Dispose();
    }

    public void StartDirection() {
        if (targetNumList.Count > 1)
        {
            RoulettDirection();
        }
        else
        {
            ActivateSkin();
        }
    }


    private void RoulettDirection()
    {
        randomFrame.gameObject.SetActive(true);

        IObservable<int> countDown1 = CreateCountDownObservable(400, 5);
        IObservable<int> countDown2 = CreateCountDownObservable(200, 5);
        IObservable<int> countDown3 = CreateCountDownObservable(100, 5);
        var sequence = countDown1.Concat(countDown2).Concat(countDown3);

        sequence
            .Subscribe(_ =>
            {
                int randomNum = RandomRangeExclusive(0, this.targetNumList.Count, new List<int>() { framePos });
                frameRect.position = rects[this.targetNumList[randomNum]].position;
                framePos = randomNum;
            }, () =>
            {
                frameRect.position = rects[targetNum].position;
                framePos = targetNum;

                Observable.Timer(TimeSpan.FromMilliseconds(500))
                .FirstOrDefault()
                .Subscribe(_ =>
                {
                    ActivateSkin();
                });
            });
    }

    private void ActivateSkin()
    {
        randomFrame.gameObject.SetActive(false);
        SetActivePickUpSkinDirectionCanvas(true);
        completeHandler?.Invoke();
    }

    public void SetActivePickUpSkinDirectionCanvas(bool setBool) // use on button 
    {
        pickUpSkinDirectionCanvas.gameObject.SetActive(setBool);
    }


    private IObservable<int> CreateCountDownObservable(int countTimeMilliseconds, int takeTime)
    {
        return Observable
            .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(countTimeMilliseconds))
            .Select(x => (int)x)
            .Take(takeTime);
    }


    public static int RandomRangeExclusive(int minInclusive, int maxExclusice, List<int> exclusiveNumList)
    {
        List<int> excludedNum = new List<int>();

        try
        {
            for (int i = minInclusive; i < maxExclusice; i++)
            {
                if (exclusiveNumList.Contains(i)) { continue; }

                excludedNum.Add(i);
            }

            int randomNum = UnityEngine.Random.Range(0, excludedNum.Count);
            return excludedNum[randomNum];
        }
        catch
        {
            throw;
        }

    }

    public void Dispose()
    {
        completeHandler = null;
    }
}
