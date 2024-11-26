using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester : Thing {
    protected override skillAbst makeSkill(int[] parSkillParameters) {
        return new skillPowerShot(parSkillParameters);
    }
}