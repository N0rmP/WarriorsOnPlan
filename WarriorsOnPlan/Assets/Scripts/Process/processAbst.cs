using System;
using System.Collections;
using System.Collections.Generic;
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
        }

        public bool hasNext() {
            return processNext != null;
        }

        public bool hasPrev() {
            return processPrev != null;
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

        #region test
        public void testChainNear() {
            Debug.Log("testChain : " + processPrev + " - " + this + " - " + processNext);
        }

        public string testChainAfterAll(int parOrder = 0) {
            if (parOrder == 0) {
                Debug.Log(
                    (this is processAction or processSystemTurnEnd ? "\n" : "") + this + " - " + processNext?.testChainAfterAll(parOrder + 1)
                    );
                return "test message is already logged dumbass";
            } else {
                return (this is processAction or processSystemTurnEnd ? "\n" : "") + this + " - " + processNext?.testChainAfterAll(parOrder + 1);
            }
        }
        #endregion test
    }
}