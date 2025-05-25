using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Processes;

namespace Cases {
    public class effectTestBleed : caseTimerSelfishTurn, ICaseBeforeAction {
        private int damagePerTurn = 0;

        public effectTestBleed(int[] parParameter) : base(parParameter, parIsVisible: true) {
            code = 94001;
            isRemovedOnAlarmed = true;
        }

        public void onBeforeAction(Thing source) {
            combatManager.CM.executeProcess(new processByproductDealDamage(
                new damageInfo[1] { new damageInfo(null, this, damagePerTurn, enumDamageType.absolute) },
                source,
                parIsShowInstant : true
                ));
        }

        public override Dictionary<string, int[]> getParameters() {
            Dictionary<string, int[]> tempResult = base.getParameters();

            tempResult["concrete"] = new int[1] { damagePerTurn };
            return tempResult;
        }

        public override void restoreParameters(Dictionary<string, int[]> parParameters) {
            base.restoreParameters(parParameters);

            damagePerTurn = parParameters["concrete"][0];
        }

        public override void restoreParameters(IEnumerator<int> parParameters) {
            base.restoreParameters(parParameters);

            damagePerTurn = parParameters.MoveNext() ? parParameters.Current : 0;
        }
    }
}