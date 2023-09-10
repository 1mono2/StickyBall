using UnityEngine;

public class CanvasOperator : MonoBehaviour
{
    [SerializeField] Canvas targetCanvas;

    public void SetActiveCanvas(bool setBool)
    {
        targetCanvas.gameObject.SetActive(setBool);
    }
}
