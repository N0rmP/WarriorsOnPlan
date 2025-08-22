using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class popupText : MonoBehaviour {
    [SerializeField]
    protected Image thisBackground;
    [SerializeField]
    protected TextMeshProUGUI thisText;

    private float speedBackgroundFade;
    private float speedTextFade;

    private float duration = 0f;

    public void Awake() {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().richText = false;
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

        transform.GetChild(0).GetComponent<ContentSizeFitter>().SetLayoutHorizontal();
        transform.GetChild(0).GetComponent<ContentSizeFitter>().SetLayoutVertical();
        GetComponent<RectTransform>().sizeDelta = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;

        duration = Mathf.Max(1.01f, (parDuration > 0f) ? parDuration : 1f + (parString.Length *
            gameManager.GM.option.curTranslation switch {
                enumTranslation.English => 0.08f,
                enumTranslation.Korean => 0.4f,
                _ => 1f
            }));
        speedBackgroundFade = thisBackground.color.a;
        speedTextFade = thisText.color.a;
    }

    protected virtual void returnThis(){
        gameManager.GM.PC.returnTextSingle(this);
    }
}
