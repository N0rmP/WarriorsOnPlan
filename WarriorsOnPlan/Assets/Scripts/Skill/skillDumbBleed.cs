using Circuits;
using Processes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cases {
    // skillDumbBleed add bleeding to the source at the start of combat, this effect is for test when developing effect system

    public class skillDumbBleed : skillAbst, ICaseEngage {
        private int effectTimerMax = 0;
        private int damage = 0;

        public override bool isReady {
            get {
                return false;
            }
        }

        public skillDumbBleed(int[] parSkillParameters) : base(parSkillParameters) {
            code = 92002;
            isRangeNeeded = false;
            isCoolTimeNeeded = false;
        }

        public override Dictionary<string, int[]> getParameters() {
            Dictionary<string, int[]> tempResult = base.getParameters();

            tempResult["concrete"] = new int[2] { effectTimerMax, damage };

            return tempResult;
        }

        public override void restoreParameters(Dictionary<string, int[]> parParameters) {
            base.restoreParameters(parParameters);

            effectTimerMax = parParameters["concrete"][0];
            damage = parParameters["concrete"][1];
        }

        public override void restoreParameters(IEnumerator<int> parParameter) {
            base.restoreParameters(parParameter);

            effectTimerMax = parParameter.MoveNext() ? parParameter.Current : 1;
            damage = parParameter.MoveNext() ? parParameter.Current : 1;
        }

        protected override void actualUseSkill(Thing source, Thing target) {
            combatManager.CM.executeProcess(new processByproductAddCase(source,
                gameManager.GM.MC.makeCodableObject<caseBase>(94001, new int[3] { 3, 3, 1 })
            ));
        }

        public void onEngage(Thing source) {
            actualUseSkill(source, null);
        }
    }
}