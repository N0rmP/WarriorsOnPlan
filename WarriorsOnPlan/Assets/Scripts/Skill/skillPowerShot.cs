using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Processes;
using Circuits;

namespace Cases {
    public class skillPowerShot : skillAbst {
        private int damage;

        private GameObject objBow = null;

        #region InfoImplementation
        public override object[] getDescriptionArgument() {
            return new object[1] { damage };
        }
        #endregion InfoImplementation

        public skillPowerShot() : base("Image/Case/Skill/Image_skillPowerShot") {
            code = 2003;
        }

        public override Dictionary<string, int[]> getParameters() {
            Dictionary<string, int[]> tempResult = base.getParameters();

            tempResult["concrete"] = new int[1] { damage };

            return tempResult;
        }

        public override void restoreParameters(Dictionary<string, int[]> parParameters) {
            base.restoreParameters(parParameters);

            damage = parParameters["concrete"][0];
        }

        public override void restoreParameters(IEnumerator<int> parParameter) {
            base.restoreParameters(parParameter);

            damage = parParameter.MoveNext() ? parParameter.Current : 1;
        }

        protected override void actualUseSkill(Thing source, Thing target) {
            combatManager.CM.executeProcess(
                new processByproductDealDamage(
                    new damageInfo[1] { new damageInfo(source, this, damage) },
                    target
                    )
                );
        }

        public override void SHOW(Thing source, Thing target) {
            source.Look(target.transform.position);

            source.thisOrganAnimation.addAttackAnimation(enumAttackAnimation.trigAttackPunch);
            source.thisOrganAnimation.animateAttack(false);

            // animationTracker 쓰도록 변경
            gameManager.GM.TC.addDelegate(
                () => {
                    combatManager.CM.FC.callVFX(
                        enumVFX.projectile_simple,
                        combatManager.CM.FC.getRetrieverMoveStop(),
                        source.transform.position,
                        target.transform.position,
                        enumMoveType.linear,
                        Color.gray,
                        0.5f
                    );
                    gameManager.GM.AC.playSE(gameManager.GM.AHouC.arrClipSwing.selectRandom());
                },
                combatManager.CM.getBodyAnimationDuration() / 2f
            );            
        }
    }
}