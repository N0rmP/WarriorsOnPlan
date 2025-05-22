using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuits {
    public class sensorAnything : sensorAbst {
        public sensorAnything(int[] parParameter) : base(parParameter) {
            code = 1102;
        }

        public override bool checkWigwagging(Thing source) {
            return true;
        }        
    }
}