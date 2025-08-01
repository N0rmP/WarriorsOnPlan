using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enumTranslation { 
    English,
    Korean
}

public class optionAIO {
    #region statics
    public int screenWidth { get; private set; } = 1920;
    public int screenHeight { get; private set; } = 1080;

    // stick represents how long is 1.0f of World Space in Screen Space
    public float stick { get; private set; }

    private enumTranslation curTranslation_ = enumTranslation.English;
    public enumTranslation curTranslation {
        get {
            return curTranslation_;
        }
        set {
            // ★ 게임 재시작 동반할 것, 안 그러면 게임 도중에 UI들 텍스트 갱신이 어려울테니
            curTranslation_ = value;
            gameManager.GM.DHouC.prepareBook();
        }
    }

    #endregion statics

    private List<ICaseResolutionChange> listCaseResolutionChange = new List<ICaseResolutionChange>();

    // ★ option 내역을 저장해두는 json 파일을 하나 만들 것, 게임 실행 시 가장 먼저 그것을 가져와 화면과 언어 등을 설정할 것

    public optionAIO(){
        if (gameManager.GM.option != null) { 
            
        }

        setStick();
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
