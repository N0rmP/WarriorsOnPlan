using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mapUIComponent {
    public static float ZOOM { get; private set; } = 1f;
    private static float mapWidthOriginal;
    private static float mapHeightOriginal;

    // field for Level
    private carrierGeneric<buttonLevel> carrierButtonLevel;
    public RectTransform contentMap { get; private set; }
    private Dictionary<int, buttonLevel> dictCodeButtonLevel { get; set; }  // ★ 이거 필요한지 참조 살펴보고 필요없으면 삭제할 것

    public canvasUpgrade CU { get; private set; }

    public mapUIComponent() {
        GameObject tempPrefabButtonLevel = Resources.Load<GameObject>("Prefab/UI/buttonLevel");
        carrierButtonLevel = new carrierGeneric<buttonLevel>(
            () => {
                buttonLevel tempResult = GameObject.Instantiate(tempPrefabButtonLevel).GetComponent<buttonLevel>();
                tempResult.transform.SetParent(contentMap.transform.GetChild(0).transform);
                return tempResult;
            },
            (x) => {
                x.gameObject.SetActive(false);
            }
        );
        contentMap = GameObject.Find("contentMap").GetComponent<RectTransform>();
        dictCodeButtonLevel = new Dictionary<int, buttonLevel>();

        mapWidthOriginal = contentMap.sizeDelta.x;
        mapHeightOriginal = contentMap.sizeDelta.y;

        CU = GameObject.Find("canvasUpgrade").GetComponent<canvasUpgrade>();

        /*
        // initiation when scene changes for key
        gameManager.GM.SceC.eventAfterActiveSceneChanged += (x) => {
            if (x.name != "SceneCombat") {
                return;
            }
            setZoom(1f, false);
            gameManager.GM.BIC.addScrollUp(() => setZoom(0.1f));
            gameManager.GM.BIC.addScrollDown(() => setZoom(-0.1f));
        };
        */
        gameManager.GM.IC.addScrollUp("SceneCombat", () => setZoom(0.1f));
        gameManager.GM.IC.addScrollDown("SceneCombat", () => setZoom(-0.1f));
    }

    private void setZoom(float parValue, bool parIsPlus = true) {
        ZOOM = Mathf.Clamp(
            parIsPlus ? ZOOM + parValue : parValue, 
            1f, 1.5f);
        contentMap.sizeDelta = new Vector2(mapWidthOriginal * ZOOM, mapHeightOriginal * ZOOM);
    }

    #region buttonLevel
    public void prepareButtonLevel(int parCode, bool parIsClear) {
        buttonLevel tempButtonLevel = carrierButtonLevel.getInterceptor();
        RectTransform tempRectTransform = tempButtonLevel.GetComponent<RectTransform>();
        dataLevel tempDataLevel = gameManager.GM.DHouC.getDataLevel(parCode);

        tempRectTransform.gameObject.name += "_" + tempDataLevel.LevelCode.ToString();
        tempRectTransform.anchorMin = tempRectTransform.anchorMax = new Vector2(tempDataLevel.MapPosition[0], tempDataLevel.MapPosition[0]);
        tempRectTransform.anchoredPosition = new Vector2(0f, 0f);

        tempButtonLevel.gameObject.SetActive(true);
        tempButtonLevel.prepareButton(tempDataLevel, parIsClear);

        dictCodeButtonLevel.Add(parCode, tempButtonLevel);
    }

    public void clearButtonLevel() {
        carrierButtonLevel.returnTotal();
        dictCodeButtonLevel.Clear();
    }
    #endregion buttonLevel
}
