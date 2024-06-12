using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class sensorAbst : caseBase {
    public sensorAbst() : base(enumCaseType.circuit) { }
    public abstract bool checkWigwagging(Thing source);
}
