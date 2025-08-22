using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Cases {
    public class upgradeTestBowSupply : upgradeAbst {
        // parToolParameter is int array passed to create weaponTester
        private List<int> parToolParameter;

        public upgradeTestBowSupply() : base("Image/Case/Tool/Image_weaponTester") {
            code = 95001;
            parToolParameter = new List<int>();
        }

        public override void actualActivate() {
            combatManager.CM.systemAddToolsProvided(
                gameManager.GM.MC.makeCodableObject<caseBase>(93001, parToolParameter, null)
            );
        }

        public override void restoreParameters(Dictionary<string, int[]> parParameters) {
            base.restoreParameters(parParameters);

            parToolParameter.Clear();
            parToolParameter.AddRange(parParameters["concrete"]);            
        }

        public override void restoreParameters(IEnumerator<int> parParameters) {
            base.restoreParameters(parParameters);

            while (parParameters.MoveNext()) {
                parToolParameter.Add(parParameters.Current);
            }
        }

        protected override void ClonePrepare() {
            base.ClonePrepare();

            parToolParameter = new List<int>();
        }
    }
}