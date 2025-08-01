using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Processes {
    public class processByproductActionAbst : processByproductAbst {
        protected Thing source;

        public processByproductActionAbst(Thing parSource, bool parIsShow = true) : base(parIsShow) {
            source = parSource;
        }

        protected override void actualDO() { }
    }
}