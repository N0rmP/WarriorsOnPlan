using System.Collections;
using System.Collections.Generic;
using System.Text;
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
            source.curPosition.sendThere(destination);
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            if (isInterfered) {
                gameManager.GM.PC.popupBasicAlert(
                    source.gameObject.getCanvasMainLocalPosition(), 
                    isWillingly ? gameManager.GM.DHouC.bookWords.strMove : gameManager.GM.DHouC.bookWords.strForcedMove +  " " + gameManager.GM.DHouC.bookWords.strInterfere
                );
            }

            // processByproductMove only moves the source, walking or jumping animation is on processByproductActionMove or caseBase
            Vector3 tempDestinationVector = destination.getVector3();
            source.moveLinear(tempDestinationVector);
        }

        #region test
        protected override void testAnythingSay(StringBuilder parSB) {
            parSB.Append(source?.ToString());
            parSB.Append(" moves from");
            parSB.Append(departure?.ToString());
            parSB.Append(" to ");
            parSB.Append(destination?.ToString());
            parSB.Append(isWillingly ? " willingly" : " forcefully");
        }
        #endregion test
    }
}