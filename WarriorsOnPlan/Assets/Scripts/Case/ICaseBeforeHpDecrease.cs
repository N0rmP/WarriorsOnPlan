using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseBeforeHpDecrease
{
    public void onBeforeHpDecrease(Thing source, ref int value);
}
