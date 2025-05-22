using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseBeforeHpIncrease
{
    public void onBeforeHpIncrease(Thing source, ref int value);
}
