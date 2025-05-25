using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

using Processes;

namespace Cases {
    public abstract class caseTimer : caseBase {

        protected bool isAutoReset = false;
        protected bool isRemovedOnAlarmed = false;

        // timerMax isn't restricted to be above 0, it can be used to show some skills don't need timer but programmer should be aware of this
        public int timerMax { get; protected set; }
        public int timerCur { get; protected set; }

        public caseTimer(int[] parArrParameter, enumCaseType parEnumCaseType = enumCaseType.effect, bool parIsVisible = false) : base(parArrParameter, parEnumCaseType, parIsVisible) { }

        protected virtual void updateTimer(Thing source) {
            if (timerCur <= 0) {
                doOnAlarmed(source);

                if (isAutoReset) {
                    resetTimer();
                }

                if (isRemovedOnAlarmed) {
                    combatManager.CM.executeProcess(new processByproductRemoveCase(source, this));
                }
            } else {
                timerCur--;
            }
        }

        public void resetTimer() {
            timerCur = timerMax;
        }

        protected virtual void doOnAlarmed(Thing source) { }

        public override void restore(mementoIParametable parMementoCase) {
            base.restore(parMementoCase);
        }

        public override Dictionary<string, int[]> getParameters() {
            Dictionary<string, int[]> tempResult = base.getParameters();

            tempResult["caseTimer"] = new int[2] { timerMax, timerCur };
            return tempResult;
        }

        public override void restoreParameters(Dictionary<string, int[]> parParameters) {
            base.restoreParameters(parParameters);
            timerMax = parParameters["caseTimer"][0];
            timerCur = parParameters["caseTimer"][1];
        }

        public override void restoreParameters(IEnumerator<int> parParameters) {
            base.restoreParameters(parParameters);

            timerMax = parParameters.MoveNext() ? parParameters.Current : 0;
            timerCur = parParameters.MoveNext() ? parParameters.Current : 0;
        }
    }
}