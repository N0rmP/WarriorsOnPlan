using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

using Processes;

namespace Cases {
    // ★ 여유있으면 isTimerNeeded, timerMax, timerCur, updateTimer, resetTimer, doOnAlarmed 넣어서 타이머를 인터페이스로 만들 것, 그리고 sensor에 결합시키셈
    public abstract class caseTimer : caseBase {
        // isTimerNeeded can be false when certain category is subclass of caseTimer but one exception doesn't use timer
        public bool isTimerNeeded { get; protected set; } = true;    
        protected bool isAutoReset = false;
        protected bool isRemovedOnAlarmed = false;

        // timerMax isn't restricted to be above 0, it can be used to show some skills don't need timer but programmer should be aware of this
        public int timerMax { get; protected set; }
        private int timerCur_;
        public int timerCur {
            get {
                return timerCur_;
            }
            protected set {
                timerCur_ = Math.Max(value, 0);
            }
        }

        public caseTimer(string parImagePath, enumCaseType parEnumCaseType = enumCaseType.effect, bool parIsVisible = false) : base(parImagePath, parEnumCaseType, parIsVisible) { }

        protected virtual void updateTimer(Thing source) {
            if (!isTimerNeeded) {
                return;
            }
            
            timerCur--;
            if (timerCur <= 0) {
                doOnAlarmed(source);

                if (isAutoReset) {
                    resetTimer();
                }

                if (isRemovedOnAlarmed) {
                    combatManager.CM.executeProcess(new processByproductRemoveCase(source, this));
                }
            }
        }

        public void resetTimer() {
            timerCur = timerMax;
        }

        protected virtual void doOnAlarmed(Thing source) { }

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

            if (isTimerNeeded) {
                timerMax = parParameters.MoveNext() ? parParameters.Current : 0;
                timerCur = parParameters.MoveNext() ? parParameters.Current : 0;
            } else {
                parParameters.MoveNext();
                parParameters.MoveNext();
            }
        }
    }
}