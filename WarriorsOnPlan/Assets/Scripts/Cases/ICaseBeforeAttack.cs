using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseBeforeAttack
{
    //source of onAttack is owner, target is the to-be-attacked warrior
    public void onBeforeAttack(Thing source, Thing target, damageInfo DInfo);
}
