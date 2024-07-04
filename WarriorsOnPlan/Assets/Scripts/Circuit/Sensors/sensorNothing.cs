using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sensorNothing : sensorAbst {
    public sensorNothing(object[] parArray) : base(parArray) { }

    public override bool checkWigwagging(Thing source) {
        return false;
    }
}
