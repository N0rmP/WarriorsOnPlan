using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseInterferableAttack {
    // source of onAttack is owner, target is the to-be-attacked warrior
    // ICaseInterferableAttack can interfere
    public bool caseFunc(Thing source, Thing target);
}
