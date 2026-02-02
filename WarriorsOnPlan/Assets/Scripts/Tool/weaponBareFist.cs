using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cases {
    public class weaponBareFist : toolWeapon {
        public weaponBareFist() : base("Image/Case/Tool/Image_weaponBareFist") {
            code = 3001;

            attackAnimation = enumAttackAnimation.trigAttackPunch;
        }
    }
}