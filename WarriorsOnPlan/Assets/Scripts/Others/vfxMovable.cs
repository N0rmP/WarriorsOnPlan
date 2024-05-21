using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vfxMovable : movableObject, IMovableSupplement {
    public enumVFXType thisVFXType;
    private ParticleSystem thisPS;

    public new void Awake() {
        base.Awake();
        thisPS = gameObject.GetComponent<ParticleSystem>();
        thisPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }


    public void whenStartMove() {
        thisPS.Play();
    }

    public void whenEndMove() {
        thisPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        combatManager.CM.FC.retrieve(this);
    }

    
}
