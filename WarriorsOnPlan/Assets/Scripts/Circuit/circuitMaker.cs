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
    public static selecterAbst makeSelecter(int parCode, int[] pp) {
        switch (parCode) {
            case 0:
                return new selecterClosest(pp[0]);
            case -1:
            default:
                return null;
        }
    }
    #endregion makeSelecter

    #region makeSensor
    /*
        words in paranthesis means parameters required
        -1  =   null
        0   =   sensorNothing ()
    */
    public static sensorAbst makeSensor(int parCode, int[] pp) {
        switch (parCode) {
            case 0:
                return new sensorNothing();
            case -1:
            default:
                return null;
        }
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
        switch (parCode) {
            case 0:
                return new navigatorStationary();
            case 1:
                return new navigatorAttackOneWeapon();
            case -1:
            default:
                return null;
        }
    }
    #endregion makeNavigator
}
