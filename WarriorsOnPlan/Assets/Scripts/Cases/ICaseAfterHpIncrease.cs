using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseAfterHpIncrease
{
    public void onAfterHpIncrease(Thing source, int value);
}
