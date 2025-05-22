using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cases;

public enum enumDamageType {
    absolute = 0,
    magic = 1,
    basic = 2
}

public class damageInfo {
    public readonly Thing sourceAttacker;
    public readonly caseBase sourceCase;

    // there are three kinds of damage changers, add / multiply / fix
    // basically add is processed first, and multiply is processed after it
    // fix ignores other changers and fixes damage strictly, fixed damaged can be changed only by another fix, you should be careful when fixing damage
    private int totalAdd;
    private float totalMultiply;
    private int fixedDamage;

    // damageDealt will be set by processByproductDealDamage, damageDealt can't be changed once set
    private int damageDealt_ = -1;
    public int damageDealt {
        get {
            return damageDealt_;
        }
        set {
            if (damageDealt_ >= 0) {
                return;
            }
            damageDealt_ = value;
        }
    }

    private int damage_ = 0;
    public int damage {
        get {
            calculateFinalDamage();
            return damage_;
        }
    }
    public int damageOriginal { get; private set; }
    
    public enumDamageType damageType { get; private set; }
    public enumVFX vfxHit { get; private set; }

    public damageInfo(Thing parSourceAttacker, caseBase parSourceCase, int parDamage, enumDamageType parDType = enumDamageType.basic, enumVFX parEnumHit = enumVFX.hit_simple) {
        sourceAttacker = parSourceAttacker;
        sourceCase = parSourceCase;
        damage_ = 0;
        damageOriginal = parDamage;
        damageType = parDType;
        vfxHit = parEnumHit;
        totalAdd = 0;
        totalMultiply = 1.0f;

        // absolute damage can't be changed at all
        fixedDamage = (damageType == enumDamageType.absolute) ? damageOriginal : -1;        
     }

    public void addDamage(int parValue) {
        //magic damage can't decrease
        if ((damageType == enumDamageType.magic) && (parValue < 0)) {
            return;
        }

        totalAdd += parValue;
    }

    // parameter of multiplyDamage represents the increased ratio by float, not percentage
    // for example if one case doubles damage, parValue should be 1f not 100f nor 2f
    public void mulitplyDamage(float parValue) {
        //magic damage can't decrease
        if ((damageType == enumDamageType.magic) && (parValue < 1)){
            return;
        }

        totalMultiply += parValue;
    }

    // if parameter of multiplyDamage is int, it represents the increase/decrease gap as percentage
    public void mulitplyDamage(int parValue) {
        //magic damage can't decrease
        if ((damageType == enumDamageType.magic) && (parValue < 0)) {
            return;
        }

        totalMultiply = (float)Math.Round(totalMultiply + (parValue / 100f), 2);
    }

    //fixedDamage has the top priority, the only one that can change the fixed damage is fixing damage later
    public void fixDamage(int parValue) {
        fixedDamage = parValue;
    }

    public void calculateFinalDamage() {
        //fixing damage precedes all
        if (fixedDamage != -1) {
            damage_ = fixedDamage;
        } else {
            //add first, multiply next
            damage_ = (int)(Mathf.Round((damageOriginal + totalAdd) * totalMultiply));
            //damage can't be below zero
            if (damage_ < 0) {
                damage_ = 0;
            }
        }
    }

    public void SHOW(Vector3 parTargetPosition) {
        // ★ 실제로 감소된 체력량 툭 튀어나오게 하기, 이거 processByproductDealDamage 혹은 processByproductDecreaseHp로 옮기죠?
        combatManager.CM.FC.callVFX(vfxHit, combatManager.CM.FC.getRetrieverParticleStop(), parTargetPosition);
    }
}
