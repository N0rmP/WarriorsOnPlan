using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseInterferableDestroied {
    // ICaseInterferableDestroied can interfere
    public bool caseFunc(Thing source, Thing destroyer);
}
