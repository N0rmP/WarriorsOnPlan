using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cases {
    public class weaponBareKnuckle : toolWeapon {
        public weaponBareKnuckle(int[] parWeaponParameters) : base(parWeaponParameters) {
            code = 3001;

            attackAnimation = enumAttackAnimation.trigAttackPunch;
        }

        public override void showEffect(Thing source, Thing parTarget) { }
    }
}