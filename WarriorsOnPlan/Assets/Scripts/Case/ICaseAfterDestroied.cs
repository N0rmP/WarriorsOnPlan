using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseAfterDestroied {
    public void caseFunc(Thing dead, Thing destroyer);
}
