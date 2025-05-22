using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cases;

namespace Processes {
    public abstract class processActionAbst : processAbst {
        protected Thing source;

        public processActionAbst(bool parIsSHOW = true) : base(parIsSHOW) { }

        protected override void doBeforeActualDo() {
            combatManager.CM.incrementCountAction();

            base.doBeforeActualDo();

            // timer update
            

            // onBeforeAction
            foreach (ICaseBeforeAction cb in source.getCaseList<ICaseBeforeAction>()) {
                cb.onBeforeAction(source);
            }
        }
        protected override void doAfterActualDo() {
            base.doAfterActualDo();

            // onAfterAction
            foreach (ICaseAfterAction cb in source.getCaseList<ICaseAfterAction>()) {
                cb.onAfterAction(source);
            }

            // timer Update
            foreach (caseTimerSelfishTurn ct in source.getCaseList<caseTimerSelfishTurn>()) {
                ct.updateOnActionEnd(source);
            }
        }

        protected override void actualDO() {
            foreach (caseTimerSelfishTurn ct in source.getCaseList<caseTimerSelfishTurn>()) {
                ct.updateOnActionStart(source);
            }
        }

        protected override void actualSHOW() {
            base.actualSHOW();
            combatUIManager.CUM.setActionCounter(thisCountAction);
            // �̰� 2�� ���� ����, �̰� �ϰ� ���� Thing state ���ŵǴ��� Ȯ�� ��
        }
    }
}