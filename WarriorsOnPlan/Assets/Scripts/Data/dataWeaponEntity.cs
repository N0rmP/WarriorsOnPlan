using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct dataWeaponEntity {
    public string Name;
    public int RangeMin;
    public int RangeMax;
    public int TimerMax;
    public enumDamageType DamageType;
    public enumAnimationType AnimationType;
}
