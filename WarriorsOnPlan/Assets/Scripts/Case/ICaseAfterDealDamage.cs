using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseAfterDealDamage {
    public void caseFunc(Thing source, Thing Target, damageInfo DInfo);
}
