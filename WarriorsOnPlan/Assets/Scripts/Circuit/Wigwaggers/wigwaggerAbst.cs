using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class wigwaggerAbst : caseAll
{
    // wigwagger set owner's navigator according to situation

    // wigwaggerAbst have no change from caseAll
    // wigwaggerAbst only exist for distinguish circuit class from other cases, and expansion

    // wigwagger of 'Always' does'nt exist, just the navigator is set in the warrior
    public wigwaggerAbst() : base(enumCaseType.circuit) { }
}
