using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseBeforeHpDecrease {
    public void caseFunc(Thing source, ref int value);
}
