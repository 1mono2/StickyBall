using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using TMPro;
using MyUtility;

public class GameManager : MonoBehaviour
{
    [Header("TimeKeeper")]
    [SerializeField] TimeKeeper timeKeeper;

    [Header("Camera")]
    [SerializeField] ChaseCamera chaseCamera;
    [SerializeField] Camera resultCamera;

    [Header("Player")]
    [SerializeField] BallMove ballMove;
    [SerializeField] PlayerCore playerCore;

    [Header("UI Structure")]
    [SerializeField] Canvas startCanvas;
    [SerializeField] Button playButton;

    [Header("GameStartCountDownCanvas")]
    [SerializeField] Canvas gameStartCountDownCanvas;
    [SerializeField] TextMeshProUGUI gameStartCountDownText;

    [Header("InGameCanvas")]
    [SerializeField] Canvas inGameCanvas;
    [SerializeField] TextMeshProUGUI timeLeftText;

    [Header("IntervalCanvas")]
    [SerializeField] Canvas intervalCanvas;
    [SerializeField] TextMeshProUGUI timeLargeText;
    [SerializeField] Button continueButton;

    [Header("ResultCanvas")]
    [SerializeField] Canvas resultCanvasOverlay;
    [SerializeField] Canvas resultCanvasCamera;
    [SerializeField] TextMeshProUGUI finishedText;
    [SerializeField] Button continueResultButton;

    [Header("Scene")]
    [SceneName] [SerializeField] string mainStage;


    // Start is called before the first frame update
    void Start()
    {
        

        playButton.OnClickAsObservable()
            .Delay(TimeSpan.FromMilliseconds(0.1))
            .FirstOrDefault()
            .Subscribe(_ => 
            {
                timeKeeper.CountDownStart();
                startCanvas.gameObject.SetActive(false);
                gameStartCountDownCanvas.gameObject.SetActive(true);
            }).AddTo(this);

        // Count Down
        timeKeeper
            .GameStartCountDownObservable
            .Subscribe(time =>
            {
                // Before game starts, start to count start
                gameStartCountDownText.text = string.Format("{0}", time);
            }, () => 
            {
                // When finish count down , game starts.
                gameStartCountDownText.text = string.Empty;
                ballMove.enableToMove = true;
                gameStartCountDownCanvas.gameObject.SetActive(false);
                inGameCanvas.gameObject.SetActive(true);
            }).AddTo(this);

        // Half First in game
        timeKeeper
          .BattleCountDownObservableFirst
          .Select(time => time + timeKeeper.halfTime)
          .Subscribe(time =>
          {
                timeLeftText.text = string.Format("Time:{0}", time);
          }, () => 
          {
              // stop playing game
              ballMove.enableToMove = false;
              // add blur
              chaseCamera.GetComponent<FastBlur>().enabled = true;
              continueButton.gameObject.SetActive(true);
          }).AddTo(this);

        // Interval
        timeKeeper
          .BattleCountDownObservableFirst
          .Where(time => time == 5) // before 5 sec
          .Subscribe(time =>
          {
              intervalCanvas.gameObject.SetActive(true);
              continueButton.gameObject.SetActive(false);
          }).AddTo(this);

        timeKeeper
            .BattleCountDownObservableFirst
            .Where(time => 0 <= time && time <= 5) //under 65, upper 60
            .Subscribe(time => 
            {
                timeLargeText.text = string.Format("{0}",  time);
            }).AddTo(this);


        continueButton.OnClickAsObservable()
            .FirstOrDefault()
            .Subscribe(_ => 
            {
                InterstitalAdsManager.Instance.ShowInterstitialAds();
                chaseCamera.GetComponent<FastBlur>().enabled = false;
                timeLargeText.text = string.Empty;
                intervalCanvas.gameObject.SetActive(false);
                // resume playing game
                ballMove.enableToMove = true;
                timeKeeper.HarfSecondStart();
            }).AddTo(this);

      
        // Half Second
        timeKeeper
           .BattleCountDownObservableSecond
           .Subscribe(time =>
           {
                // In game
                timeLeftText.text = string.Format("Time:{0}", time);
           }, () =>
           {
                // Game Finished.
                ballMove.enableToMove = false;
               ballMove.Stop();
               finishedText.transform.gameObject.SetActive(true);
                // add blur
                chaseCamera.GetComponent<FastBlur>().enabled = true;

               Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(_ =>
               {

                   chaseCamera.cam.cullingMask = 1 << 0;

                   inGameCanvas.gameObject.SetActive(false);
                   resultCamera.gameObject.SetActive(true);
                   resultCamera.gameObject.transform.position = new Vector3(chaseCamera.gameObject.transform.position.x,
                       playerCore.gameObject.transform.position.y, chaseCamera.gameObject.transform.position.z);
                   resultCanvasOverlay.gameObject.SetActive(true);
                   resultCanvasCamera.gameObject.SetActive(true);
                    // ball rotates on the spot.
                    ballMove.enableToRotate = true;
               }).AddTo(this);
           }).AddTo(this);

        continueResultButton.OnClickAsObservable()
            .FirstOrDefault()
            .Subscribe(_ =>
            {
                InterstitalAdsManager.Instance.ShowInterstitialAds();
                SceneManager.LoadScene(mainStage);
            }).AddTo(this);
    }


}
