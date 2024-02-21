using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toolWeapon : toolTimed
{
    //range of toolWeapon consists of two float nums. each index represents minimum range and maximum range. most toolWeapon's min range is 0.
    protected readonly float[] rangeOriginal;
    protected readonly int damageOriginal;
    public float[] rangeCur { get; set; }
    public int damageCur { get; set; }
}
