using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class skillAbst : caseTimerSelfishTurn {

    public bool isRanged { get; protected set; }

    public skillAbst(int parTimerMax, bool parIsTimerMax = false) : base(parTimerMax, enumCaseType.skill, parIsTimerMax) { }

    protected override void doOnAlarmed(Thing source) {
        foreach (ICaseSkillReady cb in source.getCaseList<ICaseSkillReady>()) {
            cb.onSkillReady(source);
        }
    }

    public void useSkill(Thing source, Thing target) {
        actualUseSkill(source, target);
        반환 형식으로 데미지 / 회복량 등을 자유롭게 반환할 수 있는 형태 구상하기
    }

    protected abstract void actualUseSkill(Thing sourcem, Thing target);
}
