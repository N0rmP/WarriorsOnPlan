using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class retrieverParticleStop : retrieverAbst {
    private ParticleSystem PS;

    public override bool checkRetrieve(vfxMovable parVM) {
        return PS.isStopped;
    }

    public override void doWhenAdded(vfxMovable parVM) {
        PS = parVM.GetComponent<ParticleSystem>();
    }
}
