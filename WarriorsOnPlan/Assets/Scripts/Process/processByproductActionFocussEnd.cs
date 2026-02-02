using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cases;

namespace Processes {
    public class processByproductActionFocussEnd : processByproductActionAbst {
        private effectFocussing thisEffectFocussing;

        public processByproductActionFocussEnd(Thing parSource, effectFocussing parCaseFocussing, bool parIsSHOW = true) : base(parSource, parIsSHOW) {
            thisEffectFocussing = parCaseFocussing;    
        }

        protected override void actualDO() {
            base.actualDO();

            thisEffectFocussing.delActualDo();
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            thisEffectFocussing.delShow();
        }
    }
}