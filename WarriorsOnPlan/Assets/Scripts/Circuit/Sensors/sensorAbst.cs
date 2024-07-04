using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class sensorAbst : caseBase {
    public sensorAbst(object[] parArray) : base(enumCaseType.circuit) { }
    public abstract bool checkWigwagging(Thing source);
}
