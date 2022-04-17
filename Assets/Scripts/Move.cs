using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    [Header ("카트바디 성능")]
    public float AccelSpeed = 1f; // 일반
    public float MaxSpeed = 50f; // 일반 최고속도
    public EXType ExeedType; // 익시드 타입
    private float ExAccellSpeed = 1.5f; // 익시드
    private float MaxExSpeed = 75f; // 익시드 최고속도
    private float DelExeedSpeed = 0.001f;
    [Range (0f, 1f)] public float ExeedGauge = 0f; // 시작시 모여있는 익시드
    public float rotateSpeed = 0.5f; // 회전 속도
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
        if (ExeedType == EXType.L) {
            ExAccellSpeed = 1.5f;
            MaxExSpeed = 75f;
            DelExeedSpeed = 0.001f;
        }
        else if (ExeedType == EXType.B) {
            ExAccellSpeed = 1.75f;
            MaxExSpeed = 85f;
            DelExeedSpeed = 0.0015f;
        }
        else {
            ExAccellSpeed = 2f;
            MaxExSpeed = 95f;
            DelExeedSpeed = 0.002f;
        }
    }
    private async void Update() {
        rotateSpeed = 0.5f;
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Space)) await Exeed();
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) rotateSpeed = 3f;
    }
    private void FixedUpdate() {
        float h = Input.GetAxis("Horizontal");
        float v = -Input.GetAxis("Vertical");

        Vector3 m_EulerAngleVelocity = new Vector3(0f, h * rotateSpeed, 0f);
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity);
        rb.MoveRotation(rb.rotation * deltaRotation);
        if (v != 0f && !OnExeed) ExeedGauge += 0.001f;
        if (OnExeed && NowSpeed < MaxExSpeed) rb.AddRelativeForce(Vector3.forward * v * ExAccellSpeed, ForceMode.Impulse);
        else {
            if (NowSpeed < MaxSpeed) rb.AddRelativeForce(Vector3.forward * v * AccelSpeed, ForceMode.Impulse); // 최고속도가 넘지 않는다면 가속
            if (ExeedGauge > 1f) ExeedGauge = 1f;
            else ExeedGaugeImage.fillAmount = ExeedGauge;
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
        while (true) {
            if (ExeedGaugeImage.fillAmount == 0f) {
                OnExeed = false;
                return;
            }
            ExeedGaugeImage.fillAmount -= DelExeedSpeed;
            ExeedGauge -= DelExeedSpeed;
            await Task.Delay(1);
        }
    }
    public enum EXType{
	    L, B, S
    }
}
