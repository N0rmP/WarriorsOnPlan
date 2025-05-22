using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cases {
    public class weaponTester : toolWeapon {
        public weaponTester(int[] parArray) : base(parArray) {
            code = 93001;
            attackAnimation = enumAttackAnimation.trigAttackBow;
        }

        public override void showEffect(Thing source, Thing parTarget) {
            showBasicProjectile(source.transform.position, parTarget.transform.position);
        }
    }
}