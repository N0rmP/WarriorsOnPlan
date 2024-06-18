using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class skillAbst : caseTimerSelfishTurn {

    public bool isRanged { get; protected set; }

    public skillAbst(int parTimerMax, bool parIsTimerMax = true) : base(parTimerMax, enumCaseType.skill, parIsTimerMax) { }

    public abstract void useSkill(Thing source, Thing target);
}
