using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// unlike inside-game cases there is no unified controller for outside-game cases, you should control them manually
public interface ICaseResolutionChange
{
    public void onResolutionChange(int parNewWidth, int parNewHeight);
}
