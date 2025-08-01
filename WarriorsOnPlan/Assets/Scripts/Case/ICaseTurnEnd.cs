using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseTurnEnd {
    // return : extra turn
    public bool caseFunc(Thing source);
}
