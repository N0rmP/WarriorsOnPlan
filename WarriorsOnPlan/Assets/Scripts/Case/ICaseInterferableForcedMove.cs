using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseInterferableForcedMove {
    // ICaseInterferableForcedMove can interfere
    public bool caseFunc(Thing source, node destination);
}
