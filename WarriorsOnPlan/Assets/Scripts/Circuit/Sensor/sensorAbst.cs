using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuits {
    public abstract class sensorAbst : circuitAbst<sensorAbst> {
        protected int timerCur;
        protected int timerMax;

        public sensorAbst(int[] parParameter) : base(parParameter) { }

        public void updateTimer() {
            if (timerCur > 0) {
                timerCur--;
            }
        }

        public abstract bool checkWigwagging(Thing source);

        public virtual bool checkKeepBasic(Thing source) {
            return true;
        }

        public override Dictionary<string, int[]> getParameters() {
            Dictionary<string, int[]> tempResult = base.getParameters();
            tempResult["sensorAbst"] = new int[2] { timerMax, timerCur };
            return tempResult;
        }

        public override void restoreParameters(IEnumerator<int> parParameters) {
            base.restoreParameters(parParameters);
            timerMax = parParameters.MoveNext() ? parParameters.Current : 0;
            timerCur = parParameters.MoveNext() ? parParameters.Current : 0;
        }
    }
}