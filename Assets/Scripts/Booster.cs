using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Booster : MonoBehaviour
{
    public void MakeBooster(uint NOB) {
        for (int i = 1; i < NOB; i++) {
            Transform Panel = GameObject.Find("MoreBooster").transform;
            GameObject Case = new GameObject($"Booster{NOB + 1 - (i)}Case");
            Case.layer = 5;
            RectTransform CaseRect = Case.AddComponent<RectTransform>();
            CaseRect.SetParent(Panel);
            CaseRect.sizeDelta = new Vector2(75f, 75f);
            CaseRect.localScale = new Vector3(1f, 1f, 1f);
            Image CaseImg = Case.AddComponent<Image>();
            CaseImg.color = new Color32(67, 67, 67, 255);
            
            m_Booster($"Booster{NOB + 1 - (i)}", Case).transform.SetParent(Case.transform);
            Panel.GetComponent<RectTransform>().sizeDelta += new Vector2(75, 0);
        }
        GameObject.Find("MoreBooster").transform.GetComponent<RectTransform>().sizeDelta += new Vector2(20, 0);
    }
    public GameObject m_Booster(string Name, GameObject parent) {
        GameObject Booster = new GameObject(Name);
        Booster.layer = 5;
        RectTransform BoosterRect = Booster.AddComponent<RectTransform>();
        BoosterRect.sizeDelta = new Vector2(60f, 60f);
        BoosterRect.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        BoosterRect.anchoredPosition = Vector3.zero;
        BoosterRect.position = Vector3.zero;
        Image BoosterImg = Booster.AddComponent<Image>();
        BoosterImg.sprite = Resources.Load<Sprite>("Drawings/Booster");
        return Booster;
    }
}
