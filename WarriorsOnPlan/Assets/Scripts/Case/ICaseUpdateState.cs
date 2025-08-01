using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseUpdateState {
    // public (ICaseUpdateState updater, enumStateWarrior ESW) caseFunc(Thing source);
    public (ICaseUpdateState ,enumStateWarrior) caseFunc(Thing source);

    public void onInterfered(Thing source);
}
