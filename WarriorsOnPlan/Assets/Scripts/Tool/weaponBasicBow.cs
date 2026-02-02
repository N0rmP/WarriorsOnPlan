using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cases {
    public class weaponBasicBow : toolWeapon {
        public weaponBasicBow() : base("Image/Case/Tool/image_BasicSword") {
            code = 3003;
            attackAnimation = enumAttackAnimation.trigAttackBow;
            thisObjPath = "Prefab/Weapon/weaponBasicBow";
        }

        public override void showEffect(Thing source, Thing parTarget, int parCountFolded) {
            base.showEffect(source, parTarget, parCountFolded);
            (cashStateHash csh, float time) tempTup = animationTracker.dictEaaTrackerInformation[attackAnimation];
            이거 왜 작동을 안 해; ;
            source.thisAnimationTracker.Enqueue(tempTup.csh, (tempTup.time + intervalFolded * parCountFolded,
                () => combatManager.CM.FC.showBasicProjectile(source.transform.position, parTarget.transform.position)
            ));
        }
    }
}