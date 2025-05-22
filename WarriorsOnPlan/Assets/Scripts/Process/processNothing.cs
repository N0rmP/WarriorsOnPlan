using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Processes {
    public class processAnimate : processAbst {
        Action delActualSHOW = null;

        public processAnimate(Action parDelActualSHOW, bool parIsSHOW = true) : base(parIsSHOW) {
            delActualSHOW = parDelActualSHOW;
        }

        protected override void actualDO() { }

    }
}