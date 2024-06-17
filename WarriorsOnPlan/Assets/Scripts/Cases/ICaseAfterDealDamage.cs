using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseAfterDealDamage {
    public void onAfterDealDamage(Thing source, Thing Target, damageInfo DInfo);
}
