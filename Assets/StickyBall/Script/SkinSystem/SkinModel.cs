using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using MyUtility;


public class SkinModel : SkinModelInterface
{
    int allCount = 9;

    static List<int> maxList = Enumerable.Range(0, 9).ToList<int>();

    public List<int> excludedNumList
    { get { return ExcludedNumList(_activatedNumList.ToList<int>(), maxList); } }

    IntReactiveProperty _selectedNum = new IntReactiveProperty();
    public IReadOnlyReactiveProperty<int> selectedNum => _selectedNum;

    // This List may increase, but it never decrease.
    ReactiveCollection<int> _activatedNumList = new ReactiveCollection<int>();
    public IReadOnlyReactiveCollection<int> activatedNumList => _activatedNumList;



    const string SELECTED_SKIN = "SELECTED_SKIN";
    const string ACTIVATED_SKIN = "ACTIVATED_SKIN";

    public void Load()
    {
        if (!PlayerPrefs.HasKey(SELECTED_SKIN))
        {
            PlayerPrefs.SetInt(SELECTED_SKIN, 0);
        }
        if (!PlayerPrefs.HasKey(ACTIVATED_SKIN))
        {
            PlayerPrefsExtension.SetList<int>(ACTIVATED_SKIN, new List<int>() {0});
        }

        _selectedNum.Value = PlayerPrefs.GetInt(SELECTED_SKIN);

        var tempList = PlayerPrefsExtension.GetList<int>(ACTIVATED_SKIN);
        for (int i = 0; i < tempList.Count; i++)
        {
            AddList(tempList[i]);
        }
        //foreach (int num in _activatedNumList) { Debug.Log(num); }

    }

    public void Save()
    {
        PlayerPrefs.SetInt(SELECTED_SKIN, _selectedNum.Value);
        PlayerPrefsExtension.SetList<int>(ACTIVATED_SKIN, _activatedNumList.ToList());
    }

    public void ChangeSelectedNum(int num)
    {
        _selectedNum.Value = num;
    }

    public void AddList(int num)
    {
        _activatedNumList.Add(num);
    }

    public void RandomActivate()
    { 
        if(excludedNumList.Count > 1)
        {
            int randomNum = UnityEngine.Random.Range(0, excludedNumList.Count);
            int targetRandomNum = excludedNumList[randomNum];
            AddList(targetRandomNum);
            Save();
            //Debug.Log(targetRandomNum);
        }
        else if(excludedNumList.Count == 1)
        {
            AddList(excludedNumList[0]);
            Save();
        }
        else // get over count
        {
            // through
        }
    }

    public List<int> ExcludedNumList(List<int> targetList, List<int> maxList)
    {
        List<int> excludedNumList = new List<int>();
        for (int i = 0; i < maxList.Count; i++) // i start from 1
        {

            if (targetList.Contains(maxList[i]) == false)
            {
                excludedNumList.Add(i);
            }
        }

        return excludedNumList;
    }

    
}
