using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class skillAbst : caseTimerSelfishTurn {
    public bool isRangeNeeded { get; protected set; } = true;        // isRangeNeeded should be true even if skill targets only nearby things
    public bool isCoolTimeNeeded { get; protected set; } = true;
    protected int valueOriginal { get; set; }
    public int rangeMin { get; protected set; } = -1;
    public int rangeMax { get; protected set; } = -1;

    public skillAbst(int[] parSkillParameters) : base(
            parSkillParameters[0],
            enumCaseType.skill,
            true,
            parSkillParameters[1] == 1,
            false
        ) {
        initDerived(parSkillParameters);
        // if parameter array represents rangeMin and rangeMax are both -1, it means this skill doesn't need range (give buff to only owner, or forced to be used to certain target .etc)
        if (isRangeNeeded) {
            rangeMin = Math.Max(1, parSkillParameters[2]);   // rangeMin can't be below 1
            rangeMax = Math.Max(rangeMin, parSkillParameters[3]);     // rangeMax can't be below rangeMin
        }        
    }

    protected virtual void initDerived(int[] parSkillParameters) { }

    protected override void updateTimer(Thing source) {
        base.updateTimer(source);
        source.updateSkillTimer(timerCur, timerMax);
    }

    protected override void doOnAlarmed(Thing source) {
        foreach (ICaseSkillReady cb in source.getCaseList<ICaseSkillReady>()) {
            cb.onSkillReady(source);
        }
    }

    public void useSkill(Thing source, Thing target = null) {
        actualUseSkill(source, target);
        resetTimer();
    }

    protected abstract void actualUseSkill(Thing sourcem, Thing target);
}
