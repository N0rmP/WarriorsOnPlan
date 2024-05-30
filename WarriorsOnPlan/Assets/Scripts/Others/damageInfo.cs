using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageInfo
{
    public readonly caseAll sourceCaseAll;

    private int damage_;
    //there are three kinds of damage changers, add / multiply / fix
    //basically add is processed first, and multiply is processed after it
    //fix ignores all other changers and fix damage strictly, you should be careful when fixing damage
    private int totalAdd;
    private float totalMultiply;
    private int fixedDamage;

    private Action<Thing> delEffect;

    public int damage { get { return damage_; } }
    public enumDamageType damageType { get; set; }

    public damageInfo(caseAll parSourceCaseAll, int parDamage, enumDamageType parDType, Action<Thing> parDelegate) {
        sourceCaseAll = parSourceCaseAll;
        damage_ = parDamage;
        damageType = parDType;
        totalAdd = 0;
        totalMultiply = 1.0f;
        fixedDamage = -1;
        delEffect = parDelegate;
    }

    public void addDamage(int parValue) {
        //magic damage can't decrease
        if ((damageType == enumDamageType.magic) && (parValue < 0)) {
            return;
        }

        totalAdd += parValue;
        if (totalAdd < 0) { totalAdd = 0; }
    }

    //parameter of multiplyDamage represents the increased ratio by float, not percentage
    //for example if one caseAll doubles damage, parValue should be 1f not 100f nor 2f
    public void mulitplyDamage(float parValue) {
        //magic damage can't decrease
        //multiplier can't be below zero
        if ((damageType == enumDamageType.magic) && (parValue < 1)) {
            return;
        }

        totalMultiply += parValue;
        if (totalMultiply < 0f) { totalMultiply = 0f; }
    }

    //fixedDamage has the top priority, the only one that can change the fixed damage is fixing damage later
    public void fixDamage(int parValue) {
        fixedDamage = parValue;
    }

    public void calculateFinalDamage() {
        //fix damage precedes all
        if (fixedDamage != -1) {
            damage_ = fixedDamage;
        } else {
            //add first, multiply next
            damage_ = (int)(Mathf.Round((damage_ + totalAdd) * totalMultiply));
        }
    }

    public void ATTACK(Thing target) {
        calculateFinalDamage();
        target.setCurHp(-damage_, sourceCaseAll.owner, true);
        gameManager.GM.TC.addDelegate(() => delEffect(target), 0.5f);
    }
}
