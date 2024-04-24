using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseUpdateState
{
    public (ICaseUpdateState updater, enumStateWarrior ESW) onUpdateState(Thing source);

    public void onIntefered(Thing source);
}
