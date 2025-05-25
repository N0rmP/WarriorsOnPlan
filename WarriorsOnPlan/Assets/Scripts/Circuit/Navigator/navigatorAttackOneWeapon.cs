using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

using Cases;
using System.Text;

namespace Circuits {
    public class navigatorAttackOneWeapon : navigatorAbst {
        // rangeMinMax represents the total attackable ranges of owner's weapons
        // indice represents each range-scope per two-indice
        private List<(int min, int max)> listRange = new List<(int min, int max)>();
        private node prevTargetPos = null;

        public navigatorAttackOneWeapon(int[] parParameter) : base(parParameter) {
            code = 1202;
        }

        // iterate owner.listWeapon, make the coverage list of weapons
        private void updateListRange(Thing owner) {
            int tempRangeMin;
            int tempRangeMax;

            listRange.Clear();

            foreach (toolWeapon tw in owner.copyWeapons) {
                if (listRange.Count == 0) {
                    listRange.Add((tw.rangeMin, tw.rangeMax));
                    continue;
                }

                // ★ listWeapon을 사거리 순으로 정렬 (warriorAbst 단위에서 Added 시 정렬해둘 것)
                tempRangeMin = tw.rangeMin;
                tempRangeMax = tw.rangeMax;
                for (int i = 0; i < listRange.Count; i++) {
                    if ((tempRangeMin >= listRange[i].min && tempRangeMin <= listRange[i].max) ||
                        (tempRangeMax >= listRange[i].min && tempRangeMax <= listRange[i].max)) {  // two range have some folded area
                        listRange[i] = (Math.Min(listRange[i].min, tempRangeMin), Math.Max(listRange[i].max, tempRangeMax));
                        break;
                    } else {   // two range have no folded area
                        listRange.Add((tempRangeMin, tempRangeMax));
                        break;
                    }
                }
            }

            // all warriors should have weaponBareKnuckle, this statement just prevent malfunction
            if (listRange.Count == 0) {
                Debug.Log(owner + " seems to have no weapons (this log sent by navigatorAttackOneWeapon)");
                listRange.Add((0, 1));
            }
        }


        #region override
        public override bool checkIsArrival(Thing owner) {
            updateListRange(owner);

            int tempDistance = node.getDistance(owner.curPosition, owner.whatToAttack.curPosition);
            foreach ((int min, int max) tup in listRange) {
                if ((tempDistance < tup.min) || (tempDistance > tup.max)) { continue; }
                return true;
            }
            return false;
        }

        public override bool checkIsRouteValid(Thing owner) {
            return base.checkIsRouteValid(owner) && (owner.whatToAttack.curPosition != prevTargetPos);
        }

        public override void calculateNewRoute(Thing owner) {
            node tempTargetPos = owner.whatToAttack.curPosition;

            int tempDistanceForRange;

            Stack<EDirection> tempStack = new Stack<EDirection>();
            /*
            updateListRange will be called once per a turn in checkIsArrival
            focus not to add caseBase that destroies toolWeapon while its owner is acting and make code below work again if you decide to add it

            updateListRange(owner);
            */
            Func<node, bool> delGoalCheck =
            ((n) => {
                tempDistanceForRange = node.getDistance(tempTargetPos, n);
                foreach ((int min, int max) iterTup in listRange) {
                    if (tempDistanceForRange >= iterTup.min && tempDistanceForRange <= iterTup.max) {
                        return true;
                    }
                }
                return false;
            });

            combatManager.CM.GC.BFS(owner.curPosition, delGoalCheck, tempStack,
                new Vector2(tempTargetPos.coor0 - owner.curPosition.coor0, tempTargetPos.coor1 - owner.curPosition.coor1)
            );

            polishEDirStackToRouteQueue(owner.curPosition, tempStack, route);
            prevTargetPos = tempTargetPos;
        }

        #endregion override

        #region test
        public void testListRange() {
            StringBuilder tempSB = new StringBuilder("- - - - - navigatorAttackOneWeapon - - - - -\n");
            foreach ((int min, int max) tup in listRange) {
                tempSB.Append(tup.min);
                tempSB.Append(" ~ ");
                tempSB.Append(tup.max);
                tempSB.Append("\n");
            }

            Debug.Log(tempSB.ToString());
        }
        #endregion test
    }
}