using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class retrieverAbst {
    public virtual void doWhenAdded(vfxMovable parVM) { }
    public abstract bool checkRetrieve(vfxMovable parVM);

    public virtual retrieverAbst getRetriever() {
        return this;
    }
}
