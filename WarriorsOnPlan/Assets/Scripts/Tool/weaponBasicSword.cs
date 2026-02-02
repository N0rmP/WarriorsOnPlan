using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cases {
    public class weaponBasicSword : toolWeapon {
        public weaponBasicSword() : base("Image/Case/Tool/Image_BasicSword") {
            code = 3002;
            attackAnimation = enumAttackAnimation.trigAttackBrandish;
            thisObjPath = "Prefab/Weapon/weaponBasicSword";
        }
    }
}