using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuits {
    public class sensorNothing : sensorAbst {
        public sensorNothing(int[] parParameter) : base(parParameter) {
            code = 1101;
        }

        public override bool checkWigwagging(Thing source) {
            return false;
        }
    }
}