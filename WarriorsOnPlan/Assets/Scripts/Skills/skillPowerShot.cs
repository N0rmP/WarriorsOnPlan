using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillPowerShot : skillAbst
{
    private readonly int damageOriginal;

    public skillPowerShot(int parDamage) : base(5) {
        damageOriginal = parDamage;
    }

    public override void useSkill(Thing source, Thing target) {
        
    }
}
