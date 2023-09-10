using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Diagnostics;
using TMPro;
using DG.Tweening;

public class ObjectPointDisplayer : MonoBehaviour
{
    [SerializeField] Canvas parent;
    [SerializeField] GameObject pointText;
    [SerializeField] PlayerCore playerCore;

    // Start is called before the first frame update
    void Start()
    {
        playerCore.objectSize
            //.Debug("ObjectSize")
            .Where(SizeCriterion => SizeCriterion != null && SizeCriterion.size != 0)
            .Subscribe(SizeCriterion =>
            {
                Instantiate(pointText, parent:parent.transform);
                pointText.GetComponent<TextMeshProUGUI>().text = string.Format("+{0}", SizeCriterion.size);
            });

        // Destroy object on Animation script
    }
}
