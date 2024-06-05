using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseEngage
{
    //onEngage includes onCombatStart / onEngageDuringCombat
    public virtual void onEngage(Thing source);
}
