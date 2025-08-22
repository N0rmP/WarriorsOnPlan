using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum enumTranslation { 
    English = 0,
    Korean = 1
}

public class optionAIO {
    private static canvasOption CO;

    private static dataOption curDataOption;

    // stick represents how long is 1.0f of World Space in Screen Space
    // 0.86f is sqrt(3) / 2, the ratio of length when we see the object from 60 degree
    public float stick { get; private set; }
    public float stickDegreed {
        get {
            return stick * 0.86f;
        }
    }

    private List<(int, int)> listResolution;

    #region options
    public float volumeMaster { get { return curDataOption.MasterVolume; } }
    public float volumeBgm { get { return curDataOption.BgmVolume; } }
    public float volumeSe { get { return curDataOption.SeVolume; } }

    private enumTranslation curTranslation_ = enumTranslation.English;
    public enumTranslation curTranslation { get { return curTranslation_; } }
    #endregion options
    public void init() {
        // initiating option should be done after audioMixer's snapshot-initiation (after Awake(), during or after Start())
        CO = GameObject.Find("canvasOption").GetComponent<canvasOption>();
        trueSetOption(gameManager.GM.SaveC.LOAD<dataOption>("/Save/SaveOption"));

        // rake resolution
        listResolution = new List<(int, int)>();
        foreach (Resolution rr in Screen.resolutions) {
            // ¡Ú if(rr.width)
        }
    }

    private void setOption(dataOption parDataOption, bool parIsDeactivateCO = false) {
        if (parDataOption.Translation != curTranslation) {
            gameManager.GM.PC.showPopupConfirm(
                gameManager.GM.DHouC.bookConfirmQuestion.strQuestionChangeTranslation,
                () => trueSetOption(parDataOption, parIsDeactivateCO)
            );
        } else {
            trueSetOption(parDataOption, parIsDeactivateCO);
        }
    }

    private void trueSetOption(dataOption parDataOption, bool parIsDeactivateCO = false) {
        // set curDataOption and save it
        curDataOption = parDataOption;
        gameManager.GM.SaveC.SAVE<dataOption>("SaveOption.json", curDataOption);

        // set audio volume
        gameManager.GM.AC.setVolume(curDataOption.MasterVolume, curDataOption.BgmVolume, curDataOption.SeVolume);

        // set Resolution
        Resolution tempResolution = Screen.resolutions[curDataOption.ResolutionIndex];
        Screen.SetResolution(
            tempResolution.width,
            tempResolution.height,
            curDataOption.ScreenMode,
            tempResolution.refreshRateRatio
        );
        setStick();

        // set Translation, it can also restart the total game if translation is changed
        if (parDataOption.Translation != curTranslation_) {
            RESTARTER.RRR.reloadAllScene();
            curTranslation_ = parDataOption.Translation;
            gameManager.GM.DHouC.prepareBook();
        }

        if (parIsDeactivateCO) {
            deactivateCO();
        }
    }

    private void setStick() {
        stick = 
            ((Camera.main.GetComponent<Camera>().WorldToScreenPoint(new Vector3(1f, 0f, 0f))
            - Camera.main.GetComponent<Camera>().WorldToScreenPoint(new Vector3(0f, 0f, 0f)))
             / (Vector2)(gameManager.GM.canvasMain.transform.localScale)).magnitude;
    }

    public void resetOption() {
        dataOption tempDataOption = default;
        tempDataOption.emergencyInit();
        setOption(tempDataOption);
        CO.setCO(curDataOption);
    }

    #region canvasOption
    public void activateCO() {
        CO.activateCO(curDataOption);
    }

    public void deactivateCO() {
        CO.deactivateCO();
    }

    public void confirmCO() {
        setOption(CO.getDataOption(), true);
    }
    #endregion canvasOption
}
