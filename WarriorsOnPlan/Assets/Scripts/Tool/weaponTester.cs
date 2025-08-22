using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cases {
    public class weaponTester : toolWeapon {
        public weaponTester() : base("Image/Case/Tool/Image_weaponTester") {
            code = 93001;
            attackAnimation = enumAttackAnimation.trigAttackBow;
        }

        public override void showEffect(Thing source, Thing parTarget) {
            combatManager.CM.FC.showBasicProjectile(source.transform.position, parTarget.transform.position);
            gameManager.GM.AC.playSE(
                SwissArmyStaticMethod.selectRandom<AudioClip>(
                    gameManager.GM.AHouC.arrClipSwing
                )
            );
        }        
    }
}