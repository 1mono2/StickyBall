using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;

[RequireComponent(typeof(TextMeshProUGUI))]
public class BallQuantitySizeDisplayer : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI ballQuantitySizeText;
    [SerializeField] PlayerCore playerCore;

    void Start()
    {
        playerCore.ballQuantitySize
            .Subscribe(ballQuantitySize =>
            {
                string st = ballQuantitySize.ToString();
                int length = st.Length;

          
                string centimeter = st;
                string meter = "0";

                if (length >= 3)
                {
                    centimeter = st.Substring(length - 2, 2);
                    meter = st.Substring(0, length - 2);
                }

                ballQuantitySizeText.text = $"{meter}m{centimeter}cm";
            });
    }


}
