using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cases;

public class grabberBasic : IWeaponGrabber {
    private Transform bufferTemporarizedWeapon = null;
    private Transform bufferTemporarizedWeaponHome = null;

    public void init(Transform[] parContainerHand, Transform parContainerBack) { }

    public void grabWeapon(Transform parObjWeapon, Transform[] parContainerHand, Transform parContainerBack) {
        // grab in hand
        for (int i = 0; i < parContainerHand.Length; i++) {
            if (parContainerHand[i].childCount == 0) {
                grabInHand(parObjWeapon, parContainerHand[i]);
                return;
            }
        }

        // grab on back
        grabOnBack(parObjWeapon, parContainerBack);
    }

    // find parObjWeapon in containers and remove it, and each weapon goes 1 slot ahead beacause one ObjWeapon removed
    public void dropWeapon(Transform parObjWeapon, Transform[] parContainerHand, Transform parContainerBack) {
        bool tempIsRemoved = false;

        // find and remove (from hand)
        for (int i = 0; i < parContainerHand.Length; i++) {
            if (parContainerHand[i].childCount > 0 && parContainerHand[i].GetChild(0) == parObjWeapon) {
                removeWeapon(parObjWeapon);
                tempIsRemoved = true;
                break;
            }
        }

        // find and remove (from back)
        if (!tempIsRemoved) {
            foreach (Transform tr in parContainerBack) {
                if (tr == parObjWeapon) {
                    removeWeapon(parObjWeapon);
                    break;
                }
            }
        }

        // rearrange
        // rearrange (hand)
        Transform tempNext;
        for (int i = 0; i < parContainerHand.Length; i++) {
            if (parContainerHand[i].childCount == 0) {
                tempNext = (i < parContainerHand.Length - 1) ?
                (parContainerHand[i + 1].childCount > 0 ? parContainerHand[i + 1].GetChild(0) : null) :
                (parContainerBack.childCount > 0 ? parContainerBack.GetChild(0) : null);
                if (tempNext != null) {
                    grabInHand(tempNext, parContainerHand[i]);
                } else {
                    // it means no weapon after this slot that two slots are empty in streak
                    return;
                }
            }
        }

        //rearrange (back)
        for (int i = 0; i < parContainerBack.childCount; i++) {
            grabOnBack(parContainerBack.GetChild(i), parContainerBack);
        }
    }

    public void grabTemporaryWeapon(Transform parObjWeapon, Transform[] parContainerHand, Transform parContainerBack) {
        bufferTemporarizedWeapon = parContainerHand[0].GetChild(0);
        bufferTemporarizedWeaponHome = parContainerHand[0];
        removeWeapon(bufferTemporarizedWeapon);
        grabInHand(parObjWeapon, parContainerHand[0]);
    }

    public void dropTemporaryWeapon() {
        if (bufferTemporarizedWeapon == null) {
            return;
        }

        removeWeapon(bufferTemporarizedWeaponHome.GetChild(0));
        grabInHand(bufferTemporarizedWeapon, bufferTemporarizedWeaponHome);
        bufferTemporarizedWeapon = bufferTemporarizedWeaponHome = null;
    }

    private void grabInHand(Transform parTransformWeapon, Transform parTransformHand) {
        parTransformWeapon.SetParent(parTransformHand);
        parTransformWeapon.SetAsFirstSibling();
        parTransformWeapon.localPosition = Vector3.zero;
        parTransformWeapon.localRotation = Quaternion.Euler(Vector3.zero);
    }

    private void grabOnBack(Transform parTransformWeapon, Transform parTransformBack) {
        parTransformWeapon.SetParent(parTransformBack);
        parTransformWeapon.localPosition = Vector3.zero;
        parTransformWeapon.position += new Vector3(0f, parTransformWeapon.transform.GetSiblingIndex() * 0.1f, 0f);
        parTransformWeapon.localRotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(2, 5) * -30f, 0f, 90f));
    }

    private void removeWeapon(Transform parTransformWeapon) {
        parTransformWeapon.SetParent(null);
        parTransformWeapon.position = new Vector3(-20f, 0f - 20f);
    }
}
