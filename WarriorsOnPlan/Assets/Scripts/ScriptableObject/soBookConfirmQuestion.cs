using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dataBookConfirmQuestion_default", menuName = "ScriptabbleObject/soBookConfirmQuestion")]
public class soBookConfirmQuestion : ScriptableObject, IDataInsurance {
    public string strQuestionResetInitial;
    public string strQuestionChangeTranslation;

    public void emergencyInit() {
        strQuestionResetInitial = "All preparation including tools, circuits, warriors' positions returns to the initial state.";
        strQuestionChangeTranslation = "Changing traslation requires restarting the game.";
    }
}