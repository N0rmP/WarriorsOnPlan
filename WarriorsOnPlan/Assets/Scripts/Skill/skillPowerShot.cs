using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Processes;
using Circuits;

namespace Cases {
    public class skillPowerShot : skillAbst {
        private int damage;

        public skillPowerShot(int[] parSkillParameters) : base(parSkillParameters) {
            code = 92001;
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
            Debug.Log("!!!!!!!!! POW!!!!!!!!!!!!! WER!!!!!!!! SHOOOOOOOOOOOOOOOTTTTTTT!!!!!!!!!!");
            combatManager.CM.executeProcess(
                new processByproductDealDamage(
                    new damageInfo[1] { new damageInfo(source, this, damage) },
                    target
                    )
                );
        }

        public override void SHOW(Thing source, Thing target) {
            source.Look(target.transform.position);

            source.clearAttackAnimation();
            source.addAttackAnimation(enumAttackAnimation.trigAttackCast);
            source.animateAttack(false);

            gameManager.GM.TC.addDelegate(
                () => combatManager.CM.FC.callVFX(
                    enumVFX.projectile_simple, 
                    combatManager.CM.FC.getRetrieverMoveStop(),
                    source.transform.position,
                    target.transform.position,
                    enumMoveType.linear,                    
                    Color.red,
                    0.5f),
                combatManager.CM.getBodyAnimationDuration() / 2f
            );
            
        }
    }
}