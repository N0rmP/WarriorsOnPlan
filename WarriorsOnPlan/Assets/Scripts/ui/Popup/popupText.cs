using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class popupText : MonoBehaviour {
    protected RectTransform thisRectTransform;
    protected Image thisBackground;
    protected TextMeshProUGUI thisText;
    protected CanvasGroup thisCG;

    private float speedBackgroundFade;
    private float speedTextFade;
    private float duration = 0f;

    public void Awake() {
        // transform.GetChild(0).GetComponent<TextMeshProUGUI>().richText = false;
        thisRectTransform = GetComponent<RectTransform>();
        thisCG = GetComponent<CanvasGroup>();
        if (!TryGetComponent<Image>(out thisBackground)) {
            thisBackground = transform.GetComponentInChildren<Image>();
        }
        if (!TryGetComponent<TextMeshProUGUI>(out thisText)) {
            thisText = transform.GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void Update() {
        if (duration > 0f) {
            duration -= Time.deltaTime;

            if (duration < 1f) {
                thisBackground.color -= new Color(0f, 0f, 0f, speedBackgroundFade) * Time.deltaTime;
                thisText.color -= new Color(0f, 0f, 0f, speedTextFade) * Time.deltaTime;
            }

            if (duration <= 0f) {
                returnThis();
            }
        }
    }

    public void setPopupText(Color parTextColor, Color parBackgroundColor, string parString = "", float parDuration = -1f) {
        thisText.text = parString;
        thisText.color = parTextColor;
        thisBackground.color = parBackgroundColor;

        GetComponent<RectTransform>().resizeToChildSize();

        float tempDuration = Mathf.Max(1.01f, (parDuration > 0f) ? parDuration : 1f + (parString.Length *
            gameManager.GM.option.curLocalization switch {
                "en" => 0.1f,
                "ko-KR" => 0.5f,
                _ => 1f
            }));
        speedBackgroundFade = thisBackground.color.a;
        speedTextFade = thisText.color.a;

        thisCG.alpha = 1f;
        GetComponent<RectTransform>().localScale = new Vector3(2f, 2f, 1f);
        Sequence tempSQ = DOTween.Sequence();
        tempSQ.Append(DOTween.To(() => thisRectTransform.localScale, (x) => thisRectTransform.localScale = x, Vector3.one, 0.2f));
        tempSQ.AppendInterval(tempDuration);
        tempSQ.Append(DOTween.To(() => thisCG.alpha, (x) => thisCG.alpha = x, 0f, 1f));
        tempSQ.AppendCallback(returnThis);
    }

    protected virtual void returnThis(){
        gameManager.GM.PC.returnTextSingle(this);
    }
}
