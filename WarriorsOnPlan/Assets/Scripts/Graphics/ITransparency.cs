using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITransparency {
    public void init();
    public void fadeIn(float parTimer = 1f, float parDestination = 1f);
    public void fadeOut(float parTimer = 1f, float parDestination = 0f);
    public void fadeStrict(float parValue);
}
