using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vfxMovable : movableObject {
    // isDirty becomes true when this vfxMovable can't be reused due to color change, particle system revision etc.
    private bool isDirty;

    private enumVFX thisVFXType_ = enumVFX.none;
    public enumVFX thisVFXType {
        get {
            return thisVFXType_;
        }
        set {
            if (thisVFXType_ != enumVFX.none) {
                return;
            }
            thisVFXType_ = value;
        }
    }
    private ParticleSystem thisPS;

    private retrieverAbst thisRetriever_;
    public retrieverAbst thisRetriever {
        get {
            return thisRetriever_;
        }
        set {
            if (thisRetriever_ == null) {
                thisRetriever_ = value;
                thisRetriever_.doWhenAdded(this);
            }
        }
    }

    public new void Awake() {
        base.Awake();
        isDirty = false;
        thisPS = gameObject.GetComponent<ParticleSystem>();
        thisPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        thisRetriever_ = null;
    }

    public new void Update() {
        base.Update();
        if (thisRetriever != null && thisRetriever.checkRetrieve(this)) {
            retrieveVFX();
        }
    }

    public void playParticle() {
        thisPS.Play(); 
    }

    public void retrieveVFX() {
        IEnumerator tempFunc() {
            thisRetriever_ = null;
            transform.position = new Vector3(50f, 50f, 50f);
            yield return new WaitForSeconds(2f);
            if (isDirty) {
                combatManager.CM.FC.destroy(this);
            } else {
                combatManager.CM.FC.retrieve(this);
                thisPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        };

        StartCoroutine(tempFunc());
    }

    public void setColor(Color parColor) {
        isDirty = true;
        ParticleSystem.MainModule tempParticleMain = thisPS.main;
        tempParticleMain.startColor = parColor;
    }
}
