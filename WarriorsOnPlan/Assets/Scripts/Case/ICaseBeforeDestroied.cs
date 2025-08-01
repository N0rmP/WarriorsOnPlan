using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseBeforeDestroied {
    public void caseFunc(Thing source, Thing destroyer);
}
