using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Processes {
    public class processByproductActionSkip : processByproductActionAbst {
        public processByproductActionSkip(Thing parSource, bool parIsSHOW = true) : base(parSource, parIsSHOW) { }

        protected override void actualDO() { }

        protected override void actualSHOW() {
            base.actualSHOW();

            gameManager.GM.PC.popupBasicAlert(source.gameObject.getCanvasMainLocalPosition(), gameManager.GM.DHouC.bookWords.strConrolled);

            source.animateControlled();
        }
    }
}