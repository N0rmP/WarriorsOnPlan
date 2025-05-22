using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseAfterAttack {
    //source of onAttack is owner, target is the attacked warrior
    public void onAfterAttack(Thing source, Thing target, damageInfo[] arrDInfo);
}
