using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cases;

namespace Processes {
    public class processByproductActionFocussEnd : processByproductActionAbst {
        private caseFocussing thisCaseFocussing;

        public processByproductActionFocussEnd(Thing parSource, caseFocussing parCaseFocussing, bool parIsSHOW = true) : base(parSource, parIsSHOW) {
            thisCaseFocussing = parCaseFocussing;    
        }

        protected override void actualDO() {
            base.actualDO();

            thisCaseFocussing.delActivate();
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            thisCaseFocussing.delShow();
        }
    }
}