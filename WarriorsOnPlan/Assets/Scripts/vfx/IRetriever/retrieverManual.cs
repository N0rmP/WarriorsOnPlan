using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class retrieverManual : retrieverAbst {
    private bool isRetrieve = false;

    public override bool checkRetrieve(vfxMovable parVM) {
        return isRetrieve;
    }

    public override retrieverAbst getRetriever() {
        return new retrieverManual();
    }

    public void retrieve() {
        isRetrieve = true;
    }
}
