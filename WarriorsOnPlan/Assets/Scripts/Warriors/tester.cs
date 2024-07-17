using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester : Thing
{
    protected override void initPersonal(int[] parSkillParameters) {
        this.addCase(new skillPowerShot(parSkillParameters));
    }
}