using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cases;

public interface IWeaponGrabber {
    public void init(Transform[] parContainerHand, Transform parContainerBack);
    public void grabWeapon(Transform parObjWeapon, Transform[] parContainerHand, Transform parContainerBack);
    public void dropWeapon(Transform parObjWeapon, Transform[] parContainerHand, Transform parContainerBack);
    public void grabTemporaryWeapon(Transform parObjWeapon, Transform[] parContainerHand, Transform parContainerBack);
    public void dropTemporaryWeapon();
}
