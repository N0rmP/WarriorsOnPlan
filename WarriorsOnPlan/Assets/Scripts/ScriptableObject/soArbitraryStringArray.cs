using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dataArbitraryStringArray_default", menuName = "ScriptabbleObject/soArbitraryStringArray")]
public class soArbitraryStringArray : ScriptableObject, IDataInsurance {
    public string[] SwissArmyStringArray;

    public void emergencyInit() {
        SwissArmyStringArray = new string[4];
        Array.Fill(SwissArmyStringArray, "dataArbitraryStringArray error");
    }
}