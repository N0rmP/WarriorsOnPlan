using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cases {
    public class weaponTester : toolWeapon {
        public weaponTester() : base("Image/Case/Tool/Image_weaponTester") {
            code = 93001;
            attackAnimation = enumAttackAnimation.trigAttackBow;
            thisObjPath = "Prefab/Weapon/weaponBasicBow";
        }

        public override void showEffect(Thing source, Thing parTarget, int parCountFolded) {
            base.showEffect(source, parTarget, parCountFolded);
            (cashStateHash csh, float time) tempTup = animationTracker.dictEaaTrackerInformation[attackAnimation];
            source.thisAnimationTracker.Enqueue(tempTup.csh, (tempTup.time + intervalFolded * parCountFolded,
                () => combatManager.CM.FC.showBasicProjectile(source.transform.position, parTarget.transform.position)
            ));
        }
    }
}