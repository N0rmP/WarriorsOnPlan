using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseUpdateState
{
    public (ICaseUpdateState updater, enumStateWarrior ESW) onUpdateState(Thing source);

    public virtual void onIntefered(Thing source) { }
}
