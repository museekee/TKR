using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    [System.Serializable]
    public class Parameter {
        [Header ("일반")]
        public float AccelSpeed = 1f; // 일반
        public float MaxSpeed = 50f; // 일반 최고속도
        public float rotateSpeed = 0.5f; // 회전 속도
        [Header ("익시드")]
        public EXType ExeedType; // 익시드 타입
        [HideInInspector] public float ExAccellSpeed = 1.5f; // 익시드
        [HideInInspector] public float MaxExSpeed = 75f; // 익시드 최고속도
        [HideInInspector] public float DelExeedSpeed = 0.001f; // 익시드 사용 속도
        [Range (0f, 1f)] public float ExeedGauge = 0f; // 시작시 모여있는 익시드
        public float AddExeed = 0.001f;
        public int MinExeedCharge = 25;
        [Header ("부스터")]
        public uint NumOfBooster = 2;
        public float BoosterChargeSpeed = 0.005f;
    }
    public Parameter Param = new Parameter();
    [Header ("게임 오브젝트")]
    public Text SpeedText;
    public Image ExeedGaugeImage;
    public Image BoosterGaugeImage;
    public Canvas HUD;

    #region 주행 관련 전역변수
    private bool OnExeed = false;
    private double NowSpeed = 0;
    #endregion
    private Rigidbody rb;
    private Vector3 oldPosition;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        HUD.GetComponent<Booster>().MakeBooster(Param.NumOfBooster);
        if (Param.ExeedType == EXType.L) {
            Param.ExAccellSpeed = 1.5f;
            Param.MaxExSpeed = 75f;
            Param.DelExeedSpeed = 0.001f;
        }
        else if (Param.ExeedType == EXType.B) {
            Param.ExAccellSpeed = 1.75f;
            Param.MaxExSpeed = 85f;
            Param.DelExeedSpeed = 0.0015f;
        }
        else {
            Param.ExAccellSpeed = 2f;
            Param.MaxExSpeed = 95f;
            Param.DelExeedSpeed = 0.002f;
        }
    }
    private async void Update() {
        Param.rotateSpeed = 0.5f;
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Space)) await Exeed();
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            BoosterGaugeImage.fillAmount += Param.BoosterChargeSpeed;
            Param.rotateSpeed = 3f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)) {
            if (BoosterGaugeImage.fillAmount != 1f) return;
            BoosterGaugeImage.fillAmount = 0f;
            for (int i = 1; i < Param.NumOfBooster + 1; i++) { 
                if (GameObject.Find($"Booster{Param.NumOfBooster}Case").transform.childCount == 0) {
                    GameObject Booster = HUD.GetComponent<Booster>().m_Booster(
                        $"Booster{Param.NumOfBooster}",
                        GameObject.Find($"Booster{Param.NumOfBooster}Case")
                    );
                    Booster.transform.position = Vector3.zero;
                    Booster.transform.localPosition = Vector3.zero;
                    Debug.Log(Booster.transform.localPosition);
                    Booster.transform.SetParent(
                        GameObject.Find($"Booster{Param.NumOfBooster}Case")
                        .GetComponent<RectTransform>().transform
                    );
                    return;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) {
            for (int i = 0; i < Param.NumOfBooster; i++) {
                GameObject Booster = GameObject.Find($"Booster{Param.NumOfBooster - i}Case");
                if (GameObject.Find($"Booster{Param.NumOfBooster - i}") != null) {
                    Destroy(GameObject.Find($"Booster{Param.NumOfBooster - i}"));
                    return;
                }
            }
        }
    }
    private void FixedUpdate() {
        float h = Input.GetAxis("Horizontal");
        float v = -Input.GetAxis("Vertical");

        Vector3 m_EulerAngleVelocity;
        if (v < 0) m_EulerAngleVelocity = new Vector3(0f, h * Param.rotateSpeed, 0f); // 전진 시 회전
        else m_EulerAngleVelocity = new Vector3(0f, -h * Param.rotateSpeed, 0f); // 후진 시 회전

        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity);
        rb.MoveRotation(rb.rotation * deltaRotation);
        if (!OnExeed && Math.Round(NowSpeed) > Param.MinExeedCharge) Param.ExeedGauge += Param.AddExeed;
        if (OnExeed && NowSpeed < Param.MaxExSpeed) rb.AddRelativeForce(Vector3.forward * v * Param.ExAccellSpeed, ForceMode.Impulse);
        else {
            if (NowSpeed < Param.MaxSpeed) rb.AddRelativeForce(Vector3.forward * v * Param.AccelSpeed, ForceMode.Impulse); // 최고속도가 넘지 않는다면 가속
            if (Param.ExeedGauge > 1f) Param.ExeedGauge = 1f;
            else ExeedGaugeImage.fillAmount = Param.ExeedGauge;
        }
        NowSpeed = (
            (
                (transform.position - oldPosition).magnitude)
                / Time.deltaTime
            );
        SpeedText.text = Math.Round(NowSpeed).ToString();
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
            ExeedGaugeImage.fillAmount -= Param.DelExeedSpeed;
            Param.ExeedGauge -= Param.DelExeedSpeed;
            await Task.Delay(1);
        }
    }
    public enum EXType{
	    L, B, S
    }
}
