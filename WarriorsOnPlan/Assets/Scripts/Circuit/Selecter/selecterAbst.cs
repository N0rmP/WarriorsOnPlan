using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Circuits {
    public enum enumTargetGroup { 
        none = 0b0000,
        friendly = 0b0001,
        hostile = 0b0010,
        neutral = 0b0100,
        self = 0b1000   // 말미잘 해삼 개불 제철맞은제주은갈치조림, enumTargetGroup.self is added lately
    }

    public abstract class selecterAbst : circuitAbst {
        /*
            about targetGroup & selecterAbst instance creation
                targetGroup represents the target groups by binary numbers
                if owner is in neutral side, hostile side is both player's side and enemy side, neutral side is neutral side

                (if digit == 1...)  |parameter of creator (primitive)
                lowest digit        |friendly to circuit owner's side
                middle digit        |hostile to circuit owner's side
                highest digit       |neutral
        */
        public int targetGroup { get; private set; }

        // parParameter[0] = enumSide parSide, parParameter[1] = int parTargetGroup
        public selecterAbst() {

        }

        // getTargetArray returns an array of targets only referring to targetGroup
        protected List<Thing> getTargetList(Thing source) {
            HashSet<Thing> tempResult = new HashSet<Thing>();
            enumSide tempSide = source.thisSide;
            if ((targetGroup & (int)enumTargetGroup.friendly) != 0) {
                tempResult.AddRange(combatManager.CM.HouC.getArrAlive(tempSide));
            }
            if ((targetGroup & (int)enumTargetGroup.hostile) != 0) {
                switch (tempSide){
                    case enumSide.player:
                        tempResult.AddRange(combatManager.CM.HouC.getArrAlive(enumSide.enemy));
                        break;
                    case enumSide.enemy:
                        tempResult.AddRange(combatManager.CM.HouC.getArrAlive(enumSide.player));
                        break;
                    case enumSide.neutral:
                        // grrr i hate all
                        tempResult.AddRange(combatManager.CM.HouC.getArrAlive(enumSide.player));
                        tempResult.AddRange(combatManager.CM.HouC.getArrAlive(enumSide.enemy));
                        break;
                    default:
                        break;
                }
            }
            if ((targetGroup & (int)enumTargetGroup.neutral) != 0) {
                tempResult.AddRange(combatManager.CM.HouC.getArrAlive(enumSide.neutral));
            }
            if ((targetGroup & (int)enumTargetGroup.self) != 0) {
                tempResult.Remove(source);
            }
            
            return tempResult.ToList<Thing>();
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

        public Thing select(Thing source) {
            List<Thing> tempTargetList = getTargetList(source);
            if (tempTargetList.Count == 0) {
                return null;
            }

            return actualSelect(source, tempTargetList);
        }

        protected abstract Thing actualSelect(Thing source, List<Thing> parTargetList);
    }
}