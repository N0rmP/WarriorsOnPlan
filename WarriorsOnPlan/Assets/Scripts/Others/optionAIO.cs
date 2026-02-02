using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

/*
public enum enumTranslation { 
    None = -1,
    English = 0,
    Korean = 1
}
*/

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

    #region field_of_option
    public float volumeMaster { get { return curDataOption.MasterVolume; } }
    public float volumeBgm { get { return curDataOption.BgmVolume; } }
    public float volumeSe { get { return curDataOption.SeVolume; } }

    private List<Resolution> listResolution_;
    public IReadOnlyList<Resolution> listResolution {
        get {
            return listResolution_;
        }
    }

    private string[] arrLocaleCode;
    private int indexLocale = -1;
    public string curLocalization { 
        get {
            if (indexLocale < 0 || indexLocale >= arrLocaleCode.Length) {
                return arrLocaleCode[0];
            }
            return arrLocaleCode[indexLocale];
        } 
    }
    #endregion field_of_option
    public void init() {
        // initiating option should be done after audioMixer's snapshot-initiation (after Awake(), during or after Start())
        CO = GameObject.Find("canvasOption").GetComponent<canvasOption>();
        // ★ dataOption에 변조된 값이 입력된 경우 유효한 값으로 제한하는 코드 넣기, dataSaveBasicMap 참조

        // rake Locale code
        List<Locale> tempListLocale = LocalizationSettings.AvailableLocales.Locales;
        arrLocaleCode = new string[tempListLocale.Count];
        for (int i = 0; i < tempListLocale.Count; i++) {
            arrLocaleCode[i] = tempListLocale[i].Identifier.Code;
        }

        /*
            rake resolution that satisfies the conditions below
            1. 16:9 or 16:10 or 21:9 
            2. highest refresh rate among the same screen ratios
        */
        listResolution_ = (from res in Screen.resolutions
                          where (Mathf.Approximately(res.height / (float)res.width, 9f / 16f) ||
                                Mathf.Approximately(res.height / (float)res.width, 10f / 16f) ||
                                Mathf.Approximately(res.height / (float)res.width, 9f / 21f))
                          group res by res.width * res.height into grp
                          select (from rres in grp
                                  orderby rres.refreshRateRatio.value descending
                                  select rres).First()
                         ).OrderByDescending(x => x.width * x.height).ToList<Resolution>();


        trueSetOption(gameManager.GM.SaveC.LOAD<dataOption>("/Save/SaveOption"));
    }

    private void setOption(dataOption parDataOption, bool parIsDeactivateCO = false) {
        if (parDataOption.Localization != indexLocale) {
            gameManager.GM.PC.showPopupConfirm(
                gameManager.GM.DHouC.bookConfirmQuestion.strQuestionChangeTranslation,
                () => {
                    trueSetOption(parDataOption, parIsDeactivateCO);
                    gameManager.GM.SaveC.SAVE<dataOption>("SaveOption.json", curDataOption);
                }
            );
        } else {
            trueSetOption(parDataOption, parIsDeactivateCO);
            gameManager.GM.SaveC.SAVE<dataOption>("SaveOption.json", curDataOption);
        }
    }

    private void trueSetOption(dataOption parDataOption, bool parIsDeactivateCO = false) {
        curDataOption = parDataOption;

        // set audio volume
        gameManager.GM.AC.setVolume(curDataOption.MasterVolume, curDataOption.BgmVolume, curDataOption.SeVolume);

        // set Resolution
        Screen.SetResolution(
            listResolution[curDataOption.ResolutionIndex].width,
            listResolution[curDataOption.ResolutionIndex].height,
            curDataOption.ScreenMode,
            listResolution[curDataOption.ResolutionIndex].refreshRateRatio
        );
        setStick();

        // set Translation, it can also restart the total game if translation is changed
        int tempBufferIndexLocale = indexLocale;
        if (parDataOption.Localization != indexLocale) {
            indexLocale = parDataOption.Localization;
            if (tempBufferIndexLocale >= 0) {
                RESTARTER.RRR.reloadAllScene();
                gameManager.GM.DHouC.prepareBook();
            }
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
