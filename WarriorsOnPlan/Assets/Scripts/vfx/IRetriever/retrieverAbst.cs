using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ★ 이거 인터페이스로.. 너 등신이니?
public abstract class retrieverAbst {
    public virtual void doWhenAdded(vfxMovable parVM) { }
    public abstract bool checkRetrieve(vfxMovable parVM);

    public virtual retrieverAbst getRetriever() {
        return this;
    }
}
