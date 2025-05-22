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

    // �� option ������ �����صδ� json ������ �ϳ� ���� ��, ���� ���� �� ���� ���� �װ��� ������ ȭ��� ��� ���� ������ ��

    public optionAIO(){
        setStick();

        // �� ���� �����ϰ� json ���Ϸκ��� ����� ���ڿ����� ������ �� �ֵ��� ������ ��
        basicWords = new dataBasicWords("Melee", "Number");
    }

    private void changeResolution(int parNewWidth, int parNewHeight) {
        // �� �ػ� ����

        setStick();

        foreach (ICaseResolutionChange crc in listCaseResolutionChange.ToArray()) {
            crc.onResolutionChange(parNewWidth, parNewHeight);
        }
    }

    private void changeTranslation(enumTranslation parEnumTranslation) { 
        // �� ��� ����
        // �� basic keyword json ������ ����� �����Ͽ� �⺻ �ܾ�� ����
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
