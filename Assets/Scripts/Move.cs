using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    [Header ("카트바디 성능")]
    public float Speed = 1f; // 일반
    public float ExSpeed = 2f; // 익시드
    public float rotateSpeed = 0.5f; // 회전 속도
    [Range (0f, 1f)] public float ExeedGauge = 0f;
    [Header ("게임 오브젝트")]
    public Text SpeedText;
    public Image ExeedGaugeImage;

    #region 주행 관련 전역변수
    private bool OnExeed = false;
    private double NowSpeed = 0;
    #endregion
    private Rigidbody rb;
    private Vector3 oldPosition;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    private async void Update() {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Space)) await Exeed();
    }
    private void FixedUpdate() {
        float h = Input.GetAxis("Horizontal");
        float v = -Input.GetAxis("Vertical");

        Vector3 m_EulerAngleVelocity = new Vector3(0f, h * rotateSpeed, 0f);
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity);
        rb.MoveRotation(rb.rotation * deltaRotation);
        // Vector3 vel = rb.velocity;
        // if (vel.magnitude < Speed) {
        //     rb.velocity = vel.normalized * Speed;
        // }
        if (v != 0f && !OnExeed) ExeedGauge += 0.001f;
        if (OnExeed) rb.MovePosition(
            transform.position + transform.forward * v * ExSpeed
        );
        else {
            rb.MovePosition(
                transform.position + transform.forward * v * Speed
            );
            ExeedGaugeImage.fillAmount = ExeedGauge;
        }
        NowSpeed = (
            (
                (transform.position - oldPosition).magnitude)
                / Time.deltaTime
            );
        SpeedText.text = $"{Math.Round(NowSpeed)}km/h";
        oldPosition = transform.position;
    }
    private async Task Exeed() {
        if (OnExeed) return;
        OnExeed = true;
        for (int i = 0; i < ExeedGaugeImage.fillAmount*1000; i++) {
            ExeedGaugeImage.fillAmount -= 0.001f;
            ExeedGauge -= 0.001f;
            await Task.Delay(1);
        }
        OnExeed = false;
    }
}
