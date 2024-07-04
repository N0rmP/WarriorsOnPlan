using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEditor.SceneTemplate;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class navigatorAttackOneWeapon : navigatorAbst
{
    // rangeMinMax represents the total attackable ranges of owner's weapons
    // indice represents each range-scope per two-indice
    private List<(int min, int max)> rangeRange = new List<(int min, int max)>();
    private node prevTargetPos = null;

    public navigatorAttackOneWeapon(object[] parArray) : base(parArray) { }

    // iterate owner.listWeapon, make the coverage list of weapons
    private void updateRangeRange(Thing owner) {
        int tempRangeMinCur;
        int tempRangeMaxCur;

        rangeRange.Clear();

        foreach (toolWeapon tw in owner.copyWeapons) {
            if (rangeRange.Count == 0) {
                rangeRange.Add((tw.rangeMin, tw.rangeMax));
                continue;
            }

            // ★ listWeapon을 사거리 순으로 정렬 (warriorAbst 단위에서 Added 시 정렬해둘 것)
            tempRangeMinCur = tw.rangeMin;
            tempRangeMaxCur = tw.rangeMax;
            for (int i = 0; i < rangeRange.Count; i++) {
                if ((tempRangeMinCur <= rangeRange[i].max) && (tempRangeMaxCur > rangeRange[i].max)) {  // two range have some folded area
                    rangeRange[i] = (rangeRange[i].min, tempRangeMaxCur);
                    break;
                } else if (tempRangeMinCur > rangeRange[i].max) {   // two range have no folded area
                    rangeRange.Add((tempRangeMinCur, tempRangeMaxCur));
                    break;
                }
                // unless two IF state above didn't work, cur range is completely folded on the previous one and ignored
            }
        }

        //if owner has no weapon, just make him move close to the target
        if (rangeRange.Count == 0) {
            rangeRange.Add((0, 1));
        }
    }


    #region override
    public override bool checkIsArrival(Thing owner) {
        updateRangeRange(owner);
        int tempDistance = node.getDistance(owner.curPosition, owner.whatToAttack.curPosition);
        foreach ((int min, int max) tup in rangeRange) {
            if ((tempDistance < tup.min) || (tempDistance > tup.max)) { continue; }
            return true;
        }
        return false;
    }

    public override node getNextRoute(Thing owner) {
        node tempTargetPos = owner.whatToAttack.curPosition;
        // ★ route.Count == 0 이면 BFS 실행
        //check is route valid
        bool tempIsRouteValid = (route.Count > 0);
        foreach (node nd in route) {
            if (nd.thingHere != null) {
                tempIsRouteValid = false;
                break;
            }
        }
        if (tempTargetPos != prevTargetPos) {
            tempIsRouteValid = false;
        }

        //if route is invalid, recalculate route
        if (!tempIsRouteValid) {
            int tempDistanceForRange;

            Stack<EDirection> tempStack = new Stack<EDirection>();
            updateRangeRange(owner);
            Func<node, bool> delGoalCheck =
            ((n) => {
                tempDistanceForRange = node.getDistance(tempTargetPos, n);
                foreach ((int min, int max) iterTup in rangeRange) {
                    if (tempDistanceForRange >= iterTup.min && tempDistanceForRange <= iterTup.max) {
                        return true;
                    }
                }
                return false;
            });

            combatManager.CM.GC.BFS(owner.curPosition, delGoalCheck, ref tempStack);

            polishEDirStackToRouteQueue(owner.curPosition, ref tempStack, ref route);
            prevTargetPos = tempTargetPos;
        }


        return route.Dequeue();
    }
    #endregion override
}
