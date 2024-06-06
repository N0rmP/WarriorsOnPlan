using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseBeforeUseSkill
{
    //source of onUseSkill is owner, target is the target of the skill
    public void onBeforeUseSkill(Thing source, Thing target = null);
}
