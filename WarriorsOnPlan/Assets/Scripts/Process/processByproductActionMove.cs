using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Processes {
    public class processByproductActionMove : processByproductActionAbst {
        private node departure;
        private node destination;

        public processByproductActionMove(Thing parSource, node parDestination, bool parIsSHOW = true) : base(parSource, parIsSHOW) {
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
            source.curPosition.sendThere(departure);
            // ★ 실제 GameObject position 갱신
        }
        */

        protected override void actualSHOW() {
            base.actualSHOW();
            source.transform.LookAt(source.transform.position + destination.getVector3() - departure.getVector3());
            source.thisOrganAnimation.animateMove();
        }

        #region test
        protected override void testAnythingSay(StringBuilder parSB) {
            parSB.Append(source?.ToString());
            parSB.Append(" moved from ");
            parSB.Append(departure?.ToString());
            parSB.Append(" to ");
            parSB.Append(destination?.ToString());
            parSB.Append(" by action");
        }
        #endregion test
    }
}