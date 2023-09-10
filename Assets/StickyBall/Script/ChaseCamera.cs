using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;
using DG.Tweening;

public class ChaseCamera : MonoBehaviour
{
    public Camera cam ;
    public Vector3 offset { get; private set; }
    [SerializeField] GameObject player;
    PlayerCore playerCore;
    [SerializeField] private Vector3 diffrenceOffset = new Vector3(0, 5, -5);
    const float DURATION = 0.2f;


    void Start()
    {
        cam = Camera.main;
        // Remove Parent.
        this.transform.parent = null;
        offset = this.cam.transform.position - this.player.transform.position;
        playerCore = player.GetComponent<PlayerCore>();

        playerCore.ballSize
            .Where(size => size != 1)
            .Subscribe(size => UpdatePosition(size))
            .AddTo(this);
    }

    private void UpdatePosition(byte size)
    {
        this.offset += diffrenceOffset * size;
    }
 
    private void LateUpdate()
    {
        // Moving Camera
        this.cam.transform.DOMove(this.player.transform.position + offset, DURATION);
    }

}
