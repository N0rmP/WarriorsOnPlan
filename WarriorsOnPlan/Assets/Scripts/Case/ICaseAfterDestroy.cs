using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseAfterDestroy {
    //source of onDestroy is owner, target is the destroied warrior
    public void caseFunc(Thing source, Thing target);
}
