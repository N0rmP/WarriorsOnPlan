using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseInterferableDamaged {
    // source of onDamaged is the attacker warrior, target is owner
    // ICaseInterferableDamaged can interfere
    public bool caseFunc(Thing source, Thing target, damageInfo DInfo);
}
