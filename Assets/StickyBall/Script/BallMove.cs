using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

[RequireComponent(typeof(PlayerCore))]
public class BallMove : MonoBehaviour
{
    public float speed;
    const float speedMultiply = 1.3f;
    public FloatingJoystick floatingJoystick;
    public Rigidbody rb;
    private PlayerCore playerCore;
    public bool enableToMove = false;
    public bool enableToRotate = false;

    private void Start()
    {
        playerCore = GetComponent<PlayerCore>();
        playerCore.ballSize
            .Where(size => size != 1)
            .Subscribe(size =>
        {
            speed *= speedMultiply;
        }).AddTo(this);

    }

    public void Stop()
    {
        rb.velocity = new Vector3(0, 0, 0);
    }

    private void FixedUpdate()
    {
        if (enableToMove)
        {
            Vector3 direction = Vector3.forward * floatingJoystick.Vertical + Vector3.right * floatingJoystick.Horizontal;
            rb.velocity = new Vector3(direction.x * speed * Time.fixedDeltaTime, rb.velocity.y, direction.z * speed * Time.fixedDeltaTime);

        }

        if (enableToRotate)
        {
            Quaternion q = Quaternion.AngleAxis(-2, Vector3.right);
            this.transform.rotation *= q;
        }

        //rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.Acceleration);
        /*if(direction.magnitude > 1)
        {
            direction.Normalize();
        }*/

        //rb.AddForce(direction  * (8 * direction.magnitude - rb.velocity.magnitude) / Time.fixedDeltaTime, ForceMode.Acceleration);
        //rb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);

        /*if (direction == Vector3.zero)
        {
           rb.angularVelocity = Vector3.zero;
        }
        else
        {
            rb.velocity = direction * speed * Time.fixedDeltaTime;
        }*/
    }
}
