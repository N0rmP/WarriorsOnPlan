using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Processes {
    public class processActionMove : processActionAbst {
        private node departure;
        private node destination;

        public processActionMove(Thing parSource, node parDestination, bool parIsSHOW = true) : base(parIsSHOW) {
            source = parSource;
            departure = source.curPosition;
            destination = parDestination;
        }

        protected override void doBeforeActualDo() {
            base.doBeforeActualDo();
            // �� onBeforeMove�� �ʿ��ϴٸ� �����Ͽ� ����
        }

        protected override void doAfterActualDo() {
            base.doAfterActualDo();
            // �� onAfterMove�� �ʿ��ϴٸ� �����Ͽ� ����
        }

        protected override void actualDO() {
            base.actualDO();

            combatManager.CM.executeProcess(
                new processByproductMove(
                    source, destination
                    )
                );
        }

        /*
        protected override void actualUNDO() {
            source.curPosition.sendThing(departure);
            // �� ���� GameObject position ����
        }
        */

        protected override void actualSHOW() {
            base.actualSHOW();
        }
    }
}