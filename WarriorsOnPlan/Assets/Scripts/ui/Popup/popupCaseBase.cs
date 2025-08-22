using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popupCaseBase : MonoBehaviour {
    private List<Image> listImageTotal;

    private float duration;

    public void Awake() {
        listImageTotal = new List<Image>();
        foreach (Image i in transform.GetComponentsInChildren<Image>(true)) {
            listImageTotal.Add(i);
        }
    }

    public void Update() {
        if (duration > 0f) {
            duration -= Time.deltaTime;

            if (duration < 1f) {
                foreach (Image i in listImageTotal) {
                    i.color -= new Color(0f, 0f, 0f, 1f) * Time.deltaTime;
                }
            }

            if (duration <= 0f) {
                gameManager.GM.PC.returnCaseBaseSingle(this);
            }
        }
    }

    public void setPopup(float parDuration = -1f) {
        duration = Mathf.Max(1.01f, parDuration);
    }

    public void appear() {
        duration = -1f;
        foreach (Image i in listImageTotal) {
            i.color += new Color(0f, 0f, 0f, 1f);
        }
    }
}