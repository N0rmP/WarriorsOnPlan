using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enumTranslation { 
    english,
    korean
}

public class optionAIO {
    #region statics
    public static int screenWidth { get; private set; } = 1920;
    public static int screenHeight { get; private set; } = 1080;

    // stick represents how long is 1.0f of World Space in Screen Space
    public float stick { get; private set; }

    public static enumTranslation curTranslation = enumTranslation.english;
    #endregion statics

    private List<ICaseResolutionChange> listCaseResolutionChange = new List<ICaseResolutionChange>();

    // �� option ������ �����صδ� json ������ �ϳ� ���� ��, ���� ���� �� ���� ���� �װ��� ������ ȭ��� ��� ���� ������ ��

    public optionAIO(){
        setStick();
    }

    private void changeResolution(int parNewWidth, int parNewHeight) {
        // �� �ػ� ����

        setStick();

        foreach (ICaseResolutionChange crc in listCaseResolutionChange.ToArray()) {
            crc.onResolutionChange(parNewWidth, parNewHeight);
        }
    }

    private void setStick() {
        stick = 
            (gameManager.GM.cameraMain.GetComponent<Camera>().WorldToScreenPoint(new Vector3(0f, 0f, 0f)) -
            gameManager.GM.cameraMain.GetComponent<Camera>().WorldToScreenPoint(new Vector3(1f, 1f, 0.5f))).magnitude;
    } 

    public void addCase(ICaseResolutionChange parCase) {
        listCaseResolutionChange.Add(parCase);
    }

    public void removeCase(ICaseResolutionChange parCase) {
        listCaseResolutionChange.Remove(parCase);
    }
}
