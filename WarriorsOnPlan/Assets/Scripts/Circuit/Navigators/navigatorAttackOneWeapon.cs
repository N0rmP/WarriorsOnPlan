using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;

public class navigatorAttackOneWeapon : navigatorAbst
{
    // rangeMinMax represents the total attackable ranges of owner's weapons
    // indice represents each range-scope per two-indice
    private List<(int min, int max)> rangeRange;

    public navigatorAttackOneWeapon(warriorAbst parOwner) : base(parOwner) { }

    public override void onEngage(Thing source) {
        int tempRangeMinCur;
        int tempRangeMaxCur;
        foreach (toolWeapon tw in owner_.copyWeapon) {
            if (rangeRange.Count == 0) {
                rangeRange.Add((tw.rangeMin, tw.rangeMax));
                continue;
            }

            // �� listWeapon�� ��Ÿ� ������ ���� (warriorAbst �������� Added �� �����ص� ��)
            tempRangeMinCur = tw.rangeMin;
            tempRangeMaxCur = tw.rangeMax;
            for (int i = 0; i < rangeRange.Count; i ++) {
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

    public override EDirection getNextEDirection() {
        // �� route.Count == 0 �̸� BFS ����
        //check is route valid
        bool tempIsRouteValid = (route.Count > 0);
        node tempNode = owner_.curPosition;
        Func<node, bool> delGoalCheck =
            ((n) => {
                node tempNode = owner_.whatToAttack.curPosition;
                int tempDistanceForRange = (Mathf.Abs(tempNode.coor0 - n.coor0) > Mathf.Abs(tempNode.coor1 - n.coor1)) ? Mathf.Abs(tempNode.coor0 - n.coor0) : Mathf.Abs(tempNode.coor1 - n.coor1);
                foreach ((int min, int max) iterTup in rangeRange) {
                    if (tempDistanceForRange >= iterTup.min && tempDistanceForRange <= iterTup.max) {
                        return true;
                    }
                }
                return false;
            });

        foreach (EDirection iterEDir in route) {
            tempNode = tempNode.link[(int)iterEDir];
            if (tempNode.thingHere != null) {
                tempIsRouteValid = false;
                break;
            }
        }

        //if route is invalid, recalculate route
        if (!tempIsRouteValid) {
            combatManager.CM.graphCur.BFS(owner_.curPosition, delGoalCheck, ref route);
        }       

        return route.Pop();
    }
}
