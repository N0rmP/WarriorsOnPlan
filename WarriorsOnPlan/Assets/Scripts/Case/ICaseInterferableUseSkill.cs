using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseInterferableUseSkill
{
    // source of onUseSkill is owner, target is the target of the skill
    // ICaseInterferableUseSkill can interfere
    public bool caseFunc(Thing source, Thing target = null);
}
