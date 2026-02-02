using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Processes {
    public abstract class processAbst {
        /*
            thisCountAction represents the number of actions passed, thisCountDistinguisher represents the boundary to stop reenacting temporarily
            the timings when thisCountAction & thisCountDistinguisher changes are usually same, but exception is when turn starts & combat ends
        */
        public int thisCountAction { get; private set; }
        public int thisCountDistinguisher { get; private set; }

        private bool isSHOW = true;
        protected bool isInterfered = false;

        public processAbst processPrev { get; private set; }
        public processAbst processNext { get; private set; }

        // processAbst.Do pass delegate that sets processNext by ref argument to combatManager
        // this capsulates set-next-method, and ensures the only one next process can use it
        protected Action<processAbst> delSetNext;

        public processAbst(bool parIsSHOW = true) {
            isSHOW = parIsSHOW;
            delSetNext = (x) => { processNext = x; };
        }

        // DO makes the process do its job during the actual combat, it won't be used while replaying at all
        // calling DO saves the combat in the chained processAbst, it will be used for replaying after the combat
        public void DO(ref processAbst parPrev, ref Action<processAbst> parDelSetNext) {
            doBeforeActualDo();

            // set processPrev of this class after doBeforeActualDo & before actualDo, because onBefore~ methods can create and execute new processes
            processPrev = parPrev;
            if (parDelSetNext != null) {
                parDelSetNext(this);
            }

            thisCountAction = combatManager.CM.countAction;
            // skip all left code if combat ended
            if (parPrev is processSystemCombatEnd) {
                return;
            }
            // set processLast & delSetNext of combatManager before actualDo, new process made during actualDo is latter in the process-chain
            parPrev = this;
            parDelSetNext = delSetNext;

            if (isInterfered) {
                return;
            }

            actualDO();

            doAfterActualDo();
        }

        // REENACT reenacts chained processes before next action, the next action will be returned and used for next REENACT
        public processAbst REENACT() {
            // interfered process ignores actualDo but do SHOW
            if (!isInterfered) {
                actualDO();
            }

            if (combatManager.CM.combatSpeed > 0) {
                SHOW();
            }

            if (processNext == null || thisCountDistinguisher != processNext.thisCountDistinguisher) {
                return processNext;
            } else {
                return processNext.REENACT();
            }
        }

        /*
        // UNDO undoes chained processes until current process is action, the previous process will be returned and used for next UNDO
        public processAbst UNDO() {
            //actualUNDO();

            if (processPrev == null || thisCountAction != processPrev.thisCountAction) {
                return processPrev;
            } else {
                return processPrev.UNDO();
            }
        }
        */

        public void SHOW() {
            if (isSHOW && combatManager.CM.combatSpeed < 4) {
                actualSHOW();
            }
            combatManager.CM.CUM.CStatus.updateTotal();
        }        

        protected virtual void doBeforeActualDo() {
            if (this is processSystemTurnStart or processSystemCombatEnd or processAction) {
                combatManager.CM.incrementCountDistinguisher();
            }
            thisCountDistinguisher = combatManager.CM.countDistinguisher;
        }
        protected virtual void doAfterActualDo() { }
        protected abstract void actualDO();
        protected virtual void actualSHOW() { }

        #region utility
        public bool hasNext() {
            return processNext != null;
        }

        public bool hasPrev() {
            return processPrev != null;
        }

        /*
            이거 아무리 봐도 병합 가능하겠는데...
            만약 caseBase가 processByproductDealDamage를 만들어 데미지를 줬는데, 병합됐으면 damageInfo로부터 데미지 최종값을 받아 띄우겠지?
            damageInfo 없이 체력만 잃는 경우는? 아마 processByproductHpDecrease의 valueFinal을 확인해야 할 거고
            그럼 사실상 체력을 잃는 행위에 대해 2가지 경로의 애니메이션 표시 방법이 생기는 건데 이게 맞아?
            그런데 일단 체력을 잃는 행위는 caseBase에 귀속되어야 하잖아. (게임 내의 거의 모든 처리는 결과적으로 caseBase로부터 시작하므로)
            그리고 체력을 잃는 애니메이션은 결과적으로 caseBase가 어떤 무지개똥 발광 애니메이션이든 끝마치면, 거기에 연동되어서 나오는 거고
            그럼 caseBase가 처리해야 하나?
            
            생각해보니까 이거 이런 식으로 무한 리팩토링하면 영원히 출시 못 한다.
            필수적인 거 아니면 최대한 넘기자
        public virtual bool checkMergable(processAbst obj) {
            return false;
        }

        public virtual void mergeProcess(processAbst parProcess) { }
        */
        #endregion utility

        #region test
        public void testChainNear() {
            Debug.Log("testChain : " + processPrev + " - " + this + " - " + processNext);
        }

        public virtual void testChainAfterAll(int parOrder = 0, StringBuilder parSB = null) {
            if (parOrder == 0) {
                parSB = new StringBuilder();
            }

            parSB.Append("\n");
            parSB.Append(this.GetType().Name.PadRight(31, '_'));
            parSB.Append(" : ");
            testAnythingSay(parSB);
            processNext?.testChainAfterAll(parOrder + 1, parSB);

            if (parOrder == 0) {
                Debug.Log(parSB.ToString());
            }
        }

        protected virtual void testAnythingSay(StringBuilder parSB) { }
        #endregion test
    }
}