using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuits {
    public class selecterClosest : selecterAbst {

        public selecterClosest() {
            code = 1301;
        }

        protected override Thing actualSelect(Thing source, List<Thing> parTargetList) {
            float minDistance = float.MaxValue;
            float tempDistance;
            node ownerPosition = source.curPosition;
            Thing targetCur = null;

            foreach (Thing th in parTargetList) {
                tempDistance = node.getTechnicalDistance(ownerPosition, th.curPosition);
                if (minDistance > tempDistance) {
                    minDistance = tempDistance;
                    targetCur = th;
                }
            }

            return targetCur;
        }
    }
}