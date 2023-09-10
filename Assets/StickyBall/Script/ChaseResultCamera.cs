using DG.Tweening;
using UnityEngine;

public class ChaseResultCamera : MonoBehaviour
{
    public Camera cam;
    public Vector3 offset { get; private set; }
    [SerializeField] GameObject target;

    const float DURATION = 0.2f;

    void Start()
    {
        // Remove Parent.
        this.transform.parent = null;
        offset = this.cam.transform.position - this.target.transform.position;
    }

    private void LateUpdate()
    {
        // Moving Camera
        this.cam.transform.DOMove(this.target.transform.position + offset, DURATION);
    }
}
