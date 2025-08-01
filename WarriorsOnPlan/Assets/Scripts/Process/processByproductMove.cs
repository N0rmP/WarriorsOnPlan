using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;

namespace Processes {
    // be aware not to confuse this with processByproductActionMove
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

            departure = source.curPosition;

            if (!isWillingly) {
                // onInterferableAttack
                isInterfered = source.observeInterferable<ICaseInterferableForcedMove>(new object[2] { source, destination });
                if (isInterfered) {
                    return;
                }

                // onBeforeForcedMove
                source.observeVoid<ICaseBeforeForcedMove>(new object[2] { source, destination });
            } 
        }

        protected override void doAfterActualDo() {
            base.doAfterActualDo();

            // onAfterForcedMove
            if (!isWillingly) {
                source.observeVoid<ICaseAfterForcedMove>(new object[1] { source });
            }
        }

        protected override void actualDO() {
            source.curPosition.sendThing(destination);
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            if (isInterfered) {
                gameManager.GM.PC.popupBasicAlert(
                    source.transform.position, 
                    isWillingly ? gameManager.GM.DHouC.bookWords.strMove : gameManager.GM.DHouC.bookWords.strForcedMove + 
                    " " + 
                    gameManager.GM.DHouC.bookWords.strInterfere, 
                false);
            }

            Vector3 tempDestinationVector = source.curPosition.getVector3();

            if (isWillingly) {
                source.transform.rotation = Quaternion.LookRotation(tempDestinationVector - source.transform.position);
                source.animateMove();
            }

            source.moveLinear(tempDestinationVector);
        }
    }
}