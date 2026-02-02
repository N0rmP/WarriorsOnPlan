using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Differentiaters {
    public class differentiaterNormalBasic : differentiaterBase {
        private GameObject buttonSetCircuit;

        public differentiaterNormalBasic() {
            buttonSetCircuit = GameObject.Find("buttonSetCircuit");
        }

        protected override void actualInit() {
            buttonSetCircuit.SetActive(false);
        }

        public override void restoreWhenLeave() {
            base.restoreWhenLeave();

            buttonSetCircuit.SetActive(true);
        }
    }
}