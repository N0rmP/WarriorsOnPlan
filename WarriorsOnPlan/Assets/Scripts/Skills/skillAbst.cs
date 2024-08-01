using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class skillAbst : caseTimerSelfishTurn {

    public bool isRanged { get; protected set; }
    public string skillName { get; protected set; }

    public skillAbst(int[] parSkillParameters) : base(
            parSkillParameters == null ? -1 : parSkillParameters[0]
            , enumCaseType.skill
            , (parSkillParameters != null && parSkillParameters[1] != 0) ? true : false) {
        isRanged = false;
        skillName = this.GetType().Name.Substring(5);
    }

    protected override void updateTimer(Thing source) {
        base.updateTimer(source);
        source.updateSkillTimer(timerCur, timerMax);
    }

    protected override void doOnAlarmed(Thing source) {
        foreach (ICaseSkillReady cb in source.getCaseList<ICaseSkillReady>()) {
            cb.onSkillReady(source);
        }
    }

    public void useSkill(Thing source, Thing target) {
        actualUseSkill(source, target);
        resetTimer();
    }

    protected abstract void actualUseSkill(Thing sourcem, Thing target);
}
