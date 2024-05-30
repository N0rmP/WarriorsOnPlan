using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.SceneTemplate;
using UnityEngine;

public class navigatorAttackOneWeapon : navigatorAbst
{
    // rangeMinMax represents the total attackable ranges of owner's weapons
    // indice represents each range-scope per two-indice
    private List<(int min, int max)> rangeRange;

    public navigatorAttackOneWeapon() : base() {
        rangeRange = new List<(int min, int max)>();
    }

    // iterate owner.listWeapon, make the coverage list of weapons
    private void updateRangeRange() {
        int tempRangeMinCur;
        int tempRangeMaxCur;

        rangeRange.Clear();

        foreach (toolWeapon tw in owner.copyWeapon) {
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
    }


    #region override
    protected override bool checkIsArrival() {
        updateRangeRange();
        int tempDistance = node.getDistance(owner.curPosition, owner.whatToAttack.curPosition);
        foreach ((int min, int max) tup in rangeRange) {
            if ((tempDistance < tup.min) || (tempDistance > tup.max)) { continue; }
            return true;
        }
        return false;
    }

    public override EDirection getNextEDirection() {
        // ★ route.Count == 0 이면 BFS 실행
        //check is route valid
        bool tempIsRouteValid = (route.Count > 0);
        node tempPos = owner.curPosition;
        foreach (EDirection iterEDir in route) {
            tempPos = tempPos.link[(int)iterEDir];
            if (tempPos.thingHere != null) {
                tempIsRouteValid = false;
                break;
            }
        }

        //if route is invalid, recalculate route
        if (!tempIsRouteValid) {
            int tempDistanceForRange;
            node tempTargetPos = owner.whatToAttack.curPosition;
            updateRangeRange();
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
            combatManager.CM.GC.BFS(owner.curPosition, delGoalCheck, ref route);
        }

        return route.Pop();
    }
    #endregion override
}
