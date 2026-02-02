using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Processes {
    // main function of focussing is done by caseFocussing, this process is only for shouting 'this Thing is in focussing!'
    public class processByproductActionFocuss : processByproductActionAbst {

        public processByproductActionFocuss(Thing parSource, bool parIsSHOW = true) : base(parSource, parIsSHOW) { }

        protected override void actualDO() {
            base.actualDO();
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            gameManager.GM.PC.popupBasicAlert(source.gameObject.getCanvasMainLocalPosition() + new Vector2(0f, gameManager.GM.option.stickDegreed), gameManager.GM.DHouC.bookWords.strFocussing + "...");

            source.thisOrganAnimation.animateFocuss();
        }
    }
}