using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Notifications.iOS;
using UnityEngine;
using UnityEngine.UI;

public class uiFxComponent : MonoBehaviour
{
    private Dictionary<Image, (Color changePerSecond, float timerLeft)> containerColorChange;
    private Dictionary<TextMeshProUGUI, int> containerCount;

    private float timerSlower = 0f;

    public void Awake() {
        containerColorChange = new Dictionary<Image, (Color changePerSecond, float timerLeft)>();
        containerCount = new Dictionary<TextMeshProUGUI, int>();
    }

    public void LateUpdate() {

        void funcColorChange(float parDeltaTime) {
            (Color changePerSecond, float timerLeft) tempTup;

            foreach(Image key in containerColorChange.Keys.ToArray()){
                tempTup = containerColorChange[key];
                //color change
                key.color += tempTup.changePerSecond * parDeltaTime;
                //timer update
                tempTup.timerLeft -= parDeltaTime;
                //container management
                if (tempTup.timerLeft <= 0.01f) {
                    containerColorChange.Remove(key);
                }
            }
        }

        void funcCount() {
            int tempCurText;
            int tempResult;

            foreach (TextMeshProUGUI key in containerCount.Keys.ToArray()) {
                tempCurText = Convert.ToInt32(key.text);
                //if the text reaches the destination number, remove it
                if (tempCurText == containerCount[key]) {
                    containerCount.Remove(key);
                    continue;
                }
                //basically Count change the value with average, it accelerates the number change speed as much as the gap between departure and destination
                tempResult = (containerCount[key] + tempCurText) / 2;
                //if the gap is too small that change with average doesn't work, plus or minus one manually
                if (tempResult == tempCurText) {
                    tempResult = (tempCurText > containerCount[key]) ? (tempCurText - 1) : (tempCurText + 1);
                }
                key.text = tempResult.ToString();
                Debug.Log(key.text);
            }
        }

        funcColorChange(Time.deltaTime);

        timerSlower -= Time.deltaTime;
        if (timerSlower < 0f) {
            funcCount();
            timerSlower = 0.1f;
        }
    }

    #region addNremove
    public void addColorChange(Image parImg, Color parDestinationColor, float parTimerMax) {
        if (containerColorChange.ContainsKey(parImg)) {
            //if key is already added, update its value
            containerColorChange[parImg] = ((parDestinationColor - parImg.color) / parTimerMax, parTimerMax);
        } else {
            //else add key and value
            containerColorChange.Add(parImg, ((parDestinationColor - parImg.color) / parTimerMax, parTimerMax));
        }
    }

    public void removeColorChange(Image parImg) {
        containerColorChange.Remove(parImg);
    }

    public void addCount(TextMeshProUGUI parText, int parDestinationValue) {
        if (containerCount.ContainsKey(parText)) {
            //if key is already added, sum its value and parDestinationValue
            containerCount[parText] = parDestinationValue;
        } else {
            //else add key and value
            containerCount.Add(parText, parDestinationValue);
        }
    }
    #endregion addNremove
}
