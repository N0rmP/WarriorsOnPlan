using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaseTimed
{
    //class implenting caseTimed should have timer variable, update it in updateTimer, and call onAlarmed when it goes below zero
    public void timerUpdate(float parDeltaTime);

    public void onAlarmed();
}
