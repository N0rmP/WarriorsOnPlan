using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseAfterDamaged
{
    //source of onDamaged is the attacker warrior, target is owner
    public void onAfterDamaged(Thing source, Thing target, damageInfo Dinfo);
}
