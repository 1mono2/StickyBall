using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class TimeKeeper : MonoBehaviour
{

    // Is there count donw? Does it set interval?


    [SerializeField, Range(0,5)] private int _countDownTime = 5;
    public int countDownTime => _countDownTime;
    [SerializeField, Range(20,120)] private int _gameTime = 120; // 2 min
    public int gameTime => _gameTime;
    public int halfTime => _gameTime / 2;

    public IObservable<int> GameStartCountDownObservable { get; private set; }
    public IObservable<int> BattleCountDownObservableFirst { get; private set; }
    public IObservable<int> BattleCountDownObservableSecond { get; private set; }

    private IConnectableObservable<int> startConnectableObservable;
    private IConnectableObservable<int> battleConnectableObservableFirst;
    private IConnectableObservable<int> battleConnectableObservableSecond;

    public void Start()
    {

        startConnectableObservable = CreateCountDownObservable(_countDownTime).Publish();
        GameStartCountDownObservable = startConnectableObservable;

        battleConnectableObservableFirst = CreateCountDownObservable(halfTime).Publish();
        BattleCountDownObservableFirst = battleConnectableObservableFirst;

        battleConnectableObservableSecond = CreateCountDownObservable(halfTime).Publish();
        BattleCountDownObservableSecond = battleConnectableObservableSecond;

        GameStartCountDownObservable.Subscribe(_ => {; }, () => battleConnectableObservableFirst.Connect());

        BattleCountDownObservableFirst
            .Subscribe(_ => {; }, () => 
            {
                // What you want to finished
            });
        // you're available to insert a method in second argument on Subscribe method.

       

    }

    private IObservable<int> CreateCountDownObservable(int countTime)
    {
        return Observable 
            .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))　// Timer(arg1, arg2) Timer method makes it delay to execute,
                                                                     // and it flows values at regular interval of arg2
                                                                     // TimsSpan.From***(arg) method makes value of arg converted to second.
                                                                     // It is convenient for you not to know  conputer time standard
            .Select(x => (int)(countTime - x))
            .TakeWhile(x => x >= 0);
    }

    public void CountDownStart()
    {
        startConnectableObservable.Connect();
    }

    public void HarfSecondStart()
    {
        battleConnectableObservableSecond.Connect();
    }
}
