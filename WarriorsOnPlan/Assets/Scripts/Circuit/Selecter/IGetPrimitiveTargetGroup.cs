using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGetPrimitiveTargetGroup {
    // for more information about primitive target group, please check the statement in selecterAbst
    // getPrimitiveTargetGroup returns value by byte not enumPrimitiveTargetGroup, it ensures passing more than one target groups
    public byte getPrimitiveTargetGroup();
}
