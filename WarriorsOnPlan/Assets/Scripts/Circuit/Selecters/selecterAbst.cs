using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class selecterAbst
{
    // targetGroup represents the target groups by binary numbers
    // each digit represents (from lower digit) players's warriorAbst list / enemy's warriorAbst list / neutral Thing list
    int targetGroup;

    public selecterAbst(System.Object[] parArray) {
        targetGroup = parArray[0].ConvertTo<int>();
    }

    protected Thing[] getTargetArray() {
        combatManager tempCM = combatManager.CM;
        List<Thing> tempResult = new List<Thing>();


        if ((targetGroup | 0b001) != 0) {
            tempResult.AddRange(tempCM.copyWarriorsActionOrder(0));
        }
        if ((targetGroup | 0b010) != 0) {
            tempResult.AddRange(tempCM.copyWarriorsActionOrder(1));
        }
        if ((targetGroup | 0b100) != 0) {
            tempResult.AddRange(tempCM.copyNeutralActionOrder);
        }

        return tempResult.ToArray();
    }

    public abstract Thing select(Thing source);
}
