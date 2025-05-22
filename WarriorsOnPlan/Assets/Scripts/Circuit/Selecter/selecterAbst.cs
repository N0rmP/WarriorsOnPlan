using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Circuits {
    public enum enumTargetGroup { 
        none = 0b000,
        friendly = 0b001,
        hostile = 0b010,
        neutral = 0b100
    }

    public abstract class selecterAbst : circuitAbst<selecterAbst> {
        /*
            about targetGroup & selecterAbst instance creation
                targetGroup represents the target groups by binary numbers
                if owner is in neutral side, hostile side is both player's side and enemy side, neutral side is neutral side

                (if digit == 1...)  |parameter of creator (primitive)
                lowest digit        |friendly to circuit owner's side
                middle digit        |hostile to circuit owner's side
                highest digit       |neutral
        */
        private int targetGroup;

        // parParameter[0] = enumSide parSide, parParameter[1] = int parTargetGroup
        public selecterAbst(int[] parParameter) : base(parParameter) {
            targetGroup = parParameter[0];
        }

        // getTargetArray returns an array of targets only referring to targetGroup
        protected IEnumerable<Thing> getTargetArray(enumSide parSide) {
            List<Thing> tempResult = new List<Thing>();

            if (parSide == enumSide.neutral) {
                if ((targetGroup & (int)enumTargetGroup.friendly) != 0) {
                    tempResult.AddRange(combatManager.CM.HouC.arrNeutralAlive);
                }
                if ((targetGroup & (int)enumTargetGroup.hostile) != 0) {
                    tempResult.AddRange(combatManager.CM.HouC.arrPlayerAlive);
                    tempResult.AddRange(combatManager.CM.HouC.arrEnemyAlive);
                }
            } else {
                if ((parSide == enumSide.player && (targetGroup & (int)enumTargetGroup.friendly) != 0) ||
                    (parSide == enumSide.enemy && (targetGroup & (int)enumTargetGroup.hostile) != 0)) {
                    tempResult.AddRange(combatManager.CM.HouC.arrPlayerAlive);
                }
                if ((parSide == enumSide.player && (targetGroup & (int)enumTargetGroup.hostile) != 0) ||
                    (parSide == enumSide.enemy && (targetGroup & (int)enumTargetGroup.friendly) != 0)) {
                    tempResult.AddRange(combatManager.CM.HouC.arrEnemyAlive);
                }
                if ((targetGroup & (int)enumTargetGroup.neutral) != 0) {
                    tempResult.AddRange(combatManager.CM.HouC.arrNeutralAlive);
                }
            }

            return tempResult;
        }

        public override Dictionary<string, int[]> getParameters() {
            Dictionary<string, int[]> tempResult = base.getParameters();
            tempResult["selecterAbst"] = new int[1] { (int)targetGroup };
            return tempResult;
        }

        public override void restoreParameters(Dictionary<string, int[]> parParameters) {
            base.restoreParameters(parParameters);

            if (!parParameters.ContainsKey("selecterAbst")) {
                targetGroup = 000;
                return;
            }

            targetGroup = parParameters["selecterAbst"][0];
        }

        public override void restoreParameters(IEnumerator<int> parParameters) {
            base.restoreParameters(parParameters);

            targetGroup = parParameters.MoveNext() ? parParameters.Current : (int)enumTargetGroup.none;
        }

        public abstract Thing select(Thing source);
    }
}