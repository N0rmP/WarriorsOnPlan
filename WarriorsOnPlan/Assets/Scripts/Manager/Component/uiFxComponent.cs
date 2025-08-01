using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class uiFxComponent : MonoBehaviour {
    private Dictionary<Image, (Color changePerSecond, float timerLeft)> containerColorChange;
    private Dictionary<TextMeshProUGUI, int> containerCount;
    private Dictionary<GameObject, (Vector3 destination, float multiplier)> containerMove;

    private canvasSandwitch thisCanvasSandwitch = null;
    private Transform thisCanvasPopup = null;
    private Transform thisCanvasHoveredShown = null;

    // some changes are too frequent with once in a frame, timerSlower represents their change frequency by seconds
    private float timerSlower = 0f;

    #region callback
    public void Awake() {
        containerColorChange = new Dictionary<Image, (Color, float)>();
        containerCount = new Dictionary<TextMeshProUGUI, int>();
        containerMove = new Dictionary<GameObject, (Vector3, float)>();
        
        gameManager.GM.SceC.eventAfterActiveSceneChanged += prepareCanvases;
        prepareCanvases(SceneManager.GetActiveScene());
    }

    public void Update() {     
        funcColorChange(Time.deltaTime);
        funcMove(Time.deltaTime);

        timerSlower -= Time.deltaTime;
        if (timerSlower < 0f) {
            funcCount();
            timerSlower = 0.1f;
        }
    }
    #endregion callback

    #region canvas
    public void prepareCanvases(Scene parScene) {
        GameObject makeCanvas(string parGameObjectName) {
            GameObject tempGameObject = new GameObject(parGameObjectName);
            tempGameObject.AddComponent<Canvas>();
            tempGameObject.AddComponent<GraphicRaycaster>();
            tempGameObject.transform.SetParent(gameManager.GM.canvasMain.transform);
            tempGameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
            tempGameObject.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
            tempGameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
            tempGameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
            return tempGameObject;
        }

        // canvasSandwitch
        if (thisCanvasSandwitch == null) {
            thisCanvasSandwitch = makeCanvas("canvasSandwitch").AddComponent<canvasSandwitch>();
        }        
        thisCanvasSandwitch.transform.SetParent(gameManager.GM.canvasMain.transform);
        thisCanvasSandwitch.transform.SetAsLastSibling();

        // canvasPopup
        if (thisCanvasPopup == null) {
            thisCanvasPopup = makeCanvas("canvasPopup").transform;
            thisCanvasPopup.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            thisCanvasPopup.GetComponent<Image>().enabled = false;
        }        
        thisCanvasPopup.SetParent(gameManager.GM.canvasMain.transform);
        thisCanvasPopup.transform.SetAsLastSibling();

        // canvasHoveredShown
        if (thisCanvasHoveredShown == null) {
            thisCanvasHoveredShown = makeCanvas("canvasHoveredShown").transform;
        }        
        thisCanvasHoveredShown.SetParent(gameManager.GM.canvasMain.transform);
        thisCanvasHoveredShown.transform.SetAsLastSibling();

    }

    public void pushUiActivatable(uiActivatable parUA) {
        thisCanvasSandwitch.pushUiActivatable(parUA);
    }

    public uiActivatable popUiActivatable() {
        return thisCanvasSandwitch.popUiActivatable();
    }

    public void setParentHoveredShown(Transform parTransformHoveredShown) {
        parTransformHoveredShown.SetParent(thisCanvasHoveredShown);
    }

    public void setParentPopup(Transform parTransformPopup) {
        parTransformPopup.SetParent(thisCanvasPopup);
    }

    public void setCurtainPopup(bool parIsEnabled) {
        thisCanvasPopup.GetComponent<Image>().enabled = parIsEnabled;
    }
    #endregion canvas

    #region method_in_update
    void funcColorChange(float parDeltaTime) {
        (Color changePerSecond, float timerLeft) tempTup;

        foreach (Image key in containerColorChange.Keys.ToArray()) {
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
            //if the text reaches destination number, remove it
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
        }
    }

    void funcMove(float parDeltaTime) {
        (Vector3 destination, float multiplier) tempVelocity;
        Vector3 tempStick;

        foreach (GameObject key in containerMove.Keys.ToArray()) {
            tempVelocity = containerMove[key];
            tempStick = tempVelocity.destination - key.GetComponent<RectTransform>().localPosition;

            //if the gameobject approaches destination enough, remove it
            if (tempStick.magnitude < 3f) {
                key.GetComponent<RectTransform>().localPosition = tempVelocity.destination;
                containerMove.Remove(key);
                continue;
            }
            //...move
            key.GetComponent<RectTransform>().localPosition += tempStick.normalized * parDeltaTime * tempVelocity.multiplier * tempStick.magnitude * 5f;
        }
    }
    #endregion method_in_update

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

    public void addMove(GameObject parGameObject, Vector3 parDestination, float parMultiplier = 1f) {
        if (containerMove.ContainsKey(parGameObject)) {
            //if key is already added, update its value
            containerMove[parGameObject] = (parDestination, parMultiplier);
        } else {
            //else add key and value
            containerMove.Add(parGameObject, (parDestination, parMultiplier));
        }
    }

    public void clearAll() {
        containerColorChange.Clear();
        containerCount.Clear();
        containerMove.Clear();
    }
    #endregion addNremove
}

