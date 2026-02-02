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

    public skillFocussShot() : base("Image/Case/Effect/Image_effectFocussing") {
        code = 92003;
    }

    // ★ caseFocussing tutorial, 집중 구현할 때마다 여기 오세용
    protected override void actualUseSkill(Thing source, Thing target) {
        combatManager.CM.executeProcess(
            new processByproductAddCase(
                source,
                gameManager.GM.MC.makeCodableObject<effectFocussing>(4100, new int[2] { timerFocussing, timerFocussing }, new List<object>() {
                    (Action)(() => {
                        combatManager.CM.executeProcess(new processByproductDealDamage(new damageInfo[1]{ new damageInfo(source, this, damage) }, source.whatToUseSkill));
                    }),
                    //  ★ animationTracker 쓰도록 변경
                    (Action)(() => {
                        source.thisOrganAnimation.clearAttackAnimation();
                        source.thisOrganAnimation.addAttackAnimation(enumAttackAnimation.trigAttackCast);
                        source.thisOrganAnimation.animateAttack();
                        gameManager.GM.TC.addDelegate(
                            () => {
                                combatManager.CM.FC.callVFX(
                                    enumVFX.projectile_simple,
                                    combatManager.CM.FC.getRetrieverMoveStop(),
                                    source.transform.position,
                                    source.whatToUseSkill.transform.position,
                                    enumMoveType.linear,
                                    Color.blue,
                                    0.5f
                                );
                                gameManager.GM.AC.playSE(gameManager.GM.AHouC.arrClipMagicBasic.selectRandom());
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

        source.thisOrganAnimation.animateFocuss();
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
