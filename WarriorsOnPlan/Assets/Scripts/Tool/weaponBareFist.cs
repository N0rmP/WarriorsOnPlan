using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cases {
    public class weaponBareFist : toolWeapon {
        public weaponBareFist(int[] parWeaponParameters) : base(parWeaponParameters) {
            code = 3001;

            attackAnimation = enumAttackAnimation.trigAttackPunch;
        }

        public override void showEffect(Thing source, Thing parTarget) { }
    }
}