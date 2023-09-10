using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using MyUtility;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] GameObject sphere;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float rotateSpeed = 1;
    [SerializeField] float horizontalSpeed = 1;
    [SerializeField] FloatingJoystick floatingJoystick;


    // Start is called before the first frame update
    void Start()
    {
        this.OnCollisionEnterAsObservable()
            .Subscribe(collision =>
            {
                if (collision.gameObject.CompareTag("Sticky"))
                {
                    collision.transform.SetParent(sphere.transform);
                    collision.transform.GetComponent<Collider>().enabled = false;

                    // if collision has another sound effect, playing it.
                    SoundEffectManager.Instance.PlayRandomStickSE();

                    VibrationManager.Vibration(VibrationManager.Length.Short);
                }
                
            });

        Rigidbody rb = GetComponent<Rigidbody>();
        this.FixedUpdateAsObservable()
            .Subscribe(_ =>
            {
                rb.MovePosition(this.transform.position + new Vector3(0.1f * floatingJoystick.Horizontal * horizontalSpeed, 0, 0.01f * moveSpeed));

                Quaternion q;
                if (floatingJoystick.Horizontal > 0)
                {
                    q = Quaternion.AngleAxis(2 * rotateSpeed, new Vector3(-1, 0, 1));
                }else if(floatingJoystick.Horizontal == 0)
                {
                    q = Quaternion.AngleAxis(2 * rotateSpeed, new Vector3(1, 0, 0));
                }
                else
                {
                    q = Quaternion.AngleAxis(2 * rotateSpeed, new Vector3(1, 0, 1));
                }
                
                sphere.transform.rotation *= q;

                

            });
    }

    public static int Round(float f)
    {
        if(f > 0)
        {
            return 1;
        }else if (f == 0)
        {
            return 0;
        }
        else
        {
            return -1;
        }
    }
}
