using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dataDataBookUI_default", menuName = "ScriptabbleObject/soBookUI")]
public class soBookUI : ScriptableObject, IDataInsurance {
    public string strStartGame;
    public string strOption;
    public string strQuit;
    public string strGrabTheHilt;
    public string strBrandishTheSword;

    public void emergencyInit() {
        strStartGame = "Start Game";
        strOption = "Option";
        strQuit = "Quit";
        strGrabTheHilt = "Grab the Hilt Mode";
        strBrandishTheSword = "Brandish the Sword Mode";
    }
}