using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseEngage
{
    //onEngage includes onCombatStart / onEngageDuringCombat
    public void onEngage(Thing source);
}
