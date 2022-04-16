using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    [SerializeField] public float Speed = 1f; // 일반
    [SerializeField] public float ExSpeed = 2f; // 익시드
    [SerializeField] public float rotateSpeed = 0.5f; // 회전 속도

    #region 주행 관련 전역변수
    private bool OnExeed = false;
    private double NowSpeed = 0;
    #endregion
    private Rigidbody rb;
    private Text SpeedText;
    private Vector3 oldPosition;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        SpeedText = GameObject.Find("Speed").GetComponent<Text>();
    }

    private async void FixedUpdate() {
        float h = Input.GetAxis("Horizontal");
        float v = -Input.GetAxis("Vertical");

        Vector3 m_EulerAngleVelocity = new Vector3(0f, h * rotateSpeed, 0f);
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity);
        rb.MoveRotation(rb.rotation * deltaRotation);
        // Vector3 vel = rb.velocity;
        // if (vel.magnitude < Speed) {
        //     rb.velocity = vel.normalized * Speed;
        // }
        if (Input.GetKeyDown(KeyCode.Space)) await Exeed();
        if (OnExeed) rb.MovePosition(
            transform.position + transform.forward * v * ExSpeed
        );
        else rb.MovePosition(
            transform.position + transform.forward * v * Speed
        );
        NowSpeed = (
            (
                (transform.position - oldPosition).magnitude)
                / Time.deltaTime
            );
        SpeedText.text = $"{Math.Round(NowSpeed)}km/h";
        oldPosition = transform.position;
    }
    private async Task Exeed() {
        Debug.Log("안녕1");
        if (OnExeed) return;
        OnExeed = true;
        await Task.Delay(1000);
        OnExeed = false;
    }
}
