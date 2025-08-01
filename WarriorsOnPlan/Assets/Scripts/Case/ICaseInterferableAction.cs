using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseInterferableAction {
    // ICaseInterferableAction can interfere
    public bool caseFunc(Thing source);
}
