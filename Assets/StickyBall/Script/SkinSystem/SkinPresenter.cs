using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SkinPresenter : MonoBehaviour
{
    [SerializeField] SkinView _skinView;
    SkinModel _skinModel;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _skinView.Initialized();
        this.SetEvent();
        _skinView.SetEvent();

        _skinModel = new SkinModel();
        _skinModel.Load();

        SetInteractable();
        Bind();
    }

    void SetInteractable()
    {
        foreach(int num in _skinModel.activatedNumList)
        {
            _skinView.SetButtonInteractable(true, num);
        }
        foreach(int num in _skinModel.excludedNumList)
        {
            _skinView.SetButtonInteractable(false, num);
        }
    }

    void Bind()
    {
        _skinModel.selectedNum
            .Subscribe(num =>
            {
                _skinView.ChangeSelectFramePos(num);
                _skinView.SetActiveBall(num);
            });

        // get a skin at Random
        _skinModel.activatedNumList.ObserveAdd()
            .Subscribe(num =>
            {
                _skinView.DisplayRoulettJudgment(num.Value, _skinModel.excludedNumList);
            });
    }

    void SetEvent()
    {
        for (int i = 0; i < _skinView.OnSkinsBtnClickedListeners.Count; i++)
        {
            _skinView.OnSkinsBtnClickedListeners[i].AddListener(num => { OnSkinsBtnClicked(num); });
        }

        _skinView.OnGetSkinBtnClickedListener += OnGetSkinBtnClicked;
        _skinView.OnGetBackBtnlickedListener += OnGetBuckBtnlicked;
        _skinView.OnForwardBtnClickedListener += OnforwardBtnClicked;
        _skinView.OnGetBackDirectionBtnClickedListener += OnGetBackDirectionBtnClicked;

    }

    private void OnSkinsBtnClicked(int num)
    {
        _skinModel.ChangeSelectedNum(num);
    }

    // get a skin at Random
    private void OnGetSkinBtnClicked()
    {
        _skinModel.RandomActivate();
    }

    private void OnGetBuckBtnlicked()
    {
        _skinView.SetActiveSkinSelectCanvas(false);
    }

    private void OnforwardBtnClicked()
    {
        _skinView.SetActiveSkinSelectCanvas(true);
        _skinView.ChangeSelectFramePos(_skinModel.selectedNum.Value);
    }

    private void OnGetBackDirectionBtnClicked()
    {
        _skinView.HideDirection();
    }
}
