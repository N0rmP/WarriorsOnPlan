using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selecterAbst
{
    public warriorAbst owner { get; set; }
    public virtual Thing select(bool isPlrSide) { return null; }
}
