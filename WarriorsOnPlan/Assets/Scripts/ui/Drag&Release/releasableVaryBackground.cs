using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// releasableVaryBackground work in main canvas, it will process with all wrong-released dragable objects
public class releasableVaryBackground : releasableObjectAbst {
    public new  void Start() {
        base.Start();
        targetEnumDrag = (int)enumDrag.anything;
    }

    protected override bool doWhenReleased(enumDrag parCurDragging, object[] parParameters) {
        return false;
    }
}
