using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// releasableVaryBackground work in main canvas, it will process with all wrong-released dragable objects
public class releasableVaryBackground : releasableObjectAbst {
    protected override bool doWhenReleased(object[] parParameters) {
        return false;
    }
}
