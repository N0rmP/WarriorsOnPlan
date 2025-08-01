using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuits {
    public abstract class sensorAbst : circuitAbst {
        public sensorAbst() { }

        public abstract bool checkWigwagging(Thing source);
    }
}