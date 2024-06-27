using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageInfo
{
    public readonly Thing sourceAttacker;
    public readonly caseBase sourceCaseAll;

    //there are three kinds of damage changers, add / multiply / fix
    //basically add is processed first, and multiply is processed after it
    //fix ignores all other changers and fix damage strictly, you should be careful when fixing damage
    private int totalAdd;
    private float totalMultiply;
    private int fixedDamage;

    private Action<Thing, Thing> delEffect;

    public int damage { get; private set; }
    public int dealtDamage { get; private set; }
    public enumDamageType damageType { get; set; }

    public damageInfo(Thing parSourceAttacker, caseBase parSourceCaseAll, int parDamage, enumDamageType parDType, Action<Thing, Thing> parDelegate) {
        sourceAttacker = parSourceAttacker;
        sourceCaseAll = parSourceCaseAll;
        damage = parDamage;
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
    }

    //parameter of multiplyDamage represents the increased ratio by float, not percentage
    //for example if one caseAll doubles damage, parValue should be 1f not 100f nor 2f
    public void mulitplyDamage(float parValue) {
        //magic damage can't decrease
        if ((damageType == enumDamageType.magic) && (parValue < 1)){
            return;
        }

        totalMultiply += (parValue - 1f);
    }

    //fixedDamage has the top priority, the only one that can change the fixed damage is fixing damage later
    public void fixDamage(int parValue) {
        fixedDamage = parValue;
    }

    public void calculateFinalDamage() {
        //fix damage precedes all
        if (fixedDamage != -1) {
            damage = fixedDamage;
        } else {
            //add first, multiply next
            damage = (int)(Mathf.Round((damage + totalAdd) * totalMultiply));
            //damage can't be below zero
            if (damage < 0) {
                damage = 0;
            }
        }
    }

    public int DEAL(Thing target) {
        calculateFinalDamage();
        dealtDamage = target.setCurHp(-damage, sourceAttacker);
        gameManager.GM.TC.addDelegate(() => delEffect(sourceAttacker, target), 0.5f);
        return damage;
    }
}
