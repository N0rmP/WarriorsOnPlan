using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseThisWeaponUsed {
    public void onThisWeaponUsed(Thing source, Thing target, damageInfo Dinfo);
}
