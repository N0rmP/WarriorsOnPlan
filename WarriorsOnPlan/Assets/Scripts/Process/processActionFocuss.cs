using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Processes {
    public class processActionFocuss : processActionAbst {

        public processActionFocuss(Thing parSource, bool parIsSHOW = true) : base(parIsSHOW) {
            source = parSource;
        }
        protected override void actualDO() {
            base.actualDO();
        }
    }
}