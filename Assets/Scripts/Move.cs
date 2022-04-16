using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    [SerializeField] public float Speed = 50f; // 일반
    [SerializeField] public float ExSpeed = 100f; // 익시드
    [SerializeField] public float rotateSpeed = 3f;

    private double NowSpeed = 0;
    private Rigidbody rb;
    private Text SpeedText;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        SpeedText = GameObject.Find("Speed").GetComponent<Text>();
    }

    private void FixedUpdate() {
        float h = Input.GetAxis("Horizontal");
        float v = -Input.GetAxis("Vertical");

        Vector3 m_EulerAngleVelocity = new Vector3(0f, h * rotateSpeed, 0f);
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity);
        rb.MoveRotation(rb.rotation * deltaRotation);
        rb.MovePosition(transform.position + transform.forward * v * Speed);
    }
}
