using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sensorNothing : sensorAbst {
    public override bool checkWigwagging(Thing source) {
        return false;
    }
}
