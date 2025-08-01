using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cases {
    public abstract class skillAbst : caseTimerSelfishTurn {
        // fields in region creator_customable can be changed in each creator of concrete skill class
        #region creator_customable
        // isRangeNeeded should be true even if skill targets only nearby things, it's false only when range is literally not used at all
        public bool isRangeNeeded { get; protected set; } = true;
        public bool isTargetNeeded { get; protected set; } = true;
        // targetGroupDefault is preferred target group of each skillAbst
        public int targetGroupDefault { get; protected set; } = 0b0010;
        #endregion creator_customable


        private int rangeMin_ = 1;
        private int rangeMax_ = 1;
        public int rangeMin {
            get {
                return rangeMin_;
            }
            protected set {
                rangeMin_ = Math.Min(1, value);
            }
        }
        public int rangeMax {
            get {
                return rangeMax_;
            }
            protected set {
                rangeMax_ = Math.Max(rangeMin_, value);
            }
        }

        public virtual bool isReady {
            get {
                return timerCur <= 0;
            }
        }

        public skillAbst(string parImagePath) : base(parImagePath, enumCaseType.skill, parIsVisible: true) { }

        public override Dictionary<string, int[]> getParameters() {
            Dictionary<string, int[]> tempResult = base.getParameters();
            tempResult["skillAbst"] = isRangeNeeded ? new int[2] { rangeMin, rangeMax } : new int[0];
            return tempResult;
        }

        public override void restoreParameters(IEnumerator<int> parParameters) {
            base.restoreParameters(parParameters);
            
            if (isRangeNeeded) {
                rangeMin = parParameters.MoveNext() ? parParameters.Current : 1;
                rangeMax = parParameters.MoveNext() ? parParameters.Current : 1;
            } else {
                // you should write dummy range data in skill list of level json file, so if this skill doesn't need it then skip it
                parParameters.MoveNext();
                parParameters.MoveNext();
            }
        }

        public override void restoreParameters(Dictionary<string, int[]> parParameters) {
            base.restoreParameters(parParameters);

            if (isRangeNeeded) {
                rangeMin = parParameters["skillAbst"][0];
                rangeMax = parParameters["skillAbst"][1];
            }
        }

        protected override void updateTimer(Thing source) {
            base.updateTimer(source);
            if (isTimerNeeded && combatManager.CM.combatState == enumCombatState.reenact) {
                source.updatePanelSkillTimer();
            }
        }

        protected override void doOnAlarmed(Thing source) {
            source.observeVoid<ICaseSkillReady>(new object[1] { source });
        }

        public void useSkill(Thing source, Thing target = null) {
            actualUseSkill(source, target);
            if (isTimerNeeded) {
                resetTimer();
                source.updatePanelSkillTimer();
            }
        }

        protected abstract void actualUseSkill(Thing source, Thing target);

        public virtual void SHOW(Thing source, Thing target) { }
    }
}
