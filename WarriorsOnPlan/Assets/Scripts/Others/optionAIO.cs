using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enumTranslation { 
    english,
    korean
}

public readonly struct dataBasicWords {
    public readonly string strMelee;
    public readonly string strNumber;

    public dataBasicWords(string parMelee, string parNumber) {
        strMelee = parMelee;
        strNumber = parNumber;
    }
}

// in keybinding (KeyCode)999 is used as any-key
public readonly struct keybindingCombat {
    public readonly KeyCode keyReenactNextAction;
    public readonly KeyCode keyRestorePrevAction;
    public readonly KeyCode keyChangeCombatSpeed;
}

public class optionAIO {
    #region statics
    public int screenWidth { get; private set; } = 1920;
    public int screenHeight { get; private set; } = 1080;

    // stick represents how long is 1.0f of World Space in Screen Space
    public float stick { get; private set; }

    public enumTranslation curTranslation = enumTranslation.english;
    
    public dataBasicWords basicWords { get; private set; }

    #endregion statics

    private List<ICaseResolutionChange> listCaseResolutionChange = new List<ICaseResolutionChange>();

    // ★ option 내역을 저장해두는 json 파일을 하나 만들 것, 게임 실행 시 가장 먼저 그것을 가져와 화면과 언어 등을 설정할 것

    public optionAIO(){
        setStick();

        // ★ 추후 삭제하고 json 파일로부터 저장된 문자열들을 가져올 수 있도록 변경할 것
        basicWords = new dataBasicWords("Melee", "Number");
    }

    private void changeResolution(int parNewWidth, int parNewHeight) {
        // ★ 해상도 변경

        setStick();

        foreach (ICaseResolutionChange crc in listCaseResolutionChange.ToArray()) {
            crc.onResolutionChange(parNewWidth, parNewHeight);
        }
    }

    private void changeTranslation(enumTranslation parEnumTranslation) { 
        // ★ 언어 변경
        // ★ basic keyword json 파일을 만들고 참조하여 기본 단어들 변경
    }

    private void setStick() {
        stick = 
            (Camera.main.GetComponent<Camera>().WorldToScreenPoint(new Vector3(0f, 0f, 0f)) -
            Camera.main.GetComponent<Camera>().WorldToScreenPoint(new Vector3(1f, 0f, 0f))).magnitude;
    } 

    public void addCase(ICaseResolutionChange parCase) {
        listCaseResolutionChange.Add(parCase);
    }

    public void removeCase(ICaseResolutionChange parCase) {
        listCaseResolutionChange.Remove(parCase);
    }
}
