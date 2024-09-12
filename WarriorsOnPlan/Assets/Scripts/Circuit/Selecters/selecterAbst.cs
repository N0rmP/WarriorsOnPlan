using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class selecterAbst : circuitAbst<selecterAbst> {
    // targetGroup represents the target groups by binary numbers
    // each digit represents (from lower digit) players's warriorAbst list / enemy's warriorAbst list / neutral Thing list
    int targetGroup;

    // parTargetGroup doesn't correspond with targetGroup, its digit represents (from lower digit) friendly / enemy / neutral
    public selecterAbst(Thing source, int parTargetGroup) {
        // selecterAbst can be created by uiCircuitSetterPanel to call singleInfo, in that case source can't be parametered
        if (source == null) {
            targetGroup = 0b000;
            return;
        }

        targetGroup =
            ((parTargetGroup & 0b001) != 0 ? (int)Math.Pow(2, (double)source.thisSide) : 0b000) |
            ((parTargetGroup & 0b010) != 0 ? (source.thisSide switch {
                enumSide.player     => 0b010,
                enumSide.enemy      => 0b001,
                enumSide.neutral    => 0b011,
                _ => 0b000
            }) : 0b000) |
            ((parTargetGroup & 0b100) != 0 ? 0b100 : 0b000);
    }

    protected Thing[] getTargetArray() {
        combatManager tempCM = combatManager.CM;
        List<Thing> tempResult = new List<Thing>();


        if ((targetGroup & 0b001) != 0) {
            tempResult.AddRange(tempCM.copyWarriorsActionOrder(0));
        }
        if ((targetGroup & 0b010) != 0) {
            tempResult.AddRange(tempCM.copyWarriorsActionOrder(1));
        }
        if ((targetGroup & 0b100) != 0) {
            tempResult.AddRange(tempCM.copyWarriorsActionOrder(2));
        }

        return tempResult.ToArray();
    }

    public abstract Thing select(Thing source);
}
