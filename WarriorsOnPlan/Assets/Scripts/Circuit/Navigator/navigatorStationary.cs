using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuits {
    public class navigatorStationary : navigatorAbst {
        public navigatorStationary(int[] parParameter) : base(parParameter) {
            code = 1201;
        }

        public override bool checkIsArrival(Thing owner) {
            return true;
        }

        public override void calculateNewRoute(Thing owner) {
            route.Clear();
        }
    }
}