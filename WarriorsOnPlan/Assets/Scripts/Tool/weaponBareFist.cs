using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cases {
    public class weaponBareFist : toolWeapon {
        public weaponBareFist() : base("Image/Case/Tool/Image_weaponBareFist") {
            code = 3001;

            attackAnimation = enumAttackAnimation.trigAttackPunch;
        }

        public override void showEffect(Thing source, Thing parTarget) {
            gameManager.GM.AC.playSE(
                SwissArmyStaticMethod.selectRandom<AudioClip>(
                    gameManager.GM.AHouC.arrClipPunch
                )
            );
        }
    }
}