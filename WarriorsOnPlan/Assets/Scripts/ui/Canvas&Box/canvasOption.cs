using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Unity.VisualScripting;

public class canvasOption : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI[] arrTextOption;
    [SerializeField]
    private Slider sliderMasterVolume;
    [SerializeField]
    private Slider sliderBgmVolume;
    [SerializeField]
    private Slider sliderSeVolume;
    [SerializeField]
    private TMP_Dropdown dropdownScreenMode;
    [SerializeField]
    private TMP_Dropdown dropdownResolution;
    [SerializeField]
    private TMP_Dropdown dropdownLanguage;

    #region callback
    void Start() {
        // set text of each option title
        soArbitraryStringArray tempData = gameManager.GM.FC.importResourcesJson<soArbitraryStringArray>("JustText/Option", true);
        for (int i = 0; i < arrTextOption.Length; i++) {
            // SwissArmyStringArray Lack, technically it's (i-1) but last 3 indice have FullScreenMode strings
            if (tempData.SwissArmyStringArray.Length < i - 4) {
                Debug.Log("canvasOption.Start results in an error due to indice differrence - arrTextOption.Length = " + arrTextOption.Length + " / SwissArmyStringArray.length(+3)" + tempData.SwissArmyStringArray.Length);
                break;
            }
            arrTextOption[i].text = tempData.SwissArmyStringArray[i];
        }

        // set dropdown options
        // set dropdownScreenMode, last 3 indice of Option.json are FullScreenMode strings
        dropdownScreenMode.ClearOptions();
        dropdownScreenMode.AddOptions(tempData.SwissArmyStringArray.Skip(tempData.SwissArmyStringArray.Length - 3).ToList());
        // set dropdownResolution
        dropdownResolution.ClearOptions();
        dropdownResolution.AddOptions(
            (from res in gameManager.GM.option.listResolution 
             select (res.width + " * " + res.height)).ToList()
        );
        // dropdownLangauge might be set by editor
    }
    #endregion callback

    public void setCO(dataOption parDO) {
        sliderMasterVolume.value = parDO.MasterVolume;
        sliderBgmVolume.value = parDO.BgmVolume;
        sliderSeVolume.value = parDO.SeVolume;

        dropdownScreenMode.value = (int)(parDO.ScreenMode);
        dropdownResolution.value = parDO.ResolutionIndex;
        dropdownLanguage.value = parDO.Localization;
    }

    public void activateCO(dataOption parDO) {
        setCO(parDO);

        if (GetComponent<uiActivatable>().thisEnumUiActivatableState > enumUiActivatableState.active) {
            GetComponent<uiActivatable>().activatePanel(new Vector3(0f, 0f, 0f));
        }
    }

    public void deactivateCO() {
        GetComponent<uiActivatable>().deactivatePanel();
    }

    public dataOption getDataOption() {
        // FullScreenMode.FullScreenWindow(1) ain't used, so +1 to downScreenMode.value affected by this
        return new dataOption(
            sliderMasterVolume.value, 
            sliderBgmVolume.value, 
            sliderSeVolume.value, 
            (FullScreenMode)(dropdownScreenMode.value > 0 ? dropdownScreenMode.value + 1 : dropdownScreenMode.value),
            dropdownResolution.value, 
            dropdownLanguage.value
        );
    }
}
