using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseInterferableDestroy {
    // ICaseInterferableDestroy can interfere
    public bool caseFunc(Thing source, Thing target);
}
