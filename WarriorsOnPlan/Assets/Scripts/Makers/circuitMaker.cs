using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circuitMaker
{
    //  pp means 'par parameters'

    #region makeSelecter
    /*
        words in paranthesis means parameters required
        -1  =   null
        0   =   selecterClosest (target group bits)
    */
    public static selecterAbst makeSelecter(Thing source, int parCode, int[] pp) {
        return parCode switch {
            0 => new selecterClosest(source, pp[0]),
            -1 => null,
            _ => null
        };
    }
    #endregion makeSelecter

    #region makeSensor
    /*
        words in paranthesis means parameters required
        -1  =   null
        0   =   sensorNothing ()
    */
    public static sensorAbst makeSensor(int parCode, int[] pp) {
        return parCode switch {
            0 => new sensorNothing(),
            1 => new sensorHp(pp[0], pp[1], pp[2] != 0),
            -1 => null,
            _ => null
        };
    }
    #endregion makeSensor

    #region makeNavigator
    /*
        words in paranthesis means parameters required
        -1  =   null
        0   =    navigatorStationary ()
        1   =   navigatorAttackOneWeapon ()
    */
    public static navigatorAbst makeNavigator(int parCode, int[] pp) {
        return parCode switch {
            0 => new navigatorStationary(),
            1 => new navigatorAttackOneWeapon(),
            -1 => null,
            _ => null
        };
    }
    #endregion makeNavigator
}
