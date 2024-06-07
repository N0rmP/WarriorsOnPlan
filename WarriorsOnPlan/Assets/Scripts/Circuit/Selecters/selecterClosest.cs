using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneTemplate;
using UnityEngine;

public class selecterClosest : selecterAbst {

    public override Thing select(Thing source) {
        //★ 제 3자 Thing을 타겟으로도 할 수 있게 변경할 것
        List<Thing> tempPotentialTargetList =
            (source is warriorAbst tempSource) ?
            (combatManager.CM.warriorsHpSorted[(tempSource.isPlrSide ? 1 : 0)].ConvertAll(
                new System.Converter<warriorAbst, Thing>((x) => (Thing)x)
                )) :
            null
        ;


        int minDistance = int.MaxValue;
        int tempDistance;
        node ownerPosition = source.curPosition;
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
