using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UniRx;
using System;
using System.Linq;
using System.Collections.Generic;

public class SkinView : MonoBehaviour
{
    [Header("Skin Select Canvas")]
    [SerializeField] Canvas _skinSelectCanvas;
    [SerializeField] Button[] _skins;
    List<RectTransform> _skinsRect = new List<RectTransform>();
    [SerializeField] RawImage[] _skinRawImages;
    [SerializeField] GameObject[] _balls;
    [SerializeField] Button _getSkinBtn;
    [SerializeField] Button _getBackBtn;
    [SerializeField] Button _forwardBtn;
    [SerializeField] Image _randomFrame;
    [SerializeField] Image _selectFrame;
    [SerializeField] Image _unenableTach;

    [Header("Direction Canvas")]
    [SerializeField] Canvas _directionCanvas;
    [SerializeField] Button _getBackDirectionBtn;
    [SerializeField] GameObject _backParticle;

    [HideInInspector] public List<UnityEvent<int>> OnSkinsBtnClickedListeners = new List<UnityEvent<int>>();

    [HideInInspector] public UnityAction OnGetSkinBtnClickedListener; // get skin
    [HideInInspector] public UnityAction OnGetBackBtnlickedListener; // back to main scene
    [HideInInspector] public UnityAction OnForwardBtnClickedListener; // move to skin scene
    [HideInInspector] public UnityAction OnGetBackDirectionBtnClickedListener; // back to skin scene from direction scene

    public void Initialized()
    {
        _randomFrame.gameObject.SetActive(false);
        for(int i = 0; i < _skins.Length; i++)
        {
            _skinsRect.Add(_skins[i].GetComponent<RectTransform>());
            // prepare empty action
            OnSkinsBtnClickedListeners.Add(new UnityEvent<int>());
        }

        
    }

    //================================================
    // SetActive
    //================================================

    public void SetActiveSkinSelectCanvas(bool setBool)
    {
        _skinSelectCanvas.gameObject.SetActive(setBool);
    }

    public void SetActiveDirectionCanvas(bool setBool)
    {
        _directionCanvas.gameObject.SetActive(setBool);
        _backParticle.SetActive(setBool);

    }

    public void SetButtonInteractable(bool setBool, int targetBtn)
    {
        _skins[targetBtn].interactable = setBool;
    }

    public void SetActiveBall(int targetBall)
    {
        foreach (GameObject ball in _balls)
        {
            ball.gameObject.SetActive(false);
        }
        _balls[targetBall].gameObject.SetActive(true);
    }

    //================================================
    // Method
    //================================================

    public void SetEvent()
    {
        if(OnSkinsBtnClickedListeners.Count == _skins.Length)
        {
            // cant expand this code.
            _skins[0].onClick.AddListener(() =>
            {
                OnSkinsBtnClickedListeners[0].Invoke(0);
            });
            _skins[1].onClick.AddListener(() =>
            {
                OnSkinsBtnClickedListeners[1].Invoke(1);
            });
            _skins[2].onClick.AddListener(() =>
            {
                OnSkinsBtnClickedListeners[2].Invoke(2);
            });
            _skins[3].onClick.AddListener(() =>
            {
                OnSkinsBtnClickedListeners[3].Invoke(3);
            });
            _skins[4].onClick.AddListener(() =>
            {
                OnSkinsBtnClickedListeners[4].Invoke(4);
            });
            _skins[5].onClick.AddListener(() =>
            {
                OnSkinsBtnClickedListeners[5].Invoke(5);
            });
            _skins[6].onClick.AddListener(() =>
            {
                OnSkinsBtnClickedListeners[6].Invoke(6);
            });
            _skins[7].onClick.AddListener(() =>
            {
                OnSkinsBtnClickedListeners[7].Invoke(7);
            });
            _skins[8].onClick.AddListener(() =>
            {
                OnSkinsBtnClickedListeners[8].Invoke(8);
            });
        }
        else
        {
            Debug.LogError("Don't match the Length " +
                "of OnSkinsBtnClickedListener with skins");
        }
        

        _getSkinBtn.onClick.AddListener(SafeCall(OnGetSkinBtnClickedListener));
        _getBackBtn.onClick.AddListener(SafeCall(OnGetBackBtnlickedListener));
        _forwardBtn.onClick.AddListener(SafeCall(OnForwardBtnClickedListener));
        _getBackDirectionBtn.onClick.AddListener(SafeCall(OnGetBackDirectionBtnClickedListener));

    }


   

    public void DisplayRoulettJudgment(int targetNum, IReadOnlyCollection<int> excludedNumList)
    {
        Debug.Log(excludedNumList.Count);
        if(excludedNumList.Count > 0)
        {
            RoulettDirection(targetNum, excludedNumList);
        }
        else if(excludedNumList.Count == 0)
        {
            DisplayDirection(targetNum);
        }
    }

    void RoulettDirection(int targetNum, IReadOnlyCollection<int>  excludedNumList)
    {
        _unenableTach.gameObject.SetActive(true);
        _randomFrame.gameObject.SetActive(true);
        RectTransform rect = _randomFrame.rectTransform;
        int? currentFramePos = null;

        List<int> roulettNumList = excludedNumList.Concat(new List<int> { targetNum }).ToList();
        IObservable<int> countDown1 = CreateCountDownObservable(400, 5);
        IObservable<int> countDown2 = CreateCountDownObservable(200, 5);
        IObservable<int> countDown3 = CreateCountDownObservable(100, 5);
        var sequence = countDown1.Concat(countDown2).Concat(countDown3);

        sequence
            .Subscribe(_ =>
            {
                int randomNum = RandomRangeExclusive(0, roulettNumList.Count, new List<int?>() { currentFramePos });
                rect.position = _skinsRect[roulettNumList[randomNum]].position;
                currentFramePos = randomNum;
            }, () =>
            {
                rect.position = _skinsRect[targetNum].position;
                currentFramePos = targetNum;

                Observable.Timer(TimeSpan.FromMilliseconds(500))
                .FirstOrDefault()
                .Subscribe(_ =>
                {
                    DisplayDirection(targetNum);
                });
            });
    }

    void DisplayDirection(int targetNum) // need information for identfy a skin.a
    {
        _unenableTach.gameObject.SetActive(false);
        _randomFrame.gameObject.SetActive(false);
        SetActiveDirectionCanvas(true);
        SetButtonInteractable(true, targetNum);
        _skinRawImages[targetNum].gameObject.SetActive(true);
    }

    public void HideDirection()
    {
        foreach (RawImage image in _skinRawImages)
            image.gameObject.SetActive(false);

        SetActiveDirectionCanvas(false);
    }

    public void ChangeSelectFramePos(int num)
    {
        _selectFrame.rectTransform.position = _skinsRect[num].position;
    }

    //================================================
    // Generic Method
    //================================================

    public static UnityAction SafeCall(UnityAction action)
    {
        if (action != null)
        {
            return action;
        }
        else
        {
            return delegate { };
        }

        // The other way,
        // return action ?? delegate {};
    }

    IObservable<int> CreateCountDownObservable(int countTimeMilliseconds, int takeTime)
    {
        return Observable
            .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(countTimeMilliseconds))
            .Select(x => (int)x)
            .Take(takeTime);
    }

    public static int RandomRangeExclusive(int minInclusive, int maxExclusice, List<int?> exclusiveNumList)
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
}
