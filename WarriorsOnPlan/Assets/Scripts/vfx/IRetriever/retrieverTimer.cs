using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class retrieverTimer : retrieverAbst {
    private float timer;

    public override bool checkRetrieve(vfxMovable parVM) {
        timer -= Time.deltaTime;
        return timer <= 0f;
    }

    public override retrieverAbst getRetriever() {
        return new retrieverTimer();
    }

    public void setTimer(float parTimer) {
        timer = parTimer;
    }
}
