using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class selecterAbst
{
    public warriorAbst owner { get; private set; }

    public selecterAbst(warriorAbst parOwner) {
        owner = parOwner;
    }

    public abstract Thing select(bool parIsPlrSide);
}
