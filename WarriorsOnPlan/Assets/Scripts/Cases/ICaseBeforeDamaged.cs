using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseBeforeDamaged {
    //source of onDamaged is the attacker warrior, target is owner
    public void onBeforeDamaged(Thing source, Thing target, damageInfo DInfo);
}
