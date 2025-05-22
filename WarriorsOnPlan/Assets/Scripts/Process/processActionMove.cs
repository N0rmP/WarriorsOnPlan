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
            // ★ onBeforeMove가 필요하다면 구현하여 실행
        }

        protected override void doAfterActualDo() {
            base.doAfterActualDo();
            // ★ onAfterMove가 필요하다면 구현하여 실행
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
            // ★ 실제 GameObject position 갱신
        }
        */

        protected override void actualSHOW() {
            base.actualSHOW();
        }
    }
}