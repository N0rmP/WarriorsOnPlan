using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseAfterUseSkill
{
    //source of onUseSkill is owner, target is the target of the skill
    public void onAfterUseSkill(Thing source, Thing target = null);
}
