using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Processes {
    public class processActionSkip : processActionAbst {
        public processActionSkip(bool parIsSHOW = true) : base(parIsSHOW) { }

        protected override void actualDO() { }
    }
}