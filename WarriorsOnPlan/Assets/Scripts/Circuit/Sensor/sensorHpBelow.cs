using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Circuits {
    public class sensorHpBelow : sensorAbst {
        private int threshold;

        public sensorHpBelow(int[] parParameter) : base(parParameter) {
            code = 1103;
        }

        public override bool checkWigwagging(Thing source) {
            return source.curHp <= threshold;
        }

        public override Dictionary<string, int[]> getParameters() {
            Dictionary<string, int[]> tempResult = base.getParameters();
            tempResult["concrete"] = new int[1] { threshold };
            return tempResult;
        }

        public override void restoreParameters(Dictionary<string, int[]> parParameters) {
            base.restoreParameters(parParameters);

            if (!parParameters.ContainsKey("concrete")) {
                threshold = 999;
            }

            threshold = parParameters["concrete"][0];
        }

        public override void restoreParameters(IEnumerator<int> parParameters) {
            base.restoreParameters(parParameters);

            threshold = parParameters.MoveNext() ? parParameters.Current : 999;
        }
    }
}