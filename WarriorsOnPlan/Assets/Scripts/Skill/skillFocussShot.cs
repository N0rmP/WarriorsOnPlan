using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cases;
using Processes;

public class skillFocussShot : skillAbst {
    private int timerFocussing;
    private int damage;

    #region InfoImplementation
    public override object[] getDescriptionArgument() {
        return new object[2] { timerFocussing, damage };
    }
    #endregion InfoImplementation

    public skillFocussShot() : base("Image/Case/Effect/image_caseFocussing") {
        code = 92003;
    }

    protected override void actualUseSkill(Thing source, Thing target) {
        combatManager.CM.executeProcess(
            new processByproductAddCase(
                source,
                gameManager.GM.MC.makeCodableObject<caseFocussing>(4100, new int[2] { timerFocussing, timerFocussing }, new List<object>() {
                    (Action)(() => {
                        combatManager.CM.executeProcess(new processByproductDealDamage(new damageInfo[1]{ new damageInfo(source, this, damage) }, target));
                    }),
                    (Action)(() => {
                        source.resetAnimator();
                        source.clearAttackAnimation();
                        source.addAttackAnimation(enumAttackAnimation.trigAttackCast);
                        source.animateAttack();
                        gameManager.GM.TC.addDelegate(
                            () => {
                                combatManager.CM.FC.callVFX(
                                    enumVFX.projectile_simple,
                                    combatManager.CM.FC.getRetrieverMoveStop(),
                                    source.transform.position,
                                    target.transform.position,
                                    enumMoveType.linear,
                                    Color.blue,
                                    0.5f
                                );
                            },
                            combatManager.CM.getBodyAnimationDuration()
                        );
                    })
                })
            )
        );
    }

    public override void SHOW(Thing source, Thing target) {
        base.SHOW(source, target);

        source.animateFocuss();
    }

    #region IParametable
    public override Dictionary<string, int[]> getParameters() {
        Dictionary<string, int[]> tempResult = base.getParameters();
        tempResult["concrete"] = new int[2] { timerFocussing, damage };
        return tempResult;
    }
    public override void restoreParameters(Dictionary<string, int[]> parParameters) {
        base.restoreParameters(parParameters);

        timerFocussing = parParameters["concrete"][0];
        damage = parParameters["concrete"][1];
    }

    public override void restoreParameters(IEnumerator<int> parParameters) {
        base.restoreParameters(parParameters);

        timerFocussing = parParameters.MoveNext() ? parParameters.Current : 1;
        damage = parParameters.MoveNext() ? parParameters.Current : 1;
    }
    #endregion IParametable
}
