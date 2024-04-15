using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;

public class selecterClosest : selecterAbst {
    public override Thing select(bool isPlrSide) {
        List<warriorAbst> tempPotentialTargetList = combatManager.CM.warriorsHpSorted[(isPlrSide ? 1 : 0)];
        int minDistance = int.MaxValue;
        int tempDistance;
        node ownerPosition = owner.curPosition;
        node tempNode;
        warriorAbst targetCur = null;

        foreach (warriorAbst wa in tempPotentialTargetList) {
            tempNode = wa.curPosition;
            tempDistance = Mathf.Abs(ownerPosition.coor0 - tempNode.coor0) > Mathf.Abs(ownerPosition.coor1 - tempNode.coor1) ? Mathf.Abs(ownerPosition.coor0 - tempNode.coor0) : Mathf.Abs(ownerPosition.coor1 - tempNode.coor1);
            if (minDistance > tempDistance) {
                minDistance = tempDistance;
                targetCur = wa;
            }
        }

        return targetCur;
    }
}
