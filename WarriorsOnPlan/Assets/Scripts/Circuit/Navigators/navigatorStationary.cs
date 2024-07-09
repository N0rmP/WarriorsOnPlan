using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class navigatorStationary : navigatorAbst
{

    public override node getNextRoute(Thing owner) {
        return null;
    }

    public override bool checkIsArrival(Thing owner) {
        return true;
    }
}
