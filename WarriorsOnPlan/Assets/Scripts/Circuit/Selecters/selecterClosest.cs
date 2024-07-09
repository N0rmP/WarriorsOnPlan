using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneTemplate;
using UnityEngine;

public class selecterClosest : selecterAbst {

    public selecterClosest(int parTargetGroup) : base(parTargetGroup) { }

    public override Thing select(Thing source) {
        Thing[] tempPotentialTargetList = getTargetArray();

        int minDistance = int.MaxValue;
        int tempDistance;
        node ownerPosition = source.curPosition;
        Thing targetCur = null;

        foreach (Thing th in tempPotentialTargetList) {
            tempDistance = node.getDistance(ownerPosition, th.curPosition);
            if (minDistance > tempDistance) {
                minDistance = tempDistance;
                targetCur = th;
            }
        }

        return targetCur;
    }
}
