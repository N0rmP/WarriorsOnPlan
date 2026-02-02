using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dataBookWords_default", menuName = "ScriptabbleObject/soBookWords")]
public class soBookWords : ScriptableObject, IDataInsurance {
    public string strMelee;
    public string strNumber;
    public string strReady;
    public string strVictory;
    public string strDefeated;

    public string strTool;
    public string strEffect;
    public string strSkill;
    public string strUpgrade;

    public string strInterfere;
    public string strAction;
    public string strAdd;
    public string strAttack;
    public string strConrolled;
    public string strDamaged;
    public string strDealDamage;
    public string strDeath;
    public string strFocussing;
    public string strForcedMove;
    public string strHpDecrease;
    public string strHpIncrease;
    public string strMove;
    public string strRemoving;

    public void emergencyInit() {
        strMelee = "Melee";
        strNumber = "(Number)";
        strReady = "Ready";
        strVictory = "Victory";
        strDefeated = "Defeated";

        strTool = "Tool";
        strEffect = "Effect";
        strSkill = "Skill";
        strUpgrade = "Upgrade";

        strInterfere = "Denied";
        strAction = "Action";
        strAdd = "Adding";
        strAttack = "Attack";
        strConrolled = "Controlled";
        strDamaged = "Taking Damage";
        strDealDamage = "Dealing Damage";
        strDeath = "Death";
        strFocussing = "Focussing";
        strForcedMove = "Forced Move";
        strHpDecrease = "Hp Decrease";
        strHpIncrease = "Hp Increase";
        strMove = "Move";
        strRemoving = "Removing";
    }
}