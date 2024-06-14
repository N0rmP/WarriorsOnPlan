using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class skillAbst : caseTimer {

    public int rangeMin { get; protected set;}
    public int rangeMax { get; protected set; }
    

    public skillAbst(int parTimerMax, bool parIsTimerMax = false) : base(parTimerMax, enumCaseType.skill, parIsTimerMax) { }

    

    public abstract void useSkill(Thing source, Thing target);
}
