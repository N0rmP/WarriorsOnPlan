using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneTemplate;
using UnityEngine;

namespace Circuits {
    public class selecterClosest : selecterAbst {

        public selecterClosest(int[] parParameter) : base(parParameter) {
            code = 1301;
        }

        public override Thing select(Thing source) {
            float minDistance = float.MaxValue;
            float tempDistance;
            node ownerPosition = source.curPosition;
            Thing targetCur = null;

            foreach (Thing th in getTargetArray(source.thisSide)) {
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