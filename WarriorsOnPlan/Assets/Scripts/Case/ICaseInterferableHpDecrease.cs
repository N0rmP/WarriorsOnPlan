using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseInterferableHpDecrease {
    // ICaseInterferableHpDecrease can interfere
    public bool caseFunc(Thing source, ref int value);
}
