using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toolWeapon : toolTimed
{
    //range of toolWeapon consists of two float nums. each index represents minimum range and maximum range. most toolWeapon's min range is 0.
    protected readonly int rangeMin_;
    protected readonly int rangeMax_;
    protected readonly int damageOriginal;
    public int rangeMin { 
        get {
            return rangeMin_;
        }
    }
    public int rangeMax {
        get {
            return rangeMax_;
        }
    }
    public int damageCur { get; set; }
}
