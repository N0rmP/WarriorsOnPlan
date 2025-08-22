using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ICaseAddedSystemic can't be ignored, it may be used to add weapon-prefab on the thing-prefab when weapon is added
public interface ICaseSystemicAdded {
    public void caseFunc(ICaseContainerContainer source);
}
