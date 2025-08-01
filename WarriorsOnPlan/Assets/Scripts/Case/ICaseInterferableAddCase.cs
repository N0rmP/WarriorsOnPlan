using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cases;

public interface ICaseInterferableAddCase {
    // ICaseInterferableAddCase can interfere
    public bool caseFunc(Thing source, caseBase caseAdded);
}
