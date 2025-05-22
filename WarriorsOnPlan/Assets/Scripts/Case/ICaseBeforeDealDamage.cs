using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseBeforeDealDamage {
    public void onBeforeDealDamage(Thing source, Thing target, damageInfo DInfo);
}
