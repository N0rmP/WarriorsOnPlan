using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wigwaggerSkillSelfReady : wigwaggerSkillabst, ICaseSkillReady, ICaseAfterUseSkill {
    private bool isUseSkill;

    public wigwaggerSkillSelfReady(System.Object[] parArray) : base((int)parArray[0]) {

    }

    public void onAfterUseSkill(Thing source, Thing target = null) {
        isUseSkill = false;
    }

    public void onSkillReady(Thing source) {
        resetTimer();
    }

    protected override void doOnAlarmed(Thing source) {
        isUseSkill = true;
    }

    protected override bool checkIfUseSkill(Thing source) {
        return isUseSkill;
    }
}
