using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomChoiceSystem :MonoBehaviour
{
    [SerializeField] CheckButtonInteractable[] checkButtonInteractables;

    [SerializeField] Canvas pickUpSkinDirectionCanvas;
    [SerializeField] Image randomFrame;
    List<RectTransform> buttonRects = new List<RectTransform>();

    const string SAVE_ACTIVE_SKIN = "SAVE_ACTIVE_SKIN";

    private void Start()
    {
        foreach (CheckButtonInteractable check in checkButtonInteractables)
            buttonRects.Add(check.rect);

        //if (PlayerPrefs.HasKey(SAVE_ACTIVE_SKIN))
        //{
        //    PlayerPrefs.
        //}
    }

    public void Offer()
    {

        if(checkButtonInteractables != null)
        {

            List<int> numOfButtonUninteractableList;

            if(TryGetUninteractable(out numOfButtonUninteractableList))
            {

                var randomNum = RandomFromList(numOfButtonUninteractableList);
                // PlayerPlaf

                // Skin Direction
                PickUpSkinDirection skinDirection = new PickUpSkinDirection(pickUpSkinDirectionCanvas, randomFrame,
                    randomNum, numOfButtonUninteractableList, buttonRects);

                skinDirection.completeHandler += () => { EnableNum(randomNum); };

                skinDirection.StartDirection();

                PlayerPrefs.SetInt(SAVE_ACTIVE_SKIN, randomNum);

            }

        }
        
    }

    bool TryGetUninteractable(out List<int> numOfButtonUninteractableList)
    {
        numOfButtonUninteractableList = new List<int>();

        for (int i = 0; i < checkButtonInteractables.Length; i++)
        {
            if (!checkButtonInteractables[i].GetInteractable())
            {
                numOfButtonUninteractableList.Add(i);
            }
            
        }

        if (numOfButtonUninteractableList.Count != 0)
        {
            return true;
        }
        else
        {
            return false;
        }

        
    }


    int RandomFromList(List<int> intList)
    {
        var numRandom = Random.Range(0, intList.Count); // Range doesn't include max int, but include max float.
        return intList[numRandom];
    }

    void EnableNum(int randomNum)
    {
        checkButtonInteractables[randomNum].SetInteractable(true);
    }
     
}
