using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class navigatorStationary : navigatorAbst
{
    public override EDirection getNextEDirection() {
        return EDirection.none;
    }

    protected override bool checkIsArrival() {
        return true;
    }
}
