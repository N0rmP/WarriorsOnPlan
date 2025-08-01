using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseInterferableDealDamage {
    // ICaseInterferableDealDamage can interfere... I think it might be ok with just setting Dinfo.damage zero but anyway...
    public bool caseFunc(Thing source, Thing target, damageInfo DInfo);
}
