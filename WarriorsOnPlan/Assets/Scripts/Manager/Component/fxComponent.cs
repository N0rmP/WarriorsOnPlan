using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enumVFX { 
    none,
    projectile_simple,
    hit_simple
}

public class fxComponent {
    private carrierGeneric<GameObject> carrierProjectileSimple;
    private carrierGeneric<GameObject> carrierHitSimple;

    private GameObject prefabSimpleProjectile;
    private GameObject prefabHit;

    private retrieverMoveStop instRetrieverMoveStop;
    private retrieverParticleStop instRetrieverParticleStop;
    private retrieverTimer instRetrieverTimer;
    private retrieverManual instRetrieverManual;

    public fxComponent() {
        prefabSimpleProjectile = Resources.Load<GameObject>("Prefab/VFX/vfxSimple");
        prefabHit = Resources.Load<GameObject>("Prefab/VFX/vfxHit");

        instRetrieverMoveStop = new retrieverMoveStop();
        instRetrieverParticleStop = new retrieverParticleStop();
        instRetrieverTimer = new retrieverTimer();
        instRetrieverManual = new retrieverManual();

        makeCarriers();
    }

    private void makeCarriers() {
        carrierProjectileSimple = new carrierGeneric<GameObject>(
            () => {
                GameObject tempObj = GameObject.Instantiate(prefabSimpleProjectile);
                tempObj.GetComponent<vfxMovable>().thisVFXType = enumVFX.projectile_simple;
                return tempObj;
            }
            );
        carrierHitSimple = new carrierGeneric<GameObject>(
            () => {
                GameObject tempObj = GameObject.Instantiate(prefabHit);
                tempObj.GetComponent<vfxMovable>().thisVFXType = enumVFX.hit_simple;
                return tempObj;
            }
            );
    }

    #region vfx
    public void callVFX(enumVFX parEnumVFX, retrieverAbst parRetriever, Vector3 parPosition, Color? parColor = null) {
        callVFX(parEnumVFX, parRetriever, parPosition, parPosition, enumMoveType.stationary, parColor, 0f);
    }

    // please set parEnumMoveType to enumMoveType.stationary to make vfxMovable not to move
    public void callVFX(enumVFX parEnumVFX, retrieverAbst parRetriever, Vector3 parDeparture, Vector3 parDestination, enumMoveType parEnumMoveType, Color? parColor = null, float parMoveTime = 1.5f) {
        vfxMovable tempVFXMovable = getCarrierVFX(parEnumVFX).getInterceptor().GetComponent<vfxMovable>();

        if (parColor is not null) {
            tempVFXMovable.setColor(parColor ?? Color.white);
        }
        tempVFXMovable.playParticle();        
        tempVFXMovable.transform.position = parDeparture;

        // make vfx move
        if (parEnumMoveType != enumMoveType.stationary) {
            Action<Vector3, float> tempDelegateMove = parEnumMoveType switch {
                enumMoveType.linear => tempVFXMovable.startLinearMove,
                enumMoveType.parabola => tempVFXMovable.startParabolaMove,
                _ => null
            };
            tempDelegateMove(parDestination, parMoveTime / (float)Math.Clamp(combatManager.CM.combatSpeed, 1, 3));
        }

        // setting retriever is lattest because retriever can stop vfx immediately with several variables not set yet
        tempVFXMovable.thisRetriever = parRetriever;    
    }

    private carrierGeneric<GameObject> getCarrierVFX(enumVFX parEnumVFX) {
        return (parEnumVFX switch {
            enumVFX.projectile_simple => carrierProjectileSimple,
            enumVFX.hit_simple => carrierHitSimple,
            _ => carrierProjectileSimple
        });
    }

    public void retrieve(vfxMovable parVFX) {
        getCarrierVFX(parVFX.thisVFXType).returnSingle(parVFX.gameObject);
        parVFX.transform.position = new Vector3(50f, 0f, 50f);
    }

    public void retrieveAll() {
        carrierProjectileSimple.returnTotal();
        carrierHitSimple.returnTotal();
    }

    public void destroy(vfxMovable parVFX) {
        getCarrierVFX(parVFX.thisVFXType).destroySingle(parVFX.gameObject);
    }
    #endregion vfx

    #region getRetriever
    // getRetriever is not made as factory method because of some exceptions

    public retrieverAbst getRetrieverMoveStop() {
        return instRetrieverMoveStop.getRetriever();
    }

    public retrieverAbst getRetrieverParticleStop() {
        return instRetrieverParticleStop.getRetriever();
    }

    // getRetrieverTimer requires float argument for timer initiation
    public retrieverAbst getRetrieverTimer(float parTimer) {
        retrieverTimer tempRetrieverTimer = instRetrieverTimer.getRetriever() as retrieverTimer;
        tempRetrieverTimer.setTimer(parTimer);
        return tempRetrieverTimer;
    }

    // getRetrieverManual returns retrieverManual not retrieverAbst, to ensure caller can call retrieverManual.retrieve()
    public retrieverManual getRetrieverManual() {
        return instRetrieverManual.getRetriever() as retrieverManual;
    }
    #endregion getRetriever

}
