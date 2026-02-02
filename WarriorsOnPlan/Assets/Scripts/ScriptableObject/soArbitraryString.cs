using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dataArbitraryString_default", menuName = "ScriptabbleObject/soArbitraryString", order = 0)]
public class soArbitraryString : ScriptableObject, IDataInsurance {
    public string SwissArmyString;

    public void emergencyInit() {
        SwissArmyString = "dataArbitraryString error";
    }
}