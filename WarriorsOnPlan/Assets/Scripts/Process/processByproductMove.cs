using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

namespace Processes {
    public class processByproductMove : processByproductAbst {
        private Thing source;
        private node departure;
        private node destination;
        private bool isWillingly;
        private float fltMoveTimer;

        public processByproductMove(Thing parSource, node parDestination, bool parIsWillingly = true, float parFltMoveTimer = 1f, bool parIsSHOW = true) : base(parIsSHOW) {
            source = parSource;
            isWillingly = parIsWillingly;
            destination = parDestination;
            fltMoveTimer = parFltMoveTimer;
        }

        protected override void doBeforeActualDo() {
            base.doBeforeActualDo();
            if (!isWillingly) {
                foreach (ICaseBeforeForcedMove cb in source.getCaseList<ICaseBeforeForcedMove>()) {
                    cb.onBeforeForcedMove(source, destination);
                }
            }
        }

        protected override void doAfterActualDo() {
            base.doAfterActualDo();
            if (!isWillingly) {
                foreach (ICaseAfterForcedMove cb in source.getCaseList<ICaseAfterForcedMove>()) {
                    cb.onAfterForcedMove(source);
                }
            }
        }

        protected override void actualDO() {
            departure = source.curPosition;
            source.curPosition.sendThing(destination);
        }

        /*
        protected override void actualUNDO() {
            source.curPosition.sendThing(departure, true);
        }
        */

        protected override void actualSHOW() {
            Vector3 tempDestinationVector = source.curPosition.getVector3();

            if (isWillingly) {
                source.transform.rotation = Quaternion.LookRotation(tempDestinationVector - source.transform.position);
                source.animateMove();
            }

            source.moveLinear(tempDestinationVector);
        }
    }
}